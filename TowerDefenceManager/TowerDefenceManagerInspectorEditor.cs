
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using lLCroweTool;
using static lLCroweTool.QC.EditorOnly.DataCSVEditor;
using static TowerDefenceManager;

namespace Assets.EditorOnly
{
    [CustomEditor(typeof(TowerDefenceManager))]
    public class TowerDefenceManagerInspectorEditor : Editor
    {
        TowerDefenceManager towerDefenceManager;

        private List<Dictionary<string, object>> targetWaveInfoCSVDataBible = new List<Dictionary<string, object>>();

        private List<Dictionary<string, object>> targetUpgradeInfoCSVDataBible = new List<Dictionary<string, object>>();

        private static string waveInfofileName = "TowerDefenceEnemyWaveInfo";
        private static string upgradeInfofileName = "TowerDefenceUpgradeInfo";
        private string path = "Resources";
        private bool isExist1 = false;
        private bool isExist2 = false;


        private void OnEnable()
        {
            towerDefenceManager = (TowerDefenceManager)target;
        }

        public void GetData()
        {
            targetWaveInfoCSVDataBible.Clear();
            targetUpgradeInfoCSVDataBible.Clear();
            //현재는 리소스에서됨
            targetWaveInfoCSVDataBible = CSVReader.Read("", waveInfofileName, ref isExist1);
            targetUpgradeInfoCSVDataBible = CSVReader.Read("", upgradeInfofileName, ref isExist2);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            waveInfofileName = EditorGUILayout.TextField("웨이브 파일이름", waveInfofileName);

            upgradeInfofileName = EditorGUILayout.TextField("업그레이드 파일이름", upgradeInfofileName);
            path = EditorGUILayout.TextField("경로", path);

            if (GUILayout.Button("CSV데이터 새로고침"))
            {
                GetData();
            }

            if (!isExist1)
            {
                EditorGUILayout.HelpBox("경로에 웨이브데이터파일이 존재하지 않습니다.", MessageType.Warning);
            }

            if (!isExist2)
            {
                EditorGUILayout.HelpBox("경로에 업글데이터파일이 존재하지 않습니다.", MessageType.Warning);
            }


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("데이터 불려오기"))
            {
                //CSV데이터 불려오기
                EnemyWaveInfoDataBible enemyInfoDataBible = towerDefenceManager.enemyWaveInfoDataBible;
                enemyInfoDataBible.Clear();

                //줄수에맞게 카운트
                for (int i = 0; i < targetWaveInfoCSVDataBible.Count; i++)
                {
                    int index = i;

                    //존재하지않는가
                    //존재하지않으면 추가
                    EnemyWaveInfo enemyInfoData = new EnemyWaveInfo();
                    enemyInfoData.normalEnemyAmount = (int)targetWaveInfoCSVDataBible[index]["NormalEnemy"];
                    enemyInfoData.middleEnemyAmount = (int)targetWaveInfoCSVDataBible[index]["MiddleEnemy"];
                    enemyInfoData.highEnemyAmount = (int)targetWaveInfoCSVDataBible[index]["HighEnemy"];
                    enemyInfoData.waveTime = (int)targetWaveInfoCSVDataBible[index]["WaveTime"];

                    enemyInfoDataBible.Add(index, enemyInfoData);
                }

                towerDefenceManager.enemyWaveInfoDataBible = enemyInfoDataBible;
                UpgradeInfoDataBible upgradeInfoDataBible = towerDefenceManager.upgradeInfoDataBible;
                upgradeInfoDataBible.Clear();

                //줄수에맞게 카운트
                for (int i = 0; i < targetUpgradeInfoCSVDataBible.Count; i++)
                {
                    int index = i;

                    //존재하지않는가
                    //존재하지않으면 추가
                    UpgradeInfo upgradeInfoData = new UpgradeInfo();
                    upgradeInfoData.needScore = (int)targetUpgradeInfoCSVDataBible[index]["Score"];
                    upgradeInfoData.needMetal= (int)targetUpgradeInfoCSVDataBible[index]["Metal"];

                    upgradeInfoDataBible.Add(index, upgradeInfoData);
                }

                towerDefenceManager.upgradeInfoDataBible = upgradeInfoDataBible;

            }

       
            GUILayout.EndHorizontal();

         
        }
    }
}
    