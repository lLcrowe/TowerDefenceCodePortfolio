using System.Collections;
using UnityEngine;

namespace lLCroweTool.ScoreSystem
{
    public class ScoreTarget : MonoBehaviour
    {
        //점수를 올리고 기록할때 사용하는 컴포넌트
        //유닛월드오브젝트에 부착
        //히트와 어택에서 업데이트시켜줌
       
        [SerializeField] private int killScore;//죽인수//완료
        [SerializeField] private int damageScore;//딜수//완료        
        [SerializeField] private int hitScore;//맞은수//완료
        [SerializeField] private int hillScore;//회복 수//완료
        [SerializeField] private int shildChargeScore;//쉴드회복 수//완료

        public void SetKillScore(int value)
        {
            killScore = value;
        }

        public int GetKillScore()
        {
            return killScore;
        }

        public void SetDamageScore(int value)
        {
            damageScore = value;
        }

        public int GetDamageScore()
        {
            return damageScore;
        }

        public void SethitScore(int value)
        {
            hitScore = value;
        }

        public int GethitScore()
        {
            return hitScore;
        }
        public void SetHillScore(int value)
        {
            hillScore = value;
        }

        public int GetHillScore()
        {
            return hillScore;
        }
        public void SetShildChargeScore(int value)
        {
            shildChargeScore = value;
        }

        public int GetShildChargeScore()
        {
            return shildChargeScore;
        }
    }
}