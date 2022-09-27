using System.Collections;
using UnityEngine;

namespace Assets
{
    public class BuildingPlace : MonoBehaviour
    {
        //건설하는 위치
        //트리거도 되나?=>됨
        public Turret targetTurret;//설치된 터렛오브젝트


        private void Awake()
        {
            Collider2D collider2D = GetComponent<Collider2D>();
            collider2D.isTrigger = true;
        }


        private void OnMouseDown()
        {
            Debug.Log("Click" + gameObject.name, gameObject);
            BuildingManager.Instance.ClickBuildingPlace(this);
        }

        /// <summary>
        /// 건설이 가능한지 체크
        /// </summary>
        /// <returns>가능한지 여부</returns>
        public bool CheckIsBuild()
        {
            return ReferenceEquals(targetTurret, null);
        }
    }
}