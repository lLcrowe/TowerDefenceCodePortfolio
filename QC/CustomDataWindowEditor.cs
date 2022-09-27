using UnityEngine;
using Assets;
#if UNITY_EDITOR
using UnityEditor;
#pragma warning disable 0618
namespace lLCroweTool.QC.EditorOnly
{
    //확인하기
    //public class CustomDataEditorShow<T> where T : ObjectDeScription_Base
    //{
    //    //해당되는 데이터
    //    private T targetData;
    //    private T targetLoadData;


    //}

    public abstract class CustomDataWindowEditor<T> : EditorWindow where T : ObjectLabelData
    {
        protected string dataContentName;//데이터컨텐츠이름
        protected bool isNewData = false;//새로운데이터인지 로드데이터인지 체크
        protected Vector2 scollPos;//스크롤 좌표
        protected string temp = "";

        //저장데이터
        protected string tag;
        protected string folderName;

        //해당되는 데이터
        protected T targetData;
        protected T targetLoadData;

        private void OnEnable()
        {
            isNewData = false;
            targetData = null;
            targetLoadData = null;
            SetDataCotentName();
        }

        /// <summary>
        /// 데이터컨텐츠이름 세팅하는 함수
        /// </summary>
        protected abstract void SetDataCotentName();

        /// <summary>
        /// 데이터리셋
        /// </summary>
        private void ResetData()
        {
            OnEnable();
        }

        /// <summary>
        /// 데이터생성시 작동되는 행동
        /// </summary>
        protected abstract void CreateDataAction();//제네릭은 new를 지정못함

        private void OnGUI()
        {
            scollPos = EditorGUILayout.BeginScrollView(scollPos, GUILayout.Height(515));

            if (DataCreateLoadSection())
            {
                EditorGUILayout.Space();
                UpDownResetAndMoreUI();
                if (!ReferenceEquals(targetData, null))
                {
                    DataDisplaySection();
                    UpDownResetAndMoreUI();
                }
            }
            EditorGUILayout.EndScrollView();
        }
        
