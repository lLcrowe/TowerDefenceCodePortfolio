using UnityEditor;
using UnityEngine;

namespace lLCroweTool.QC.EditorOnly
{
    [CustomEditor(typeof(Object), true), CanEditMultipleObjects]
    public class UnityObjectInspectorEditor : Editor
    {
        private ButtonMethodHandler _buttonMethod;

        private void OnEnable()
        {
            if (target == null) return;

           
            _buttonMethod = new ButtonMethodHandler(target);
        }

      

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _buttonMethod?.OnBeforeInspectorGUI();

            _buttonMethod?.OnAfterInspectorGUI();
        }
    }
}