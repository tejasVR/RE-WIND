using System;
using System.Collections.Generic;
using UnityEngine;
using XREngine.Core.Scripts.VR.Player;

namespace ReWind.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class BreathSensor : MonoBehaviour
    {
        /*[SerializeField] private float micOutputDividerPC;
        [SerializeField] private float upwardsForcePC;*/
        
        [Space(7)]
        [SerializeField] private float micOutputDivider;
        [SerializeField] private float upwardsForce;
        [SerializeField] private float pushForceMultiplier;

        
        [Space(7)]
        [SerializeField] private ParticleSystem windParticles;
        [SerializeField] private float particlePlayThreshold;
        
        private Collider _breathCollider;
        private Transform _head;
        private float _breathForce;
        
        private List<LeafObject> _intersectingLeafObjects = new List<LeafObject>();
        
        /*
        private float _micOutputDivider;
        private float _upwardsForce;*/

        private void Awake()
        {
            _breathCollider = GetComponent<Collider>();
        }
        
        private void Start()
        {
            _head = PlayerManager.Instance.GetHeadTransform();
            // SetPlatformVariables();
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

        private void FixedUpdate()
        {
            CalculateBreathForce();
            PlayWindParticles(_breathForce);
        }

        /*private void SetPlatformVariables()
        {
            if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Quest)
            {
                _micOutputDivider = micOutputDivider;
                _upwardsForce = upwardsForce;
            }
            else if(OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Rift_S || OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Link_Quest)
            {
                _micOutputDivider = micOutputDividerPC;
                _upwardsForce = upwardsForcePC;
            }
            
        }*/

        private void CalculateBreathForce()
        {
            var micOutput = MicOutput.Instance.MicVolume;

            _breathForce = micOutput / micOutputDivider;

            var pushDirection = _head.transform.forward + (_head.transform.up * upwardsForce);
            
            if (_breathForce < particlePlayThreshold) return;

            foreach (var leaf in _intersectingLeafObjects)
            {
                if (leaf == null)
                {
                    _intersectingLeafObjects.Remove(leaf);
                    continue;
                }
                
                leaf.PushLeaf(pushDirection * (_breathForce * pushForceMultiplier));
            }
        }

        private void PlayWindParticles(float breathForce)
        {
            var main = windParticles.main;
            main.startSpeed = _breathForce * 5;

            if (breathForce > particlePlayThreshold)
            {
                if (!windParticles.isPlaying)
                {
                    windParticles.Play();
                }
            }
            else
            {
                if (windParticles.isPlaying)
                {
                    windParticles.Stop();
                }
            }
        }
    }
}
