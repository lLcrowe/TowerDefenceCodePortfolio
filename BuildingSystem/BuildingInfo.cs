using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets
{
    [CreateAssetMenu(fileName = "New BuildingData", menuName = "lLcroweTool/New BuildingInfo")]
    public class BuildingInfo : ScriptableObject
    {   
        public AssetReferenceGameObject buildingPrefab;//빌딩프리팹
        public AssetReferenceSprite bluePrintBuildingImage;//미리보기 건물이미지

        public string buildingDescription;

        //필요자원
        public int score;
        public int metal;
        public int eletric;
    }
}