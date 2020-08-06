using System;
using System.Collections.Generic;
using UnityEngine;

namespace XREngine.Core.Scripts.VR.Player
{
    public class TeleportPuck : MonoBehaviour
    {
        #region  EVENTS

        public event Action TeleportCollidingCallback = delegate {  };
        public event Action TeleportNormalCallback = delegate {  };
        
        #endregion
        
        public bool IsColliding { get; private set; }

        [SerializeField] private GameObject puckModel;
        [SerializeField] private Renderer teleportCylinder;
        // [SerializeField] LineRenderer teleportLine;

        //[Header("Cylinder Materials")]
        private Material _availableMaterial;
        private Material _unavailableMaterial;

        // [Header("Line Materials")]
        // [SerializeField] private Material availableLine;
        // [SerializeField] private Material unavailableLine;

        private Collider _teleportCollider;
        private List<Collider> _collidingWith = new List<Collider>();

        private void Awake()
        {
            _teleportCollider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            AvailableMaterial();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_teleportCollider.bounds.Intersects(other.bounds)) return;
            
            if (other.CompareTag("Teleport Area")) return;
                
            IsColliding = true;
            _collidingWith.Add(other);

            TeleportCollidingCallback();
        }

        private void OnTriggerExit(Collider other)
        {
            _collidingWith.Remove(other);

            if (_collidingWith.Count == 0)
            {
                IsColliding = false;

                TeleportNormalCallback();
            }
        }

        public void Initialize(Material availableMat, Material unavailableMat)
        {
            _availableMaterial = availableMat;
            _unavailableMaterial = unavailableMat;
            
            AvailableMaterial();
        }

        public void ShowPuck()
        {
            puckModel.SetActive(true);
        }

        public void HidePuck()
        {
            puckModel.SetActive(false);
        }

        public void AvailableMaterial()
        {
            teleportCylinder.material = _availableMaterial;
        }

        public void UnavailableMaterial()
        {
            teleportCylinder.material = _unavailableMaterial;
        }
    }
}