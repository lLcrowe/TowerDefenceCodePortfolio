using UnityEngine;

namespace lLCroweTool
{
    public class MousePointer : MonoBehaviour
    {
        private static MousePointer instance;
        public static MousePointer Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    instance = FindObjectOfType<MousePointer>();
                    if (ReferenceEquals(instance, null))
                    {
                        GameObject gameObject = new GameObject();
                        instance = gameObject.AddComponent<MousePointer>();
                    }
                }
                return instance;
            }
        }

        //포인터만 사용
        public Vector2 mouseWorldPosition;
        public Vector2 mouseScreenPosition;
        private Transform tr;
#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.
        private Camera camera;
#pragma warning restore CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.

        private void Awake()
        {
            instance = this;
            tr = transform;
            gameObject.name = "MousePointer";
            camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            //mouseWorldPosition = camera.ViewportToWorldPoint(Input.mousePosition);
            mouseScreenPosition = Input.mousePosition;
            tr.position = mouseWorldPosition;
        }

        private void OnDestroy()
        {
            instance = null;
            tr = null;
            camera = null;
        }
    }
}
