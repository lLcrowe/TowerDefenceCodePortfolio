using System.Collections;
using UnityEngine;

namespace Assets
{
    public class Enemy : UnitObject_Base
    {
        //움직이다가 적군이 탐색되면 공격하는 객체

        [Space]
        [Header("움직임 설정")]
        public MoveDirectionType moveDirectionType;
        public float moveSpeed;
        private int firePosCount;

        public EnemyInfo enemyInfo;

        private void Update()
        {
            //전방 체크
            if (SearchDirect(this, false))
            {
                Fire();
            }
            else
            {
                Move(this, GetDirection(moveDirectionType));
            }
        }

        protected override void Fire()
        {   
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

                firePosCount++;
                if (firePosCount >= firePosArray.Length)
                {
                    firePosCount = 0;
                }
            }
        }

        public Vector2 GetDirection(MoveDirectionType moveDirectionType)
        {
            Vector3 newPos = Vector2.zero;
            switch (moveDirectionType)
            {
                case MoveDirectionType.Right:
                    newPos = tr.right;
                    break;
                case MoveDirectionType.Left:
                    newPos = -tr.right;
                    break;
                case MoveDirectionType.Up:
                    newPos = tr.up;
                    break;
                case MoveDirectionType.Down:
                    newPos = -tr.up;
                    break;
            }
            return newPos;
        }

        protected override Transform GetMoveTr()
        {
            return tr;
        }

        protected override float GetMoveSpeed()
        {
            return moveSpeed;
        }

        protected override Transform GetRotateTr()
        {
            return null;
        }

        protected override float GetRotateSpeed()
        {
            return 0;
        }
    }

    public enum MoveDirectionType
    {
        Right,
        Left,
        Up,
        Down,
    }
}
