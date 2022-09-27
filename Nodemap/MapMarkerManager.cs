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

        //���ʸ�Ŀ
        public List<MapMarker> mapMarkerList = new List<MapMarker>();

        //���� �ִ� �ʸ�Ŀ �ֺ����ִ� �ʸ�Ŀ��
        public List<MapMarker> nearSideMapMarkerList = new List<MapMarker>();

        [Header("�ʸ�Ŀ ����")]
        public MapMarker targetingNextMarker;//Ÿ������ �� ��Ŀ
        public MapMarker curMarker;//���� �ִ� ��Ŀ


        public MapMarker startMapMarker;//���۽� �������ִ� ��Ŀ
        public GameObject targetFlagObject;//���ʻ󿡼� �����̴� ������Ʈ

        [Header("���̷��� ����")]
        public bool isUseHeightLevel;//����ϸ�//�������//������̿� �������̷� ������ġ�� ���� �ִ� ����
        public int currentHeightlevel;//�����δ� �ȳ��������ϴ� ������


        [Header("�÷�����")]
        public float curPosColorAlpha = 1f;//���� ��ġ ���İ�
        public float selectColorAlpha = 0.8f;//���� ��ġ ���İ�
        public float nearPosColorAlpha = 0.7f;//���� ��ġ�� ��ó ���İ�
        public float OtherPosColorAlpha = 0.5f;//�׿� ��ġ�� ���İ�




        [Header("��ư����")]
        public Button moveButton;

        [System.Serializable]
        public class MapMarkerLineObjectPool : CustomObjectPool<MapMarkerLine> { }
        [SerializeField] public MapMarkerLineObjectPool mapMarkerLineObjectPool = new MapMarkerLineObjectPool();

        //UI����
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

            //��ġ����
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

            //��ư����
            if (!ReferenceEquals(moveButton, null))
            {
                moveButton.onClick.AddListener(delegate { MoveFlagObject(); });
            }
        }


        /// <summary>
        /// �ʸ�Ŀ�� ����ϴ� �Լ�
        /// </summary>
        /// <param name="value">Ÿ���� �� �ʸ�Ŀ</param>
        public void RegisterMapMarkerData(MapMarker value)
        {
            MapMarker marker = value;

            //����ߴ��� ���� üũ
            if (!mapMarkDataInfoBible.ContainsKey(marker.markerData))
            {
                //����� ��������
                //�űԵ��

                bool isLock = marker.markerData.targetPrecedeMarkerData != null;//��ݿ���
                mapMarkDataInfoBible.Add(marker.markerData, isLock);
            }
        }

        /// <summary>
        /// �ʸ�Ŀ���� �رݽ�Ű�� �Լ�
        /// </summary>
        /// <param name="value">Ÿ���� �� �ʸ�Ŀ</param>
        public void OnLockMapMarker(MapMarkerData value)
        {
            MapMarkerData markerData = value;
            if (mapMarkDataInfoBible.ContainsKey(markerData))
            {
                //�����ü
                mapMarkDataInfoBible[markerData] = false;
            }
        }

        /// <summary>
        /// ���� ��Ŀ�� ��ó �ʵ��� �����ִ� �Լ�
        /// </summary>
        public void SearchNearSideMapMarker()
        {
            //�ʸ�Ŀ�� ��ϵ� ������ üũ
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
        /// �÷��׿�����Ʈ�� �����̰� �ϴ� �Լ�
        /// </summary>
        private IEnumerator MoveFlagObject()
        {
            //���� ��Ŀ���� ������Ŀ��
            targetFlagObject.transform.DOMove(targetingNextMarker.transform.position, 1f);
            yield return new WaitForSeconds(1f);
            curMarker = targetingNextMarker;
            targetingNextMarker = null;
            curMarker.MapMarkerAction();
            SearchNearSideMapMarker();
        }

        /// <summary>
        /// �ʸ�Ŀ�� ���������� ȣ���ϴ� �Լ�
        /// </summary>
        /// <param name="mouseDownMapMarker">Ŭ���� �ʸ�Ŀ</param>
        public void ClickTargetMapMarker(MapMarker mouseDownMapMarker)
        {
            //��ư�� ������ �ڱ��ڽ��� Ŭ���ϸ� �ƹ��ൿ����
            if (targetingNextMarker == mouseDownMapMarker)
            {
                if (ReferenceEquals(moveButton, null))
                {
                    //��ư�� ������ ������
                    MoveFlagObject();
                }
                else
                {
                    moveButton.interactable = false;
                }
                return;
            }

            targetingNextMarker = null;

            //�̵��������� üũ
            //�ʸ�Ŀ�� ��ϵ� ������ üũ
            List<MapMarkerLine> tempList = curMarker.GetMapMarkerLineList();
            for (int i = 0; i < tempList.Count; i++)
            {
                MapMarkerLine mapMarkerLine = tempList[i];
                MapMarker nextMapMarker = mapMarkerLine.GetNextConnectMarker(curMarker);//����� ��Ŀ�� üũ

                if (nextMapMarker == targetingNextMarker)
                {
                    //������ ��Ŀ�� ������
                    //����
                    targetingNextMarker = mouseDownMapMarker;
                    break;
                }
            }

            //Ÿ���õ� ��Ŀ�� ����ִ���
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
        /// ��Ŀ�� ���� �����͸� �ؽ�Ʈ�� �����ִ� �Լ�
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


            //���� �ִ� �������� üũ
            //���� üũ
            //���� ������üũ
            if (!ReferenceEquals(targetingNextMarker.markerData.targetPrecedeMarkerData, null))
            {
                textObject.text = LocalizingManager.Instance.GetLocalLizeText("���� �������ʿ�");
                Canvas.ForceUpdateCanvases();
                return;
            }

            //��ϵ̴���
            string content = null;
            if (mapMarkDataInfoBible.ContainsKey(targetingNextMarker.markerData))
            {
                //��ϵ�����//����� Ǯ���ִ��� üũ
                if (mapMarkDataInfoBible[targetingNextMarker.markerData])
                {
                    content = LocalizingManager.Instance.GetLocalLizeText("��Ī : �� �� ����.") +
                        "\n" + LocalizingManager.Instance.GetLocalLizeText("���� : �� �� ����.");
                }
                else
                {
                    content = LocalizingManager.Instance.GetLocalLizeText("��Ī : ") + targetingNextMarker.markerData.objectName + "\n" + LocalizingManager.Instance.GetLocalLizeText("���� : ") + targetingNextMarker.markerData.objectShortDescription;
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