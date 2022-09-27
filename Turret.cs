using lLCroweTool;
using lLCroweTool.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class Turret : UnitObject_Base
    {
        //터렛
        //적군을 탐지후 해당방향으로 발사체를 발사하여 적군을 격파하는 객체

        //회전
        [Space]
        [Header("회전 설정")]
        public float rotateSpeed;
        public RotateType rotateType;

        //컴포넌트
        private GunRecoilAnim gunRecoilAnim;

        protected override void Awake()
        {
            base.Awake();
            gunRecoilAnim = GetComponent<GunRecoilAnim>();
        }

        void Update()
        {
            //원형내부에 아무것도 없으면 넘어감
            if (!SearchNearSide(this))
            {
                return;
            }

            //회전
            Rotate(this, rotateType);

            //전방 체크
            if (SearchDirect(this, true))
            {
                Fire();
            }
        }

        protected override void Fire()
        {
            //발사
            int firePosCount = gunRecoilAnim.GetActionFireCount();
            if (firePosCount == -1)
            {
                //일제사격
                for (int j = 0; j < firePosArray.Length; j++)
                {
                    Projectile attackObject = fireObjectPool.RequestObjectPrefab();
                    Transform tempTr = firePosArray[j];
                    attackObject.GetTransform().SetPositionAndRotation(tempTr.position, tempTr.rotation);
                }
            }
            else
            {
                //단발사격
                Projectile attackObject = fireObjectPool.RequestObjectPrefab();
                Transform tempTr = firePosArray[firePosCount];
                attackObject.GetTransform().SetPositionAndRotation(tempTr.position, tempTr.rotation);
                attackObject.ActionAttackObject(unitObject);
            }
            gunRecoilAnim.ActionRecoil();//애님작동
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