using UnityEditor;
using UnityEngine;

namespace lLCroweTool.QC.EditorOnly
{
    public class lLcroweCustomHotKey : Editor
    {
        //Ŀ���� ����Ű
        //�޴������ۿ� �ִ� �������� �����ͼ� ������ _ ���� ���ϴ� ����Ű�� ���� ��
        //[MenuItem("GameObject/ActiveToggle _a")]
        [MenuItem("GameObject/ActiveToggle _`")]
        private static void SelectGameObjectActiveAndDeActive()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                go.SetActive(!go.activeSelf);
            }   
        }
    }

}
