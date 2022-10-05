using System.Collections.Generic;
using UnityEngine;
using lLCroweTool;

namespace Assets.TowerDefencePortfolio
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class SightTrigger : MonoBehaviour
    {
        //조절할 폴리곤2D
        //private PolygonCollider2D poly2D;

        public AxisDirectionType sightDirectionType;
        [Range(0f, 180f)]
        public float leftAngle;
        [Range(0f, 180f)]
        public float rightAngle;


        public float range;
        [Range(1, 5)]
        public int circleDotgeAmount = 1;


        public bool isUseNearRange;
        [Min(0)]
        public float nearRange;

        public List<Collider2D> detectColliderList = new List<Collider2D>();

        [Tag] public string tag;

        private void Awake()
        {   
            PolygonCollider2D poly2D = GetComponent<PolygonCollider2D>();
            poly2D.isTrigger = true;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tag))
            {
                if (!detectColliderList.Contains(collision))
                {
                    detectColliderList.Add(collision);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (detectColliderList.Contains(collision))
            {
                detectColliderList.Remove(collision);
            }
        }

        public bool GetFirstTarget(out Collider2D targetColliderObject)
        {
            if (detectColliderList.Count == 0)
            {
                targetColliderObject = null;
                return false;
            }

            targetColliderObject = detectColliderList[0];
            return true;
        }
    }
}