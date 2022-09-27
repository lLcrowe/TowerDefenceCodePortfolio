using System.Collections;
using UnityEngine;
using lLCroweTool.Dictionary;
using lLCroweTool.Singleton;
using lLCroweTool.ObjectPool;
using lLCroweTool;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Assets.NodeMap
{
    public class MapMarkerManager : MonoBehaviour
    {
        [System.Serializable]
        public class MapMarkerInfoBible : CustomDictionary<MapMarkerData, bool> { }
        public MapMarkerInfoBible mapMarkDataInfoBible = new MapMarkerInfoBible();

        //모든맵마커
        public List<MapMarker> mapMarkerList = new List<MapMarker>();

        //현재 있는 맵마커 주변에있는 맵마커들
        public List<MapMarker> nearSideMapMarkerList = new List<MapMarker>();

        [Header("맵마커 세팅")]
        public MapMarker targetingNextMarker;//타겟팅이 된 마커
        public MapMarker curMarker;//현재 있는 마커


        public MapMarker startMapMarker;//시작시 세팅해주는 마커
        public GameObject targetFlagObject;//노드맵상에서 움직이는 오브젝트

        [Header("높이레벨 세팅")]
        public bool isUseHeightLevel;//사용하면//현재높이//현재높이와 다음높이로 다음위치로 갈수 있는 여부
        public int currentHeightlevel;//밑으로는 안내려가게하는 기준점


        [Header("컬러지정")]
        public float curPosColorAlpha = 1f;//현재 위치 알파값
        public float selectColorAlpha = 0.8f;//선택 위치 알파값
        public float nearPosColorAlpha = 0.7f;//현재 위치의 근처 알파값
        public float OtherPosColorAlpha = 0.5f;//그외 위치의 알파값




        [Header("버튼설정")]
        public Button moveButton;

        [System.Serializable]
        public class MapMarkerLineObjectPool : CustomObjectPool<MapMarkerLine> { }
        [SerializeField] public MapMarkerLineObjectPool mapMarkerLineObjectPool = new MapMarkerLineObjectPool();

        //UI관련
        public TextMeshProUGUI textObject;


        private void Awake()
        {
            for (int i = 0; i < mapMarkerList.Count; i++)
            {
                MapMarker mapMarker = mapMarkerList[i];
                if (!ReferenceEquals(mapMarker.markerData, null))
                {
                    RegisterMapMarkerData(mapMarker);
                    mapMarker.SetColor(OtherPosColorAlpha);
                }
                mapMarker.SetMapMarkerManager(this);
            }

            //위치지정
            if (ReferenceEquals(startMapMarker, null))
            {
                if (mapMarkerList.Count != 0)
                {
                    targetFlagObject.transform.position = mapMarkerList[0].transform.position;
                }
            }
            else
            {
                targetFlagObject.transform.position = startMapMarker.transform.position;
            }

            //버튼설정
            if (!ReferenceEquals(moveButton, null))
            {
                moveButton.onClick.AddListener(delegate { MoveFlagObject(); });
            }
        }


        /// <summary>
        /// 맵마커를 등록하는 함수
        /// </summary>
        /// <param name="value">타겟이 될 맵마커</param>
        public void RegisterMapMarkerData(MapMarker value)
        {
            MapMarker marker = value;

            //등록했는지 여부 체크
            if (!mapMarkDataInfoBible.ContainsKey(marker.markerData))
            {
                //등록을 안했으면
                //신규등록

                bool isLock = marker.markerData.targetPrecedeMarkerData != null;//잠금여부
                mapMarkDataInfoBible.Add(marker.markerData, isLock);
            }
        }

        /// <summary>
        /// 맵마커락을 해금시키는 함수
        /// </summary>
        /// <param name="value">타겟이 될 맵마커</param>
        public void OnLockMapMarker(MapMarkerData value)
        {
            MapMarkerData markerData = value;
            if (mapMarkDataInfoBible.ContainsKey(markerData))
            {
                //잠금해체
                mapMarkDataInfoBible[markerData] = false;
            }
        }

        /// <summary>
        /// 현재 마커의 근처 맵들을 보여주는 함수
        /// </summary>
        public void SearchNearSideMapMarker()
        {
            //맵마커에 등록된 라인을 체크
            nearSideMapMarkerList.Clear();
            List<MapMarkerLine> tempList = curMarker.GetMapMarkerLineList();

            for (int i = 0; i < tempList.Count; i++)
            {
                MapMarker mapMarker = tempList[i].GetNextConnectMarker(curMarker);
                mapMarker.SetColor(nearPosColorAlpha);
                nearSideMapMarkerList.Add(mapMarker);
            }
        }

        /// <summary>
        /// 플래그오브젝트를 움직이게 하는 함수
        /// </summary>
        private IEnumerator MoveFlagObject()
        {
            //현재 마커에서 다음마커로
            targetFlagObject.transform.DOMove(targetingNextMarker.transform.position, 1f);
            yield return new WaitForSeconds(1f);
            curMarker = targetingNextMarker;
            targetingNextMarker = null;
            curMarker.MapMarkerAction();
            SearchNearSideMapMarker();
        }

        /// <summary>
        /// 맵마커를 선택했을시 호출하는 함수
        /// </summary>
        /// <param name="mouseDownMapMarker">클릭한 맵마커</param>
        public void ClickTargetMapMarker(MapMarker mouseDownMapMarker)
        {
            //버튼이 있을시 자기자신을 클릭하면 아무행동안함
            if (targetingNextMarker == mouseDownMapMarker)
            {
                if (ReferenceEquals(moveButton, null))
                {
                    //버튼이 없으면 움직임
                    MoveFlagObject();
                }
                else
                {
                    moveButton.interactable = false;
                }
                return;
            }

            targetingNextMarker = null;

            //이동가능한지 체크
            //맵마커에 등록된 라인을 체크
            List<MapMarkerLine> tempList = curMarker.GetMapMarkerLineList();
            for (int i = 0; i < tempList.Count; i++)
            {
                MapMarkerLine mapMarkerLine = tempList[i];
                MapMarker nextMapMarker = mapMarkerLine.GetNextConnectMarker(curMarker);//연결된 마커를 체크

                if (nextMapMarker == targetingNextMarker)
                {
                    //다음맵 마커가 맞으면
                    //세팅
                    targetingNextMarker = mouseDownMapMarker;
                    break;
                }
            }

            //타겟팅된 마커가 비어있는지
            if (ReferenceEquals(targetingNextMarker, null))
            {
                if (!ReferenceEquals(moveButton, null))
                {
                    moveButton.interactable = true;
                }
            }

            ShowMapMakerData(targetingNextMarker);
        }

        /// <summary>
        /// 마커에 대한 데이터를 텍스트에 보여주는 함수
        /// </summary>
        /// <param name="targetMapMarker"></param>
        public void ShowMapMakerData(MapMarker targetMapMarker)
        {
            if (ReferenceEquals(targetMapMarker, null))
            {
                textObject.text = LocalizingManager.Instance.GetLocalLizeText("Not Detect");
                Canvas.ForceUpdateCanvases();
                return;
            }


            //갈수 있는 방향인지 체크
            //선행 체크
            //선행 데이터체크
            if (!ReferenceEquals(targetingNextMarker.markerData.targetPrecedeMarkerData, null))
            {
                textObject.text = LocalizingManager.Instance.GetLocalLizeText("선행 데이터필요");
                Canvas.ForceUpdateCanvases();
                return;
            }

            //등록됫는지
            string content = null;
            if (mapMarkDataInfoBible.ContainsKey(targetingNextMarker.markerData))
            {
                //등록됫으면//잠금이 풀려있는지 체크
                if (mapMarkDataInfoBible[targetingNextMarker.markerData])
                {
                    content = LocalizingManager.Instance.GetLocalLizeText("명칭 : 알 수 없음.") +
                        "\n" + LocalizingManager.Instance.GetLocalLizeText("내용 : 알 수 없음.");
                }
                else
                {
                    content = LocalizingManager.Instance.GetLocalLizeText("명칭 : ") + targetingNextMarker.markerData.objectName + "\n" + LocalizingManager.Instance.GetLocalLizeText("내용 : ") + targetingNextMarker.markerData.objectShortDescription;
                }
                textObject.text = content;
                Canvas.ForceUpdateCanvases();
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}