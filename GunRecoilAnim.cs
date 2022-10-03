using UnityEngine;
using DG.Tweening;
using lLCroweTool;
using MEC;
using System.Collections.Generic;

namespace Assets
{
    public class GunRecoilAnim : MonoBehaviour
    {
        //건의 주퇴복좌기를 구현하기 위한 클래스
        //현재 오브젝트를 쏘고난뒤 돌아오는 애니메이션


        [Header("포 발사시 뒤로 가는 설정")]
        [Tooltip("뒤로 후퇴하는거리")]
        public float backDistnace = 0;//뒤로 후퇴하는거리
        [Tooltip("뒤로 후퇴하는 시간")]
        public float backIntervalTime = 0;//뒤로 후퇴하는 시간


        [Header("포 발사후 원래되로 가는 설정")]
        [Tooltip("원래 위치")]
        public float originDistance = 1;//원래위치
        [Tooltip("되돌아오는 시간")]
        public float resetIntervalTime = 0.3f;//되돌아오는 시간
        [Tooltip("되돌아오는 시간동안 코루틴이 대기하는 여부")]
        public bool isWaitResetTime = false;//되돌아오는 시간동안 코루틴이 대기하는 여부


        [Header("동시발사 설정")]
        [Tooltip("동시발사 여부")]
        public bool isSolvoShot = false;
        [SerializeField] [HideInInspector] private int firePosCount = 0;

        [Header("포 발사 위치설정")]
        [Tooltip("포 위치는 현스크립트로 설정됩니다.")]
        public Transform[] gunTransformArray = new Transform[0];
        private CoroutineHandle handle;


        /// <summary>
        /// 리코일 액션
        /// </summary>
        public void ActionRecoil()
        {
            if (handle.IsRunning)
            {
                return;
            }
            handle = Timing.RunCoroutine(ActionAnim(this));
        }

        public int GetActionFireCount()
        {
            //-1은 일제사격
            if (isSolvoShot)
            {
                return -1;
            }
            return firePosCount;
        }

        public Transform[] GetFirePosArray()
        {
            return gunTransformArray;
        }

        /// <summary>
        /// 건리코일애니메이션처리를 위한 코루틴
        /// </summary>
        /// <param name="gunRecoilAnim">건리코일애님모듈</param>
        private static IEnumerator<float> ActionAnim(GunRecoilAnim gunRecoilAnim)
        {
            float backIntervalTime = gunRecoilAnim.backIntervalTime;
            float backDistnace = gunRecoilAnim.backDistnace;
            float originDistance = gunRecoilAnim.originDistance;
            float resetIntervalTime = gunRecoilAnim.resetIntervalTime;
            int i;

            if (gunRecoilAnim.isSolvoShot)
            {
                for (i = 0; i < gunRecoilAnim.gunTransformArray.Length; i++)
                {
                    Transform tr = gunRecoilAnim.gunTransformArray[i];
                    tr.DOLocalMoveY(-backDistnace, backIntervalTime);                   
                }


                yield return Timing.WaitForSeconds(backIntervalTime);
                for (i = 0; i < gunRecoilAnim.gunTransformArray.Length; i++)
                {
                    Transform tr = gunRecoilAnim.gunTransformArray[i];
                    tr.DOLocalMoveY(originDistance, resetIntervalTime);
                }
            }
            else
            {
                Transform tr = gunRecoilAnim.gunTransformArray[gunRecoilAnim.firePosCount];
                tr.DOLocalMoveY(-backDistnace, backIntervalTime);
                yield return Timing.WaitForSeconds(backIntervalTime);
                tr.DOLocalMoveY(originDistance, resetIntervalTime);

                gunRecoilAnim.firePosCount++;
                if (gunRecoilAnim.firePosCount >= gunRecoilAnim.gunTransformArray.Length)
                {
                    gunRecoilAnim.firePosCount = 0; 
                }
            }

            if (!gunRecoilAnim.isWaitResetTime)
            {
                yield return Timing.WaitForSeconds(resetIntervalTime);
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector2 startPos = transform.position + transform.up * originDistance;
            Gizmos.DrawWireSphere(startPos, 0.2f);
            Vector2 endPos = transform.position + transform.transform.up * -backDistnace;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endPos, 0.2f);
        }
    }
}