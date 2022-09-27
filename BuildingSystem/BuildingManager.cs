using UnityEngine;
using lLCroweTool.Singleton;
using lLCroweTool.ObjectPool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System;
using System.Collections;
using lLCroweTool.DestroyManger;

namespace Assets
{
    public class BuildingManager : MonoBehaviourSingleton<BuildingManager>
    {
        public BuildingPlace targetBuildingPlace;
        public AssetReferenceAtlasedSprite dismantleBuildingImage;

        public Transform buttonPlacePos;

        [System.Serializable]
        public class BuildingButtonPool : CustomObjectPool<BuildingButton> { }
        public BuildingButtonPool buildingButtonPool = new BuildingButtonPool();

        public BuildingInfo[] buildingInfoArray;


        public IEnumerator ActionBuilding(BuildingInfo buildingInfo, BuildingButtonType buildingButtonType)
        {
            TowerDefenceManager manager = TowerDefenceManager.Instance;
            switch (buildingButtonType)
            {
                case BuildingButtonType.Build:
                    //제작
                    //체크
                    if (ReferenceEquals(targetBuildingPlace.targetTurret, null))
                    {
                        //못한다는 안내

                        yield break;
                    }

                    if (!manager.CheckBuildingResource(buildingInfo))
                    {
                        //못한다는 안내

                        yield break;
                    }

                    //작동
                    AsyncOperationHandle opHandle = buildingInfo.buildingPrefab.LoadAssetAsync<GameObject>();
                    yield return opHandle;

                    if (opHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        //제대로 됫으면 생성
                        Turret targetTurret = (Turret)Instantiate(buildingInfo.buildingPrefab.Asset, transform);

                        targetBuildingPlace.targetTurret = targetTurret;
                    }

                    break;
                case BuildingButtonType.BuildingDismantle:
                    //체크
                    if (!ReferenceEquals(targetBuildingPlace.targetTurret, null))
                    {
                        //못한다는 안내

                        yield break;
                    }

                    //작동
                    DestroyManager.Instance.AddDestoryGameObject(targetBuildingPlace.targetTurret.gameObject);

                    break;
            }
            yield return null;
            BuildingButtonAllDeActive();
        }

        public void ClickBuildingPlace(BuildingPlace buildingPlace)
        {
            targetBuildingPlace = buildingPlace;

            BuildingButton button;
            //UI오픈
            //건물중
            int len = buildingInfoArray.Length;
            for (int i = 0; i < len; i++)
            {
                BuildingInfo buildingInfo = buildingInfoArray[i];
                button = RequestObjectPrefab();
                button.SetBuildingButton(buildingInfo, BuildingButtonType.Build);
            }
          
            //철거
            button = RequestObjectPrefab();
            button.SetBuildingButton(null, BuildingButtonType.BuildingDismantle);
        }


        public void BuildingButtonAllDeActive()
        {
            buildingButtonPool.AllObjectDeActive();
        }

        private BuildingButton RequestObjectPrefab()
        {
            BuildingButton button = buildingButtonPool.RequestObjectPrefab();
            button.transform.SetParent(buttonPlacePos);
            return button;
        }
    }

    public enum BuildingButtonType
    {
        Build,
        BuildingDismantle,
    }
}








