using lLCroweTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class Projectile : MonoBehaviour
    {
        public AttackInfo attackInfo = new AttackInfo();

        public int curPenetration;
        public Vector2 startPos;

        public float projectileSpeed;
        public ContactFilter2D filter2D;

        private List<RaycastHit2D> hit2DList = new List<RaycastHit2D>();
        private Transform tr;

        private UnitObject actionUnitObject;

        private void Awake()
        {
            tr = transform;
        }

        private void Update()
        {
            //�Ÿ�üũ
            if (lLcroweUtil.CheckDistance(startPos, tr.position, attackInfo.distance))
            {
                DeActive();//����
                return;
            }

            //�̵�
            //����ü������
            Vector3 dir = Vector3.up * projectileSpeed;
            tr.Translate(dir * Time.deltaTime);//���÷� ������

            //����
            if (Physics2D.Raycast(tr.position, tr.up, filter2D, hit2DList, 0.2f) == 0)
            {
                return;
            }

            for (int i = 0; i < hit2DList.Count; i++)
            {
                Collider2D collider = hit2DList[i].collider;
                if (collider.TryGetComponent(out UnitObject unitObject))
                {
                    //üũ
                    if (unitObject.teamType == actionUnitObject.teamType)
                    {
                        continue;
                    }

                    //��Ʈ
                    UnitObject.UnitStatusTakeDamaged(attackInfo.damage, unitObject, actionUnitObject);
                    //����Ʈ
                    DeActive();//����
                    break;
                }
            }
        }

        public void ActionAttackObject(UnitObject unitObject)
        {
            actionUnitObject = unitObject;
            gameObject.SetActive(true);
        }

        private void DeActive()
        {
            gameObject.SetActive(false);
            actionUnitObject = null;
        }

        public Transform GetTransform()
        {
            return tr;
        }

    }
}

