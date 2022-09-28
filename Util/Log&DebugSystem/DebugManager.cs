using UnityEngine;
using lLCroweTool.Singleton;

namespace lLCroweTool.DebugLog
{
    public class DebugManager : MonoBehaviourSingleton<DebugManager>
    {
        //중앙방식인 디버그매니저
        //필요시에 디버그를 찍게해주는 매니저이다
        //외부에서 사용하는 함수객체 있음

        //디버그 했을시만 볼수있게 하는 용도
        private bool useDebugSystem = false;
        private bool useLog = false;

        /// <summary>
        /// 디버그사용 함수
        /// </summary>
        /// <param name="logContent">로그용 내용</param>
        /// <param name="target">타겟되는 오브젝트</param>
        /// <param name="debugType">디버그타입</param>
        public void Log(string logContent, Object targetObject, DebugType debugType)
        {
            if (!useDebugSystem)
            {
                return;
            }

            switch (debugType)
            {
                case DebugType.Info:
                    Debug.Log(logContent, targetObject);
                    break;
                case DebugType.Waring:
                    Debug.LogWarning(logContent, targetObject);
                    break;
                case DebugType.Error:
                    Debug.LogError(logContent, targetObject);
                    break;
                case DebugType.Break:
                    Debug.Break();
                    break;
            }
        }

        public void SetUseDebugSystem(bool isUse)
        {
            useDebugSystem = isUse;
        }

        public void SetUseLog(bool isUse)
        {
            useLog = isUse;
        }

        public enum DebugType
        {
            //알림용
            Info,
            Waring,
            Error,

            //디버그용
            Break,//에디터정지
        }


    }

}
