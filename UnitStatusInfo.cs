using System.Collections;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(fileName = "New UnitStatusInfoData", menuName = "lLcroweTool/New UnitStatusInfo")]
    public class UnitStatusInfo : ScriptableObject
    {
        //유닛스탯용 클래스
        public int healthMaxPoint;
        public int healthCurPoint;

        public int healthRatePoint;
        public float healthRateTime;
        public float rateTime;//캐싱용

        public int armorPower;
    }
}