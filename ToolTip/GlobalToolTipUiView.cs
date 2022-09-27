using lLCroweTool.DebugLog;

namespace lLCroweTool.ToolTipSystem
{
    public class GlobalToolTipUiView : ToolTipUiView
    {
        private static GlobalToolTipUiView instance;
        public static GlobalToolTipUiView Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    instance = FindObjectOfType<GlobalToolTipUiView>();
                    //if (ReferenceEquals(instance, null))
                    if (ReferenceEquals(instance, null))
                    {
                        DebugManager.Instance.Log("해당되는 오브젝트가 없습니다. 게임실행시 처음부터 초기화해주세요", null, DebugManager.DebugType.Waring);

                        // ReSharper disable once ArrangeStaticMemberQualifier
                        //_instance = (MasterAudio)GameObject.FindObjectOfType(typeof(MasterAudio));
                        //return _instance;
                    }
                }
                return instance;
            }
        }

        protected override void Awake()
        {   
            instance = this;
            base.Awake();
        }
    }
}