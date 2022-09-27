using System.Collections;
using UnityEngine;

namespace Assets.NodeMap
{
    public class MapMarkerData : ScriptableObject
    {
        //대상이되는 선행마커데이터
        public MapMarkerData targetPrecedeMarkerData;

        //ObjectDeScription Data
        //유닛은 다오브젝트로 칭함 => 20191221
        //오브젝트해석기 및 설명용
        public Sprite objectSprite;//오브젝트의 아이콘 이미지
        [Space]
        public string objectName;//오브젝트의 이름
        //[TextArea]//너무크다
        [Multiline]
        public string objectShortDescription;//오브젝트의 짧은설명
        [TextArea]
        public string objectDescription;//오브젝트의 설명
    }
}