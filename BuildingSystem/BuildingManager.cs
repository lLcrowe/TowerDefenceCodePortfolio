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
                    //����
                    //üũ
                    if (ReferenceEquals(targetBuildingPlace.targetTurret, null))
                    {
                        //���Ѵٴ� �ȳ�

                        yield break;
                    }

                    if (!manager.CheckBuildingResource(buildingInfo))
                    {
                        //���Ѵٴ� �ȳ�

                        yield break;
                    }

                    //�۵�
                    AsyncOperationHandle opHandle = buildingInfo.buildingPrefab.LoadAssetAsync<GameObject>();
                    yield return opHandle;

                    if (opHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        //����� ������ ����
                        Turret targetTurret = (Turret)Instantiate(buildingInfo.buildingPrefab.Asset, transform);

                        targetBuildingPlace.targetTurret = targetTurret;
                    }

                    break;
                case BuildingButtonType.BuildingDismantle:
                    //üũ
                    if (!ReferenceEquals(targetBuildingPlace.targetTurret, null))
                    {
                        //���Ѵٴ� �ȳ�

                        yield break;
                    }

                    //�۵�
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
            //UI����
            //�ǹ���
            int len = buildingInfoArray.Length;
            for (int i = 0; i < len; i++)
            {
                BuildingInfo buildingInfo = buildingInfoArray[i];
                button = RequestObjectPrefab();
                button.SetBuildingButton(buildingInfo, BuildingButtonType.Build);
            }
          
            //ö��
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








