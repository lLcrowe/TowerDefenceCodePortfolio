using System.Collections;
using UnityEngine;
using lLCroweTool;

namespace Assets.NodeMap
{
    public class MapMarkerLine : MonoBehaviour
    {
        //�ʸ�Ŀ������ Line�� ó���ϱ� ���� Ŭ����
        public MapMarker mapMarkerAPoint;
        public MapMarker mapMarkerBPoint;

        public LineRenderer lineRenderer;

        public LineType lineType;



        private void Awake()
        {
            lineRenderer = lLcroweUtil.GetAddComponent<LineRenderer>(gameObject);
        }

        /// <summary>
        /// ����� ���� �ʸ�Ŀ�� ������
        /// </summary>
        /// <param name="mapMarker">���� �ʸ�Ŀ</param>
        /// <returns>��������� �ʰ�������</returns>
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
            //� ���ϴ� ���� ó��
            Vector3[] linePosArray = null;






            return linePosArray;
        }








        public enum LineType
        {
            Straight,//����
            Curve//�
        }
    }
}