using lLCroweTool;
using lLCroweTool.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class Turret : UnitObject_Base
    {
        //�ͷ�
        //������ Ž���� �ش�������� �߻�ü�� �߻��Ͽ� ������ �����ϴ� ��ü

        //ȸ��
        [Space]
        [Header("ȸ�� ����")]
        public float rotateSpeed;
        public RotateType rotateType;

        //������Ʈ
        private GunRecoilAnim gunRecoilAnim;

        protected override void Awake()
        {
            base.Awake();
            gunRecoilAnim = GetComponent<GunRecoilAnim>();
        }

        void Update()
        {
            //�������ο� �ƹ��͵� ������ �Ѿ
            if (!SearchNearSide(this))
            {
                return;
            }

            //ȸ��
            Rotate(this, rotateType);

            //���� üũ
            if (SearchDirect(this, true))
            {
                Fire();
            }
        }

        protected override void Fire()
        {
            //�߻�
            int firePosCount = gunRecoilAnim.GetActionFireCount();
            if (firePosCount == -1)
            {
                //�������
                for (int j = 0; j < firePosArray.Length; j++)
                {
                    Projectile attackObject = fireObjectPool.RequestObjectPrefab();
                    Transform tempTr = firePosArray[j];
                    attackObject.GetTransform().SetPositionAndRotation(tempTr.position, tempTr.rotation);
                }
            }
            else
            {
                //�ܹ߻��
                Projectile attackObject = fireObjectPool.RequestObjectPrefab();
                Transform tempTr = firePosArray[firePosCount];
                attackObject.GetTransform().SetPositionAndRotation(tempTr.position, tempTr.rotation);
                attackObject.ActionAttackObject(unitObject);
            }
            gunRecoilAnim.ActionRecoil();//�ִ��۵�
        }

        protected override Transform GetMoveTr()
        {
            return null;
        }

        protected override float GetMoveSpeed()
        {
            return 0;
        }

        protected override Transform GetRotateTr()
        {
            return tr;
        }

        protected override float GetRotateSpeed()
        {
            return rotateSpeed;
        }
    }
}