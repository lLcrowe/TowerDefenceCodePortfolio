using UnityEngine;
namespace lLCroweTool
{
    /// <summary>
    /// 게임데이터매니저
    /// 중앙 데이처 처리를 위한 그룹관리를 위한 구역    
    /// </summary>
    [CreateAssetMenu(fileName = "GameDataManager", menuName = "lLcroweTool/GameDataManager")]
    public class GameDataManager : ScriptableObject
    {
        //에디터윈도우로 처리하는게 맞아보이는데



        private static GameDataManager instance;
        public static GameDataManager Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    instance = FindObjectOfType<GameDataManager>();
                    //if (ReferenceEquals(instance, null))
                    if (ReferenceEquals(instance, null))
                    {
                        instance = Resources.Load<GameDataManager>("GameDataManager");
                        if (instance == null)
                        {
                            Debug.LogError("게임데이터매니저 파일을 제작해주세요.\n게임데이터들이 있는 Root폴더를 경로로 지정해야함");
                        }
                    }
                }
                return instance;
            }
        }

        //리소스 폴더에서 다른시스템의 설정을 담당
        //빌드할시에는 안들어가게?
        //파일에대한걸 불려올때도 string으로 처리하는거 같은데 글썌
        //인덱싱?


        //경로
        //파일이름

        
        //읽을때는 리소스폴더 사용//쓸때는 
        //게임데이터들이 있는 Root폴더를 경로로 지정해야함
        [Header("게임데이터매니저 쓰기 파일경로")]
        [SerializeField] private string gameCSVWritterFilePath;

        //각파일 데이터 읽기 쓰기 파일&폴더 경로

        [Header("게임GCV데이터 읽기쓰기 파일경로")]
        [Tooltip("현재 스크립터블오브젝트가 있는 경로를 칭해야함")]
        [SerializeField] private string gameCSVFilePath = "GameData";

        [ButtonMethod]
        public void GetCurFilePath()
        {
            gameCSVWritterFilePath = Application.dataPath;
        }

       
        //void Test()
        //{
        //    //타겟팅 될 아이디 값, 밸류값
        //    //라인이 있는 만큼 들어감
        //    List<Dictionary<string, object>> data_Dialog = CSVReader.Read("TestQuest - TestQuest");

        //    //로우들을 체크 i => 줄
        //    for (int i = 0; i < data_Dialog.Count; i++)
        //    {
        //        Debug.Log(data_Dialog[i]["Quest제목"].ToString());
        //    }
        //    for (int i = 0; i < data_Dialog.Count; i++)
        //    {
        //        Debug.Log(data_Dialog[i]["Quest내용"].ToString());
        //    }


        //    List<string[]> rowDataList = new List<string[]>();
        //    string localpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//현재 유저의 바탕화면 위치

            

        //    //라인생성
        //    //string[] target = { id, value1, value2 };

        //    //라인집어넣기//첫번쨰라인은 키값으로 봐줘야함
        //    //rowDataList.Add(target);

        //    Debug.Log(Application.dataPath);//현재 프로젝트의 에셋경로로 지정됨//고로 asset/ 뭐시기로 해주면됨


        //    //CSVWritter.WriteCsv(rowDataList, localpath, "targetTest");
        //}


        public string GetGameCSVFilePath()
        {
            return gameCSVFilePath;
        }

        public string GetWritterCSVFilePath()
        {
            return gameCSVWritterFilePath;
        }


    }
   
}
