using UnityEngine;
using UnityEngine.Events;

namespace Boquinhas.Util
{
    public class EventTrigger : MonoBehaviour
    {
        public UnityEvent awakeEvent;
        public UnityEvent startEvent;

        private void Awake()
        {
            awakeEvent.Invoke();
        }

        private void Start()
        {
            startEvent.Invoke();
        }
    }
}