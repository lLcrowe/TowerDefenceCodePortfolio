﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace lLCroweTool
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonMethodAttribute : PropertyAttribute
    {
        public readonly ButtonMethodDrawOrder DrawOrder;

        public ButtonMethodAttribute(ButtonMethodDrawOrder drawOrder = ButtonMethodDrawOrder.AfterInspector)
        {
            DrawOrder = drawOrder;
        }

    }
    public enum ButtonMethodDrawOrder
    {
        BeforeInspector,
        AfterInspector
    }

#if UNITY_EDITOR
	
	

	public class ButtonMethodHandler
	{
		public readonly List<(MethodInfo Method, string Name, ButtonMethodDrawOrder order)> TargetMethods;
		public int Amount => TargetMethods?.Count ?? 0;

		private readonly Object _target;

		public ButtonMethodHandler(Object target)
		{
			_target = target;

			var type = target.GetType();
			var bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			var members = type.GetMembers(bindings).Where(IsButtonMethod);

			foreach (var member in members)
			{
				var method = member as MethodInfo;
				if (method == null) continue;

				if (IsValidMember(method, member))
				{
					var attribute = (ButtonMethodAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonMethodAttribute));
					if (TargetMethods == null) TargetMethods = new List<(MethodInfo, string, ButtonMethodDrawOrder)>();
					TargetMethods.Add((method, SplitCamelCase(method.Name) , attribute.DrawOrder));
				}
			}
		}

		public void OnBeforeInspectorGUI()
		{
			if (TargetMethods == null) return;

			foreach (var method in TargetMethods)
			{
				if (method.order != ButtonMethodDrawOrder.BeforeInspector) continue;

				if (GUILayout.Button(method.Name)) InvokeMethod(_target, method.Method);
			}

			//GUILayout.Space(5);
			//EditorGUILayout.Space();
		}

		public void OnAfterInspectorGUI()
		{
			if (TargetMethods == null) return;
			//GUILayout.Space(5);
			//EditorGUILayout.Space();

			foreach (var method in TargetMethods)
			{
				if (method.order != ButtonMethodDrawOrder.AfterInspector) continue;

				if (GUILayout.Button(method.Name)) InvokeMethod(_target, method.Method);
			}
		}

		public void Invoke(MethodInfo method) => InvokeMethod(_target, method);


		private void InvokeMethod(Object target, MethodInfo method)
		{
			var result = method.Invoke(target, null);

			if (result != null)
			{
				var message = $"{result} \nResult of Method '{method.Name}' invocation on object {target.name}";
				Debug.Log(message, target);
			}
		}

		private bool IsButtonMethod(MemberInfo memberInfo)
		{
			return Attribute.IsDefined(memberInfo, typeof(ButtonMethodAttribute));
		}

		private bool IsValidMember(MethodInfo method, MemberInfo member)
		{
			if (method == null)
			{
				Debug.LogWarning(
					$"Property <color=brown>{member.Name}</color>.Reason: Member is not a method but has EditorButtonAttribute!");
				return false;
			}

			if (method.GetParameters().Length > 0)
			{
				Debug.LogWarning(
					$"Method <color=brown>{method.Name}</color>.Reason: Methods with parameters is not supported by EditorButtonAttribute!");
				return false;
			}

			return true;
		}
		private string SplitCamelCase(string inputCamelCaseString)
		{
			string sTemp = Regex.Replace(inputCamelCaseString, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
			return Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim();
		}
	}
	
#endif
}