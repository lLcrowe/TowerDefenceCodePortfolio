using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets
{
    [CreateAssetMenu(fileName = "New AttackInfoData", menuName = "lLcroweTool/New AttackInfo")]
    public class AttackInfo : ScriptableObject
    {
        public AssetReferenceSprite attackSprite;
        public int damage;
        public int penetration;//관통력
        public float distance;//거리
    }
}
