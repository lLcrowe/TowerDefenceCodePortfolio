using lLCroweTool;
using UnityEngine;
using UnityEngine.Events;

namespace Assets
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EventTrigger : MonoBehaviour
    {
        [Tag] public string tag;
        public UnityEvent unityEvent;

        private void Awake()
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tag))
            {
                unityEvent.Invoke();
            }
        }
    }
}