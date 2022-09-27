using System.Collections;
using UnityEngine;

namespace lLCroweTool.ScoreSystem
{
    public class ScoreManager : MonoBehaviour
    {
        private static ScoreManager instance;
        public static ScoreManager Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    instance = FindObjectOfType<ScoreManager>();
                    //if (ReferenceEquals(instance, null))
                    if (ReferenceEquals(instance, null))
                    {
                        GameObject gameObject = new GameObject();
                        instance = gameObject.AddComponent<ScoreManager>();
                        gameObject.name = "-=ScoreManager=-";
                    }
                }
                return instance;
            }
        }

        //20210720
        //스코어타겟도 같이 업데이트 해줘야함
        //전체 플레이어 점수를 뜻함
        [Header("총합점수")]
        [SerializeField] private int killScore;//죽인수//완료
        [SerializeField] private int damageScore;//딜수//완료

        private void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// 스코어타겟의 특정 점수를 올리는 함수
        /// </summary>
        public void AddScore(ScoreTarget scoreTarget, ScoreType scoreType, int value)
        {
            switch (scoreType)
            {
                case ScoreType.Kill:
                    scoreTarget.SetKillScore(scoreTarget.GetKillScore() + value);
                    break;
                case ScoreType.Damage:
                    scoreTarget.SetDamageScore(scoreTarget.GetDamageScore() + value);
                    break;                
                //case ScoreType.Hit:
                //    scoreTarget.SethitScore(scoreTarget.GethitScore() + value);
                //    break;
                //case ScoreType.Hill:
                //    scoreTarget.SetHillScore(scoreTarget.GetHillScore() + value);
                //    break;
                //case ScoreType.ShildCharge:
                //    scoreTarget.SetShildChargeScore(scoreTarget.GetShildChargeScore() + value);
                //    break;
                    
            }

            //플레이어가 조종하는 유닛인지
            //scoreTarget.TryGetComponent(out TestWorldUnitObject unitObject);
            //if (unitObject == PlayerController.Instance.GetTargetPlayerObject())
            //{
            //    //토탈점수 올림
            //    AddTotalScore(scoreType, value);
            //}


            //토탈점수 올림
            AddTotalScore(scoreType, value);
        }

        /// <summary>
        /// 총합 플레이어 점수 올리는 함수 
        /// </summary>
        private void AddTotalScore(ScoreType scoreType, int value)
        {
            switch (scoreType)
            {
                case ScoreType.Kill:
                    killScore += value;
                    break;
                case ScoreType.Damage:
                    damageScore += value;
                    break;                
                //case ScoreType.Hit:
                //    hitScore += value;
                //    break;
                //case ScoreType.Hill:
                //    hillScore += value;
                //    break;
                //case ScoreType.ShildCharge:
                //    shildChargeScore += value;
                //    break;
            }
        }

        public int GetTotalKillScore()
        {
            return killScore;
        }
        public int GetTotalDamageScore()
        {
            return damageScore;
        }
    }

    



    public enum ScoreType
    {
        //디폴트
        Kill,
        Damage,
        //Hit,
        //Hill,
        //ShildCharge,


        

        //커스텀
        //아이템줍기
        //아이템쓰기
        //유닛선택하기
        //유닛명령하기관련


    }
}