using UnityEditor;
using UnityEngine;

namespace lLCroweTool
{

	public class Tag : PropertyAttribute
	{
	}
	[CustomPropertyDrawer(typeof(Tag))]
	public class TagDraw : PropertyDrawer
	{

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.String)
			{
				if (!_checked) 
				{
					Warning(property);
				}
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
		}

		private bool _checked;

		private void Warning(SerializedProperty property)
		{
			Debug.LogWarning(string.Format("프로퍼티 <color=brown>{0}</color> in object <color=brown>{1}</color> 가 잘못된 타입입니다. 스트링 체크",
				property.name, property.serializedObject.targetObject));
			_checked = true;
		}
	}
}