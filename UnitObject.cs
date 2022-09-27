using UnityEngine;
using lLCroweTool.TimerSystem;
using lLCroweTool.DestroyManger;
using lLCroweTool.ScoreSystem;

namespace Assets
{
    [RequireComponent(typeof(TimerModule))]
    public class UnitObject : MonoBehaviour
    {
        //���ֿ�����Ʈ
        public TeamType teamType;
        public UnitStatusInfo unitStatus;

        //���ط� ���� = (�⺻ ���ݷ� + ����Ƚ�� * ���� ���ݷ�) + (�߰� ���ݷ� + ����Ƚ�� * ���� ���ݷ�) - �� ������ ����

        //�ǹ��� ���� �� �ǹ��� �⺻ HP�� �� HP�� 10% ���� �����Ѵ�.
        //�ǹ��� ���� �� �ǹ��� ������ 0. ���׷��̵� �߿��� ���׷��̵� �� �ǹ��� HP�� ������ ���Ѵ�.

        //�ʴ� 0.2734 ȸ��, ü�� 1ȸ���� �� 3.66�� �Ҹ� (���� ���� 0.38276 ȸ��, 2.61��)
        //ƽ-> ������Ʈ�� ���� ���� ���� �Ǵ� �����̴� �ð������� ǥ���ϴ� �ð��� ª���Ͽ� ƽ���� ó��
        //���ڰ� ���ƺ��δ�.


        private TimerModule timerModule;

        private void Awake()
        {
            timerModule = GetComponent<TimerModule>();
            timerModule.SetTimer(0.02f);
            timerModule.AddUnityEvent(delegate { UpdateUnitObject(this); });
        }

        /// <summary>
        /// ���� ������Ʈ �Լ�
        /// </summary>
        private static void UpdateUnitObject(UnitObject unitObject)
        {
            if (unitObject.unitStatus.healthCurPoint <= 0)
            {
                //����ǰ�����Ʈ�� 0�����۰ų� ������
                //���ó��
                UnitObjectDead(unitObject);
            }
            if (unitObject.unitStatus.healthRatePoint <= 0)
            {
                return;
            }

            //ƽ�� �۵���
            if (Time.time > unitObject.unitStatus.healthRateTime + unitObject.unitStatus.rateTime)
            {
                if (unitObject.unitStatus.healthCurPoint < unitObject.unitStatus.healthMaxPoint)
                {
                    //�ִ�ǰ�����Ʈ���� ������ ���
                    unitObject.unitStatus.healthCurPoint = RecoveryPoint(unitObject.unitStatus.healthCurPoint, unitObject.unitStatus.healthMaxPoint, unitObject.unitStatus.healthRatePoint);
                }
                unitObject.unitStatus.rateTime = Time.time;
            }
        }

        /// <summary>
        /// ���ȿ� ����� ���
        /// </summary>
        /// <param name="damagePoint">������ �������</param>
        /// <param name="unitStatusModule">Ÿ���̵� ���ָ��</param>
        /// <param name="actionedUnitObject">�����ϴ� ��� ���ֿ�����Ʈ</param>
        public static void UnitStatusTakeDamaged(int damagePoint, UnitObject unitObject, UnitObject actionedUnitObject)
        {
            //����� ������
            if (!ReferenceEquals(actionedUnitObject, null))
            {
                if (actionedUnitObject.TryGetComponent(out ScoreTarget scoreTarget))
                {
                    ScoreManager.Instance.AddScore(scoreTarget, ScoreType.Damage, damagePoint);
                }
            }

            UnitStatusInfo unitStatus = unitObject.unitStatus;
            var realDamage = damagePoint;

            //�Ƹ�
            realDamage -= unitStatus.armorPower;
            if (realDamage <= 0)
            {
                //������� 0�̴� ����
                return;
            }

            int tempHealthPoint = unitStatus.healthCurPoint - realDamage;
            if (tempHealthPoint <= 0)
            {
                tempHealthPoint = 0;
                //ų ������
                if (!ReferenceEquals(actionedUnitObject, null))
                {
                    if (actionedUnitObject.TryGetComponent(out ScoreTarget scoreTarget))
                    {
                        ScoreManager.Instance.AddScore(scoreTarget, ScoreType.Kill, 1);
                    }
                }
                unitStatus.healthCurPoint = tempHealthPoint;
                //����ǰ�����Ʈ�� 0�����۰ų� ������
                //���ó��
                UnitObjectDead(unitObject);
            }
            else
            {
                unitStatus.healthCurPoint = tempHealthPoint;
            }
        }

        private static void UnitObjectDead(UnitObject unitObject)
        {
            unitObject.gameObject.SetActive(false);//��Ȱ��ȭ
                                                   //����Ʈó��
                                                   //DestroyManager.Instance.AddWaitDestoryGameObject(unitObject.gameObject);//�ı�ó������
            if (unitObject.teamType == TeamType.Enemy)
            {
                if (unitObject.TryGetComponent(out Enemy enemy))
                {
                    TowerDefenceManager.Instance.AddResource(TowerDefenceManager.ResourceType.Score, enemy.enemyInfo.metal);//����ó��
                    TowerDefenceManager.Instance.AddResource(TowerDefenceManager.ResourceType.Metal, enemy.enemyInfo.metal);//����ó��
                }
            }
        }

        /// <summary>
        /// ����Ʈ ȸ�� �Լ�
        /// </summary>
        /// <param name="currentPointValue">���� ����Ʈ��</param>
        /// <param name="maxPointValue">�ִ�ġ ����Ʈ��</param>
        /// <param name="ratePointValue">ȸ��ġ ����Ʈ��</param>
        /// <returns>ȸ���� ���� ����Ʈ��</returns>
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


