using lLCroweTool;
using lLCroweTool.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public abstract class UnitObject_Base : MonoBehaviour
    {
        //공격하는 오브젝트일시 상속받아서 처리하기
        
        //거리체크
        [Header("탐지관련")]
        public float distance;
        [Tag] public string tag;
        public ContactFilter2D filter2D;

        [Space]
        [Header("발사위치")]
        public Transform[] firePosArray = new Transform[0];//발사위치

        
        //발사체처리
        [System.Serializable]
        public class FireObjectPool : CustomObjectPool<Projectile> { }
        [Space]
        [Header("발사체 설정")]
        [SerializeField] protected FireObjectPool fireObjectPool = new FireObjectPool();

        
        //체크용//스태딕처리가능할듯함
        [SerializeField] [HideInInspector]private Transform target;//타겟팅        
        protected List<RaycastHit2D> hit2DList = new List<RaycastHit2D>();
        protected List<Collider2D> collider2DList = new List<Collider2D>();

        //컴포넌트
        protected Transform tr;//현재자기자신
        protected UnitObject unitObject;//공격하는 오브젝트

        protected virtual void Awake()
        {
            tr = transform;
            unitObject = GetComponent<UnitObject>();
        }

        protected static bool CheckSightTrigger()
        {
           


            return true;
        }

        /// <summary>
        /// 근처 원형으로 감지
        /// </summary>
        /// <param name="unitObject_Base">유닛오브젝트베이스</param>
        /// <returns>감지됫는지 여부</returns>
        protected static bool SearchNearSide(UnitObject_Base unitObject_Base)
        {
            if (Physics2D.OverlapCircle(unitObject_Base.tr.position, unitObject_Base.distance, unitObject_Base.filter2D, unitObject_Base.collider2DList) > 0)
            {
                int len = unitObject_Base.collider2DList.Count;
                for (int i = 0; i < len; i++)
                {
                    Collider2D collider = unitObject_Base.collider2DList[i];
                    //태그비교
                    if (collider.CompareTag(unitObject_Base.tag))
                    {
                        unitObject_Base.target = collider.transform;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 전방으로만 탐지후 태그체크
        /// </summary>
        /// <param name="unitObject_Base">유닛오브젝트베이스</param>
        /// <param name="isUseNearSide">근처유닛탐지여부</param>
        /// <returns>탐지된 여부</returns>
        protected static bool SearchDirect(UnitObject_Base unitObject_Base, bool isUseNearSide)
        {
            if (Physics2D.Raycast(unitObject_Base.tr.position, unitObject_Base.tr.up, unitObject_Base.filter2D, unitObject_Base.hit2DList, unitObject_Base.distance) > 0)
            {
                int len = unitObject_Base.hit2DList.Count;
                for (int i = 0; i < len; i++)
                {
                    Collider2D collider = unitObject_Base.hit2DList[i].collider;

                    if (isUseNearSide)
                    {
                        //존재하는지
                        if (!unitObject_Base.collider2DList.Contains(collider))
                        {
                            continue;
                        }
                    }

                    //태그비교
                    if (collider.CompareTag(unitObject_Base.tag))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 감지된 첫번째 타겟으로 회전
        /// </summary>
        /// <param name="unitObject_Base">유닛오브젝트베이스</param>
        protected static void Rotate(UnitObject_Base unitObject_Base, RotateType rotateType)
        {
            //있으면 회전
            if (ReferenceEquals(unitObject_Base.target, null))
            {
                return;
            }
            Rotate(unitObject_Base, unitObject_Base.target.position, rotateType);
        }

        /// <summary>
        /// 회전 함수
        /// </summary>
        /// <param name="unitObject_Base">유닛오브젝트베이스</param>
        /// <param name="targetPos">회전할 위치</param>
        protected static void Rotate(UnitObject_Base unitObject_Base, Vector2 targetPos, RotateType rotateType)
        {
            float time = unitObject_Base.GetRotateSpeed() * Time.deltaTime;
            Transform tempTr = unitObject_Base.GetRotateTr();
            switch (rotateType)
            {
                case RotateType.Slerp:
                    lLcroweUtil.RotateSlerp(tempTr, targetPos, time);
                    break;
                case RotateType.Lerp:
                    lLcroweUtil.RotateLerp(tempTr, targetPos, time);
                    break;
                case RotateType.Turret:
                    lLcroweUtil.RotateTurret(tempTr, targetPos, time);
                    break;
            }
        }

        /// <summary>
        /// 움직이는 함수
        /// </summary>
        /// <param name="unitObject_Base">유닛오브젝트베이스</param>
        /// <param name="targetPos">움직일 위치</param>
        protected static void Move(UnitObject_Base unitObject_Base, Vector2 targetPos)
        {
            unitObject_Base.GetMoveTr().Translate(targetPos * unitObject_Base.GetMoveSpeed() * Time.deltaTime);
        }

        /// <summary>
        /// 발사 함수
        /// </summary>
        protected abstract void Fire();

        /// <summary>
        /// 움직일 트랜스폼 가져오는 함수
        /// </summary>
        /// <returns>움직일 트랜스폼</returns>
        protected abstract Transform GetMoveTr();

        /// <summary>
        /// 움직이는 속도값 가져오는 함수
        /// </summary>
        /// <returns>움직이는 속도값</returns>
        protected abstract float GetMoveSpeed();

        /// <summary>
        /// 회전할 트랜스폼 가져오는 함수
        /// </summary>
        /// <returns>회전할 트랜스폼</returns>
        protected abstract Transform GetRotateTr();

        /// <summary>
        /// 회전값 가져오는 함수
        /// </summary>
        /// <returns>회전값</returns>
        protected abstract float GetRotateSpeed();

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, distance);
        }

        /// <summary>
        /// 회전타입
        /// </summary>
        public enum RotateType
        {
            Slerp,
            Lerp,
            Turret,
        }
    }
}