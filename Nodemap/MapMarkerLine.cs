using System.Collections;
using UnityEngine;
using lLCroweTool;

namespace Assets.NodeMap
{
    public class MapMarkerLine : MonoBehaviour
    {
        //맵마커사이의 Line을 처리하기 위한 클래스
        public MapMarker mapMarkerAPoint;
        public MapMarker mapMarkerBPoint;

        public LineRenderer lineRenderer;

        public LineType lineType;



        private void Awake()
        {
            lineRenderer = lLcroweUtil.GetAddComponent<LineRenderer>(gameObject);
        }

        /// <summary>
        /// 연결된 다음 맵마커를 가져옴
        /// </summary>
        /// <param name="mapMarker">기존 맵마커</param>
        /// <returns>다음연결된 맵가져오기</returns>
        public MapMarker GetNextConnectMarker(MapMarker mapMarker)
        {
            return mapMarker == mapMarkerAPoint ? mapMarkerBPoint : mapMarkerAPoint;
        }


        public void SetPos(Vector2 start, Vector2 end)
        {
            Vector3[] linePosArray;
            switch (lineType)
            {
                case LineType.Straight:
                    linePosArray = new Vector3[2];
                    linePosArray[0] = start;
                    linePosArray[1] = end;
                    lineRenderer.SetPositions(linePosArray);
                    break;
                case LineType.Curve:
                    linePosArray = new Vector3[2];
                    linePosArray[0] = start;
                    linePosArray[1] = end;
                    lineRenderer.SetPositions(linePosArray);
                    break;
            }
        }

        private static Vector3[] GetCurveLinePos(Vector2 start, Vector2 end)
        {
            //곡선 구하는 공식 처리
            Vector3[] linePosArray = null;






            return linePosArray;
        }








        public enum LineType
        {
            Straight,//직선
            Curve//곡선
        }
    }
}