using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using lLCroweTool;
using lLCroweTool.ToolTipSystem;

namespace Assets
{
    public class BuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private BuildingInfo targetBuildData;
        [SerializeField] private BuildingButtonType buildingButtonType;
        [Header("버튼에서 텍스트는 필요없음")]
        [SerializeField] private Button button;

        private void Awake()
        {
            button = lLcroweUtil.GetAddComponent<Button>(gameObject);
        }

        /// <summary>
        /// 건물생성버튼 세팅하는 함수
        /// </summary>
        /// <param name="buildingData">세팅할 건물데이터</param>
        /// <param name="buildModeType">건축모드 타입</param>
        public IEnumerator SetBuildingButton(BuildingInfo buildingData, BuildingButtonType buildModeType)
        {  
            targetBuildData = buildingData;
            buildingButtonType = buildModeType;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate 
            {
                BuildingManager.Instance.ActionBuilding(targetBuildData, buildModeType);
            });

            AsyncOperationHandle opHandle;
            switch (buildingButtonType)
            {
                case BuildingButtonType.Build:
                    //건설
                    opHandle = buildingData.bluePrintBuildingImage.LoadAssetAsync<Sprite>();
                    yield return opHandle;
                    button.image.sprite = opHandle.Result as Sprite;
                    break;
                case BuildingButtonType.BuildingDismantle:
                    //해제
                    opHandle = BuildingManager.Instance.dismantleBuildingImage.LoadAssetAsync<Sprite>();
                    yield return opHandle;
                    button.image.sprite = opHandle.Result as Sprite;
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            string tempTitle = "";
            string tempContent = "";
            switch (buildingButtonType)
            {
                case BuildingButtonType.Build:
                    if (targetBuildData == null)
                    {
                        return;
                    }
                    tempTitle = LocalizingManager.Instance.GetLocalLizeText(targetBuildData.name);
                    tempContent = LocalizingManager.Instance.GetLocalLizeText(targetBuildData.buildingDescription);
                    break;
                case BuildingButtonType.BuildingDismantle:
                    //툴팁
                    tempTitle = LocalizingManager.Instance.GetLocalLizeText("건물철거");
                    tempContent = LocalizingManager.Instance.GetLocalLizeText("건물을 철거합니다.");
                    break;
            }
            GlobalToolTipUiView.Instance.ShowText(tempTitle, tempContent);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GlobalToolTipUiView.Instance.ClearText();
            GlobalToolTipUiView.Instance.OffText();
        }

    }

}
