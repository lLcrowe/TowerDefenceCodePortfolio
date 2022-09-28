using UnityEngine;

namespace lLCroweTool.Singleton
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    instance = FindObjectOfType<T>();
                    if (ReferenceEquals(instance, null))
                    {
                        GameObject tmp = new GameObject();
                        instance = tmp.AddComponent<T>();
                        tmp.name = "-=" + typeof(T).Name + "=-";
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake() //=>
        {
            //instance = transform.GetComponent<T>();                        
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            //instance = FindObjectOfType<T>();
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}