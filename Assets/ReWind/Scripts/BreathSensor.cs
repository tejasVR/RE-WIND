using System;
using System.Collections.Generic;
using UnityEngine;
using XREngine.Core.Scripts.VR.Player;

namespace ReWind.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class BreathSensor : MonoBehaviour
    {
        [SerializeField] private float micOutputDivider;
        [SerializeField] private float upwardsForce;
        
        private Collider _breathCollider;
        private Transform _head;
        private float _breathForce;
        
        private List<LeafObject> _intersectingLeafObjects = new List<LeafObject>();

        private void Awake()
        {
            _breathCollider = GetComponent<Collider>();
        }
        
        private void Start()
        {
            _head = PlayerManager.Instance.GetHeadTransform();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("LeafObject")) return;
            
            var leafObject = other.GetComponent<LeafObject>();
                
            if (!_intersectingLeafObjects.Contains(leafObject))
            {
                _intersectingLeafObjects.Add(leafObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("LeafObject")) return;
            
            var leafObject = other.GetComponent<LeafObject>();
                
            if (_intersectingLeafObjects.Contains(leafObject))
            {
                _intersectingLeafObjects.Remove(leafObject);
            }
        }

        private void Update()
        {
            CalculateBreathForce();
        }

        private void CalculateBreathForce()
        {
            var micOutput = MicOutput.Instance.MicVolume;

            _breathForce = micOutput / micOutputDivider;

            var pushDirection = _head.transform.forward + (_head.transform.up * upwardsForce);

            foreach (var leaf in _intersectingLeafObjects)
            {
                leaf.PushLeaf(pushDirection * _breathForce);
            }
        }
    }
}