        /// <summary>
        /// 데이터제작로드 섹션
        /// </summary>
        /// <returns>데이터대상 존재여부</returns>
        private bool DataCreateLoadSection()
        {
            EditorGUILayout.BeginVertical();

            if (targetData == null)
            {
                if (GUILayout.Button(dataContentName + "데이터 생성"))
                {   
                    CreateDataAction();
                    isNewData = true;
                }
            }
            else
            {
                if (GUILayout.Button("리셋"))
                {
                    ResetData();
                }
            }

            EditorGUILayout.BeginHorizontal();
            targetLoadData = (T)EditorGUILayout.ObjectField("현재선택된 " + dataContentName + "데이터", targetLoadData, typeof(T), true);
            if (GUILayout.Button(dataContentName + "데이터 로드"))
            {
                if (targetLoadData != null)
                {
                    targetData = Instantiate(targetLoadData);
                    targetData.name = targetLoadData.name;
                    isNewData = false;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            bool isInit = !ReferenceEquals(targetData, null);//초기화상태체크
            //존재하면
            if (isInit)
            {
                EditorGUILayout.HelpBox("현재 선택된 데이터 : " + targetData.objectName, MessageType.Info);

                if (!isNewData)
                {
                    //로드데이터일시
                    EditorGUILayout.HelpBox("현재 로드된 데이터 : " + targetLoadData.objectName + "\n현재 선택된 데이터의 이름과 다름이름으로 변경시 새로 생성됩니다.", MessageType.Warning);
                }

                if (isNewData)
                {
                    temp = "생성";
                }
                else
                {
                    if (targetLoadData == null)
                    {
                        ResetData();
                    }
                    else
                    {
                        if (targetLoadData.objectName == targetData.objectName)
                        {
                            temp = "덮어쓰기";
                        }
                        else
                        {
                            temp = "새로 생성";
                        }
                    }
                }
            }

            return isInit;
        }

        /// <summary>
        /// 데이터표시구간후와 시작부분에 보여주는 UI관련
        /// </summary>
        private void UpDownResetAndMoreUI()
        {
            if (GUILayout.Button(temp + " " + dataContentName + "데이터"))
            {
                SetSaveFileData();//폴더이름//태그
                CreateDataObject(targetData, targetData.objectName, folderName, tag);
            }
        }

        /// <summary>
        /// 저장하기 전 폴더이름과 태그를 설정하는 함수
        /// </summary>
        protected abstract void SetSaveFileData();

        /// <summary>
        /// 데이터표시구간
        /// </summary>
        protected abstract void DataDisplaySection();

        //테스트용
        //해당위치에 에셋이 존재하느지 여부
        //public static bool IsExist(string sAssetPath)
        //{
        //    if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(sAssetPath))) return false;
        //    return true;
        //}

        /// <summary>
        /// 데이터생성 함수
        /// </summary>
        /// <param name="dataObject">해당되는 데이터</param>
        /// <param name="fileName">데이터파일 이름</param>
        /// <param name="folderName">지정할 폴더이름</param>
        /// <param name="tag">꼬리표</param>
        public static void CreateDataObject(Object dataObject, string fileName, string folderName, string tag)
        {
            if (fileName == "")
            {
                Debug.Log("데이터오브젝트 이름을 정해주세요");
                return;
            }


            //데이터에셋 새롭게 만들기
            //해당폴더가 있는가?
            if (!AssetDatabase.IsValidFolder("Assets/A1"))
            {
                //폴더가 없으면
                Debug.Log("\n" + "경로에 해당되는 폴더가 없어서 폴더를 추가시켯했습니다.");
                AssetDatabase.CreateFolder("Assets", "A1");
            }

            //해당폴더가 있는가?
            if (!AssetDatabase.IsValidFolder("Assets/A1/GameData"))
            {
                //폴더가 없으면
                Debug.Log("\n" + "경로에 해당되는 폴더가 없어서 폴더를 추가시켯했습니다.");
                AssetDatabase.CreateFolder("Assets/A1", "GameData");
            }

            //해당폴더가 있는가?
            string content = "Assets/A1/GameData/";
            if (!string.IsNullOrEmpty(folderName))
            {
                if (!AssetDatabase.IsValidFolder("Assets/A1/GameData/" + folderName))
                {
                    //폴더가 없으면
                    Debug.Log("\n" + "경로에 해당되는 폴더가 없어서 폴더를 추가시켯했습니다.");
                    AssetDatabase.CreateFolder("Assets/A1/GameData", folderName);
                }
                content += folderName + "/";
            }

            if (string.IsNullOrEmpty(tag))
            {
                content += fileName + ".asset";
            }
            else
            {
                content += fileName + "_" + tag + ".asset";
            }

            //이미 존재하면 덮어쓰기로
            Object overWriteObject = AssetDatabase.LoadMainAssetAtPath(content);
            if (overWriteObject == null)
            {
                //존재하지않으면 
                //생성
                AssetDatabase.CreateAsset(dataObject, content);
                overWriteObject = dataObject;
            }
            else
            {
                //존재하면
                //복사 및 교체되지만 링크는 유지//덮어쓰기
                string name = overWriteObject.name;//전에 쓰던이름을 가져와서 새로 쓸오브젝트에 대입시켜줘야됨
                EditorUtility.CopySerializedIfDifferent(dataObject, overWriteObject);
                overWriteObject.name = name;
            }

            //생성
            //AssetDatabase.CreateAsset(dataObject, content);
            AssetDatabase.SaveAssets();//저장되지 않은 모든 자산 변경 사항을 디스크에 씁니다.
            AssetDatabase.Refresh();//새로고침
            Selection.activeObject = overWriteObject;//선택
            //Debug.Log(tag + " " + dataObject.name + "가 추가 되었습니다." + "\n" + "경로는 Assets/A1/GameData/" + folderName + "/ 입니다");
            EditorUtility.DisplayDialog("저장 안내창", overWriteObject.name + "가 추가 되었습니다." + "\n" + "경로는 " + content + " 입니다", "OK");
        }

    }
}
#endif
