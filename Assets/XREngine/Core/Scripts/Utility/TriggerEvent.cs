using UnityEngine;

namespace XREngine.Core.Scripts.Utility
{
    public class TriggerEvent : MonoBehaviour
    {
        [SerializeField] private string eventName;
        public string EventName => eventName;
    }
}
