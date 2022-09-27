//using System.Collections;
//using UnityEditor;
//using UnityEngine;

//namespace lLCroweTool.QC.EditorOnly
//{
//    [CustomEditor(typeof(T))]
//    public abstract class CustomDataInspecterEditor<T> : Editor where T : Component
//    {
//        //인스팩터에디터 제너릭화
//        //불가능//CustomEditor 어트리뷰트는 T를 사용못함//제작이 힘듬

//        protected T targetEditor;

//        private void OnEnable()
//        {
//            targetEditor = (T)target;
//        }


//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            EditorGUILayout.LabelField("-=" + typeof(T).Name + " 인스팩터에디터" + "=-");
//            DataDisplaySection();
//        }

//        /// <summary>
//        /// 데이터표시구간
//        /// </summary>
//        protected abstract void DataDisplaySection();

//    }
//}