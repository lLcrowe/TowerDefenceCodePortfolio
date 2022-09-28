using UnityEngine;
using lLCroweTool.Dictionary;

namespace lLCroweTool
{
    [CreateAssetMenu(fileName = "New LocalizeDBData", menuName = "lLcroweTool/LocalizeDBData")]
    public class LocalizeDBObjectScript : ScriptableObject
    {
        //데이터베이스

        /// <summary>
        /// DB 버전
        /// </summary>
        public string version;//적어두는법은 릴리즈노트처럼

        /// <summary>
        /// DB데이터를 적어둔 오너들
        /// </summary>
        public string author;

        /// <summary>
        /// DB 설명
        /// </summary>
        public string description;
        //로컬라이징데이터들이 들어가 있는 데이터
        [SerializeField]public LocalizeDataBible localizeDataBible = new LocalizeDataBible();//번역정보가 들어가있는 데이터시트
    }

    [System.Serializable]
    public class LocalizeDataBible : CustomDictionary<string, LocalizeData>{}

    [System.Serializable]
    public class LocalizeData
    {
        public string string_ID;
        public string kor;
        public string eng;
    }
}