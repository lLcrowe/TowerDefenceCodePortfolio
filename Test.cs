using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class Test : MonoBehaviour
    {

        public float distance;
        public ContactFilter2D filter2D;
        private Transform tr;
        [SerializeField] public List<RaycastHit2D> hit2DList = new List<RaycastHit2D>();
        [SerializeField] public List<RaycastHit2D> hit2D2List = new List<RaycastHit2D>();
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        private void Awake()
        {
            tr = transform;
        }

        void Update()
        {
            //속도비교후 체크


            int i;
            Vector3 dirUp = tr.up * distance;
            string tempContent = "None";


            stopwatch.Start();

            for (int k = 0; k < 1000; k++)
            {
                if (Physics2D.Linecast(tr.position, dirUp + tr.position, filter2D, hit2DList) > 0)
                {
                    tempContent = "Exist" + hit2DList.Count;
                }
            }
            stopwatch.Stop();
            Debug.Log(tempContent + "Line" + stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();

            for (int k = 0; k < 1000; k++)
            {
                if (Physics2D.Raycast(tr.position, dirUp, filter2D, hit2D2List) > 0)
                {
                    tempContent = "Exist" + hit2DList.Count;
                }
            }
            stopwatch.Stop();
            Debug.Log(tempContent + "Ray" + stopwatch.ElapsedMilliseconds);
            Debug.DrawRay(tr.position, dirUp, Color.red);
        }
    }
}