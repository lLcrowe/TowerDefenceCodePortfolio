using System.Collections;
using UnityEngine;
using lLCroweTool;

namespace Assets.TowerDefencePortfolio
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class SightTrigger : MonoBehaviour
    {
        //조절할 폴리곤2D
        //private PolygonCollider2D poly2D;

        public SightDirectionType sightDirectionType;
        [Range(0f, 180f)]
        public float leftAngle;
        [Range(0f, 180f)]
        public float rightAngle;


        public float range;
        [Range(1, 5)]
        public int circleDotgeAmount;


        public bool isUseNearRange;
        [Min(0)]
        public float nearRange;

        public class DetectCollider2DEventModule : QueueEventModule<Collider2D> {}


        [Tag] public string tag;

        private DetectCollider2DEventModule detectCollider2DEventModule = new DetectCollider2DEventModule();

        private void Awake()
        {   
            PolygonCollider2D poly2D = GetComponent<PolygonCollider2D>();
            poly2D.isTrigger = true;
            detectCollider2DEventModule.SetTimer(0.06f);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {

        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            
        }

        private DetectCollider2DEventModule GetDetectCollider2DEventModule()
        {
            return detectCollider2DEventModule;
        }
    }

    public enum SightDirectionType
    {
        X,//X 방향
        Y,//Y 방향
    }
}