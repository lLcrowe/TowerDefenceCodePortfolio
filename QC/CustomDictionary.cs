using System.Collections.Generic;
using UnityEngine;
using System;

namespace lLCroweTool.Dictionary
{
    /// <summary>
    /// 커스텀딕셔너리, 시리얼 라이징가능하게 제작됨
    /// </summary>
    /// <typeparam name="T1">키 타입</typeparam>
    /// <typeparam name="T2">밸류 타입</typeparam>    
    public class CustomDictionary<T1, T2> : Dictionary<T1, T2>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<T1> keyList = new List<T1>();//키값
        [SerializeField] private List<T2> valueList = new List<T2>();//밸류값

        //ISerializationCallbackReceiver는
        //OnBeforeSerialize는 데이터를 저장할 때 호출
        //OnAfterDeserialize는 데이터를 불러올 때 호출
        /// <summary>
        /// 데이터를 저장할떄 호출하는 함수
        /// ISerializationCallbackReceiver 인터페이스함수
        /// </summary>
        public void OnBeforeSerialize()
        {
            keyList.Clear();
            valueList.Clear();

            foreach (KeyValuePair<T1, T2> pair in this)
            {
                keyList.Add(pair.Key);
                valueList.Add(pair.Value);
            }
        }

        /// <summary>
        /// 데이터를 불려올때 호출하는 함수
        /// ISerializationCallbackReceiver 인터페이스함수
        /// </summary>
        public void OnAfterDeserialize()
        {
            Clear();

            for (int i = 0; i < keyList.Count; i++)
            {
                Add(keyList[i], valueList[i]);
            }
        }

        /// <summary>
        /// 딕셔너리자료들을 인스펙터에 싱크맞춤
        /// </summary>
        public void SyncDictionaryToInspector()
        {
            //리스트 초기화
            keyList.Clear();
            valueList.Clear();

            foreach (KeyValuePair<T1, T2> pair in this)
            {
                keyList.Add(pair.Key); valueList.Add(pair.Value);
            }
        }

        /// <summary>
        /// 인스펙터자료들을 딕셔너리에 싱크를 맞춤
        /// </summary>
        public void SyncInspectorToDictionary()
        {
            //딕셔너리 초기화
            Clear();

            for (int i = 0; i < keyList.Count; i++)
            {
                //중복된 키가 있다면 에러 출력
                if (ContainsKey(keyList[i]))
                {
                    Debug.LogError("중복된 키가 있습니다.");
                    //넘어감
                    //break;
                }
                Add(keyList[i], valueList[i]);
            }
        }

        public List<T1> GetKeyList()
        {
            return keyList;
        }
        public List<T2> GetValueList()
        {
            return valueList;
        }
    }

    //사용법//여기서 [System.Serializable] 안붙히면 작동안됨 그리고 필드에다도 똑같이 SerializeField 붙여야지 작동됨
    //[System.Serializable]
    //public class TestBible : CustomDictionary<string, string> { }
    //TestBible testBible = new TestBible();
}
