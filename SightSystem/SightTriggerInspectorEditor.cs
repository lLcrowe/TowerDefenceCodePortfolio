using System.Collections;
using UnityEngine;
using UnityEditor;
using Assets.TowerDefencePortfolio;
using lLCroweTool;

namespace Assets.EditorOnly
{
    [CustomEditor(typeof(SightTrigger))]
    public class SightTriggerInspectorEditor : Editor
    {

        private SightTrigger sightTrigger;

        private void OnEnable()
        {
            sightTrigger = (SightTrigger)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!sightTrigger.TryGetComponent(out PolygonCollider2D poly2D))
            {
                EditorGUILayout.HelpBox("PolygonCollider2D 컴포넌트가 필요합니다.", MessageType.Info);
                return;
            }

            //로컬위치로 위치가 지정됨
            float range = sightTrigger.range;
            Vector3 worldPos = Vector2.zero;
            Vector3 rightPos = lLcroweUtil.AngleToDirection(-sightTrigger.rightAngle, sightTrigger.sightDirectionType);
            Vector3 leftPos = lLcroweUtil.AngleToDirection(sightTrigger.leftAngle, sightTrigger.sightDirectionType);
            int startPos = 0;//반복문 시작위치
            Vector2[] circlePos;
            int len;

            //원형을 그리자
            //각도체크후 일정각도마다 좌표찍기
            //찍고 싶은 점 숫자 구하기
            //해당 점 숫자 만큼 각도를 줘서 점을 찍어주기
            float totalAngle = Mathf.Abs(sightTrigger.leftAngle) + Mathf.Abs(sightTrigger.rightAngle);
            float addedAngle = totalAngle / sightTrigger.circleDotgeAmount;
            float temp = sightTrigger.leftAngle;//초기값

            //시작2 포인트1//끝포인트1//중간포인트circleDotgeAmount
            if (sightTrigger.isUseNearRange)
            {
                //최대거리 체크
                sightTrigger.nearRange = sightTrigger.nearRange > sightTrigger.range ? sightTrigger.range : sightTrigger.nearRange;

                //설정
                len = sightTrigger.circleDotgeAmount + 3;
                circlePos = new Vector2[len];
                circlePos[0] = rightPos * sightTrigger.nearRange;
                startPos++;
                circlePos[1] = leftPos * sightTrigger.nearRange;
                startPos++;
                circlePos[2] = leftPos * range;
                startPos++;
            }
            else
            {   
                //설정
                len = sightTrigger.circleDotgeAmount + 2;
                circlePos = new Vector2[len];
                circlePos[0] = worldPos;
                startPos++;
                circlePos[1] = leftPos * range;
                startPos++;
            }

            //외각원형그려주기
            for (int i = startPos; i < len; i++)
            {
                temp -= addedAngle;
                Vector2 pt = Quaternion.AngleAxis(temp, Vector3.forward) * (lLcroweUtil.GetSightDirectionType(sightTrigger.sightDirectionType) * range);
                circlePos[i] = pt;
            }
            circlePos[len - 1] = rightPos * range;

            poly2D.points = circlePos;
        }
    }
}