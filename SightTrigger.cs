using System.Collections;
using UnityEngine;

namespace Assets.TowerDefencePortfolio
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class SightTrigger : MonoBehaviour
    {
        //조절할 폴리곤2D
        //private PolygonCollider2D poly2D;

        public SightDirectionType sightDirectionType;
        [Range(0f,180f)]
        public float leftAngle;
        [Range(0f, 180f)]
        public float rightAngle;

        
        public float range;
        [Range(1, 6)]
        public int circleDotgeAmount;


        public bool isUseNearRange;
        [Min(0)]
        public float nearRange;

        private void Awake()
        {
            //poly2D = GetComponent<PolygonCollider2D>();
            //초기세팅용이라 인게임에선 가지고 있을필요는 없음
            //if (Application.isPlaying)
            //{
            //    Destroy(this);
            //}
        }
    }

    public enum SightDirectionType
    {
        X,//X 방향
        Y,//Y 방향
    }
}