using UnityEngine;
using UnityEngine.Events;

namespace lLCroweTool.TimerSystem
{
    /// <summary>
    /// 업데이트타이머 모듈
    /// </summary>
    public class TimerModule : MonoBehaviour
    {
        [Header("-=1. 몇초마다 이벤트를 발생할것인가.")]
        //몇초에 리셋될건지 해주는 타이머
        [SerializeField] protected float timer = 0;//작동될 타이머 : 0.02~ 0.05 정도//0.02인 이유는 눈으로봐도 그렇게 차이가 안나기 때문

        //이벤트호출용//유니티이벤트를 사용하는 타임모듈 베이스 클래스
        //원하는 시간이 될때마다 이벤트호출용도
        //모듈로서 사용할려면 사용할려는 오브젝트에서
        //따로 호출할려는 이벤트들을 생성해야 하며
        //해당객체도 시간이 필요하면 카운트함수를 제작해야함
        [Header("-=2. 사용할 이벤트를 설정")]
        public UnityEvent unityEvent;//호출할시 GC가 쌓임//단 매프레임마다 쌓이진 않고 일정시간마다 쌓임


        [Header("-=3. 독립적인 타이머인가?")]
        //월드타이머와 별개로 돌아간건지
        public bool indieTimer = false;

        //기존의 돌아가는 타이머
        //[SerializeField]
        private float time = -1;
        private static float timerValue;

        protected virtual void Awake()
        {
            if (unityEvent == null)
            {
                unityEvent = new UnityEvent();
            }
        }

        protected void OnEnable()
        {
            if (time == -1)
            {
                ResetTime();
                //time = Time.time;
            }
        }

        private void Update()
        {
            UpdateTimerModule(this);
        }

        /// <summary>
        /// 업데이트 타이머모듈이 사용하는 업데이트
        /// </summary>
        /// <param name="updateTimerModule">타겟이 될 업데이트타이머모듈</param>
        private static void UpdateTimerModule(TimerModule updateTimerModule)
        {
            //월드타이머존재여부 또는 별개의 타이머인지
            timerValue = updateTimerModule.indieTimer ? updateTimerModule.GetTimer() + updateTimerModule.GetTime() : (updateTimerModule.GetTimer() * Time.timeScale) + updateTimerModule.GetTime();

            if (Time.time > timerValue)
            {
                //Debug.Log("time : "+ time);

                //이벤트작동
                //Profiler.BeginSample("TestUpdate");
                updateTimerModule.unityEvent.Invoke();//32B
                //Profiler.EndSample();

                //Debug.Log("시간이 초기화되었습니다");
                updateTimerModule.ResetTime();
            }
        }

        /// <summary>
        /// 작동될 이벤트 추가(세팅)
        /// </summary>
        /// <param name="action">함수</param>
        public void AddUnityEvent(UnityAction action)
        {
            //AddListener(delegate{함수();})
            unityEvent.AddListener(action);
        }

        /// <summary>
        /// 모든이벤트삭제
        /// </summary>
        public void RemoveAllUnityEvent()
        {
            unityEvent.RemoveAllListeners();
        }

        /// <summary>
        /// 원하는 이벤트만 삭제
        /// </summary>
        /// <param name="action">함수</param>
        public void RemoveUnityEvent(UnityAction action)
        {
            unityEvent.RemoveListener(action);
        }


        /// <summary>
        /// 타이머 세팅(시간초)
        /// </summary>
        /// <param name="value">시간</param>
        public void SetTimer(float value)
        {
            timer = value;
        }

        /// <summary>
        /// 세팅한 타이머를 가져오는 함수
        /// </summary>
        /// <returns>세팅된 타이머</returns>
        public float GetTimer()
        {
            return timer;
        }

        /// <summary>
        /// 캐싱한 타임을 가져오는 함수
        /// </summary>
        /// <returns>지정한 타임</returns>
        public float GetTime()
        {
            return time;
        }

        /// <summary>
        /// 시간을 현재 시간으로 초기화
        /// </summary>
        public void ResetTime()
        {
            time = Time.time;
        }

        protected virtual void OnDestroy()
        {
            unityEvent = null;
        }
    }
}
