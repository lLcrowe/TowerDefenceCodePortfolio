using System.Collections;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(fileName = "New EnemyInfoData", menuName = "lLcroweTool/New EnemyInfo")]
    public class EnemyInfo : ScriptableObject
    {
        public int score;
        public int metal;
    }
}