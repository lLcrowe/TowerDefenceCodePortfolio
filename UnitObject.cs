using UnityEngine;
using lLCroweTool.TimerSystem;
using lLCroweTool.DestroyManger;
using lLCroweTool.ScoreSystem;

namespace Assets
{
    [RequireComponent(typeof(TimerModule))]
    public class UnitObject : MonoBehaviour
    {
        //유닛오브젝트
        public TeamType teamType;
        public UnitStatusInfo unitStatus;

        //피해량 계산식 = (기본 공격력 + 공업횟수 * 공업 공격력) + (추가 공격력 + 공업횟수 * 공업 공격력) - 적 유닛의 방어력

        //건물을 지을 때 건물의 기본 HP는 총 HP의 10% 에서 시작한다.
        //건물을 지을 때 건물의 방어력은 0. 업그레이드 중에는 업그레이드 전 건물의 HP와 방어력을 지닌다.

        //초당 0.2734 회복, 체력 1회복에 약 3.66초 소모 (공유 기준 0.38276 회복, 2.61초)
        //틱-> 업데이트당 으로 봐도 무방 또는 움직이는 시간당으로 표현하니 시간을 짧게하여 틱으로 처리
        //후자가 나아보인다.


        private TimerModule timerModule;

        private void Awake()
        {
            timerModule = GetComponent<TimerModule>();
            timerModule.SetTimer(0.02f);
            timerModule.AddUnityEvent(delegate { UpdateUnitObject(this); });
        }

        /// <summary>
        /// 유닛 업데이트 함수
        /// </summary>
        private static void UpdateUnitObject(UnitObject unitObject)
        {
            if (unitObject.unitStatus.healthCurPoint <= 0)
            {
                //현재건강포인트가 0보다작거나 같으면
                //사망처리
                UnitObjectDead(unitObject);
            }
            if (unitObject.unitStatus.healthRatePoint <= 0)
            {
                return;
            }

            //틱당 작동됨
            if (Time.time > unitObject.unitStatus.healthRateTime + unitObject.unitStatus.rateTime)
            {
                if (unitObject.unitStatus.healthCurPoint < unitObject.unitStatus.healthMaxPoint)
                {
                    //최대건강포인트보다 작으면 재생
                    unitObject.unitStatus.healthCurPoint = RecoveryPoint(unitObject.unitStatus.healthCurPoint, unitObject.unitStatus.healthMaxPoint, unitObject.unitStatus.healthRatePoint);
                }
                unitObject.unitStatus.rateTime = Time.time;
            }
        }

        /// <summary>
        /// 스탯에 대미지 계산
        /// </summary>
        /// <param name="damagePoint">순수한 대미지량</param>
        /// <param name="unitStatusModule">타겟이될 유닛모듈</param>
        /// <param name="actionedUnitObject">공격하는 대상 유닛오브젝트</param>
        public static void UnitStatusTakeDamaged(int damagePoint, UnitObject unitObject, UnitObject actionedUnitObject)
        {
            //대미지 점수업
            if (!ReferenceEquals(actionedUnitObject, null))
            {
                if (actionedUnitObject.TryGetComponent(out ScoreTarget scoreTarget))
                {
                    ScoreManager.Instance.AddScore(scoreTarget, ScoreType.Damage, damagePoint);
                }
            }

            UnitStatusInfo unitStatus = unitObject.unitStatus;
            var realDamage = damagePoint;

            //아머
            realDamage -= unitStatus.armorPower;
            if (realDamage <= 0)
            {
                //대미지가 0이니 멈춤
                return;
            }

            int tempHealthPoint = unitStatus.healthCurPoint - realDamage;
            if (tempHealthPoint <= 0)
            {
                tempHealthPoint = 0;
                //킬 점수업
                if (!ReferenceEquals(actionedUnitObject, null))
                {
                    if (actionedUnitObject.TryGetComponent(out ScoreTarget scoreTarget))
                    {
                        ScoreManager.Instance.AddScore(scoreTarget, ScoreType.Kill, 1);
                    }
                }
                unitStatus.healthCurPoint = tempHealthPoint;
                //현재건강포인트가 0보다작거나 같으면
                //사망처리
                UnitObjectDead(unitObject);
            }
            else
            {
                unitStatus.healthCurPoint = tempHealthPoint;
            }
        }

        private static void UnitObjectDead(UnitObject unitObject)
        {
            unitObject.gameObject.SetActive(false);//비활성화
                                                   //이팩트처리
                                                   //DestroyManager.Instance.AddWaitDestoryGameObject(unitObject.gameObject);//파괴처리여부
            if (unitObject.teamType == TeamType.Enemy)
            {
                if (unitObject.TryGetComponent(out Enemy enemy))
                {
                    TowerDefenceManager.Instance.AddResource(TowerDefenceManager.ResourceType.Score, enemy.enemyInfo.metal);//점수처리
                    TowerDefenceManager.Instance.AddResource(TowerDefenceManager.ResourceType.Metal, enemy.enemyInfo.metal);//점수처리
                }
            }
        }

        /// <summary>
        /// 포인트 회복 함수
        /// </summary>
        /// <param name="currentPointValue">현재 포인트량</param>
        /// <param name="maxPointValue">최대치 포인트량</param>
        /// <param name="ratePointValue">회복치 포인트량</param>
        /// <returns>회복된 현재 포인트량</returns>
        private static int RecoveryPoint(int currentPointValue, int maxPointValue, int recoveryPointValue)
        {
            currentPointValue += recoveryPointValue;
            if (currentPointValue > maxPointValue)
            {
                currentPointValue = maxPointValue;
            }
            return currentPointValue;
        }

        public enum TeamType
        {
            Player,
            Enemy,
        }
    }
}


