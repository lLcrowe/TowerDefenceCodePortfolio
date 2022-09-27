using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.NodeMap
{
    public class MapMarker : MonoBehaviour
    {
        private MapMarkerManager parentMapMarkerManager;

        public bool isUseEnterToolTip = false;//툴팁사용여부

        public int heightLevel;//높이레벨

        public MapMarkerData markerData;
        public List<MapMarkerLine> connectMapMarkerLineList = new List<MapMarkerLine>();//연결된 라인들//에디터에서 처리예정


        public SpriteRenderer sr;//마커표시를 위한 스프라이트
        public BoxCollider2D box2d;//콜라이더용

        /// <summary>
        /// 부모가 될 맵마커매니저를 세팅하는 함수
        /// </summary>
        /// <param name="value">맵마커매니저</param>
        public void SetMapMarkerManager(MapMarkerManager value)
        {
            parentMapMarkerManager = value;
        }
       
        public void SetColor(float a)
        {
            Color color = sr.color;
            color.a = a;
            sr.color = color;
        }

        public void SetColor(float r, float g, float b)
        {
            Color color = sr.color;
            color.r = r;
            color.g = g;
            color.b = b;
            sr.color = color;
        }

        public void SetColor(Color color)
        {
            sr.color = color;
        }

        public List<MapMarkerLine> GetMapMarkerLineList()
        {
            return connectMapMarkerLineList;
        }


        //마우스버튼을 클릭했을시
        private void OnMouseDown()
        {
            MouseButtonAction(MouseButtonType.Down, this);
        }

        //마우스버튼을 클릭한상태로 움직였을시
        //private void OnMouseDrag()
        //{
        //    Debug.Log(gameObject.name + "OnMouseDrag이벤트");
        //}

        //마우스가 콜라이더안쪽으로 들어왔을시
        private void OnMouseEnter()
        {
            MouseButtonAction(MouseButtonType.Enter, this);
        }


        //마우스가 콜라이더안쪽으로 나갔을시
        private void OnMouseExit()
        {
            MouseButtonAction(MouseButtonType.Exit, this);
        }

        /// <summary>
        /// 맵마커의 스테이지 진행을 위한 함수
        /// </summary>
        public virtual void MapMarkerAction()
        {
            Debug.Log("MapMarker Stage Action");
        }

        private static void MouseButtonAction(MouseButtonType mouseButtonType, MapMarker mapMarker)
        {
            switch (mouseButtonType)
            {
                case MouseButtonType.Enter:
                    if (!mapMarker.isUseEnterToolTip)
                    {
                        return;
                    }

                    mapMarker.parentMapMarkerManager.ShowMapMakerData(mapMarker);
                    break;
                case MouseButtonType.Exit:
                    if (!mapMarker.isUseEnterToolTip)
                    {
                        return;
                    }

                    mapMarker.parentMapMarkerManager.ShowMapMakerData(null);
                    break;
                case MouseButtonType.Down:
                    mapMarker.parentMapMarkerManager.ClickTargetMapMarker(mapMarker);
                    break;
            }
        }


        public enum MouseButtonType
        {
            Enter,
            Exit,
            Down,
        }
    }
}