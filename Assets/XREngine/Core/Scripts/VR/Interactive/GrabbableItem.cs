using System;
using UnityEngine;
using XRCORE.Scripts.VR.Interactive;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player;

namespace XREngine.Core.Scripts.VR.Interactive
{
    [RequireComponent(typeof(Rigidbody), typeof(AudioSource), typeof(Collider))]
    public class GrabbableItem : InteractableItem
    {
        private enum GrabType
        {
            Free,
            LerpToHand,
            SnapToHandCenter
        }
        
        [Header("Grab Settings")]
        [SerializeField] private GrabType grabType;
        [SerializeField] private AudioClip grabSFX;
        
        private Rigidbody _rb;
        private Hand _grabbingHand;
        private AudioSource _audioSource;
        private PhysicsProperties _physicsProperties;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (grabType == GrabType.LerpToHand)
            {
                LerpToHand();
            }
        }

        public override void OnItemSelected(PlayerComponent playerComponent)
        {
            base.OnItemSelected(playerComponent);

            if (playerComponent.GetComponent<Hand>())
            {
                Grab(playerComponent.GetComponent<Hand>());
            }
            
        }

        public override void OnItemUnselected(PlayerComponent playerComponent)
        {
            base.OnItemUnselected(playerComponent);
            
            Ungrab();
        }

        private void Grab(Hand hand)
        {
            _grabbingHand = hand;
            
            // Save current physics properties
            _physicsProperties = new PhysicsProperties(_rb.isKinematic, _rb.useGravity);

            switch (grabType)
            {
                case GrabType.Free:
                    transform.parent = _grabbingHand.transform;
                    break;
                case GrabType.SnapToHandCenter:
                    transform.parent = _grabbingHand.transform;
                    transform.localPosition = Vector3.zero;
                    transform.rotation = _grabbingHand.transform.rotation;
                    break;
            }

            ApplyPhysicsProperties(true, false);
            
            PlayGrabSound();
        }
        
        private void Ungrab()
        {
            switch (grabType)
            {
                case GrabType.Free:
                    transform.parent = null;
                    break;
                case GrabType.SnapToHandCenter:
                    transform.parent = null;
                    break;
            }
            
            ApplyPhysicsProperties(_physicsProperties.kinematic, _physicsProperties.enableGravity);
            
            _rb.velocity = _grabbingHand.GetPositionVelocity();
            _rb.angularVelocity = _grabbingHand.GetAngularVelocity();
            
            _grabbingHand = null;
        }      
        
        private void LerpToHand()
        {
            if (_grabbingHand == null) return;
            
            transform.position = Vector3.Slerp(transform.position, _grabbingHand.transform.position, Time.deltaTime * 12F);
            transform.rotation = Quaternion.Slerp(transform.rotation, _grabbingHand.transform.rotation, Time.deltaTime * 12F);
        }
        
        private void PlayGrabSound()
        {
            if (grabSFX != null)
            {
                _audioSource.PlayOneShot(grabSFX);
            }
        }

        private void ApplyPhysicsProperties(bool kinematic, bool enableGravity)
        {
            _rb.isKinematic = kinematic;
            _rb.useGravity = enableGravity;
        }
    }

    [Serializable]
    public struct PhysicsProperties
    {
        public bool kinematic;
        public bool enableGravity;

        public PhysicsProperties(bool kinematic, bool enableGravity)
        {
            this.kinematic = kinematic;
            this.enableGravity = enableGravity;
        }
        
    }
}
