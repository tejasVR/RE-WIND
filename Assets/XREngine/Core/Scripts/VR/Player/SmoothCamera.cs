using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using XRCORE.Scripts.VR.Player;

namespace XREngine.Core.Scripts.VR.Player
{
    public class SmoothCamera : MonoBehaviour
    {

        [Header("Camera Smoothing Settings")] 
        [SerializeField] [Range(0, 15)] private float positionSmoothing;
        [SerializeField] [Range(0, 15)] private float rotationSmoothing;

        [Header("Camera Tech Settings")]
        [SerializeField] [Range(50,120)] private float fieldOfView;
        
        [Header("Offset Settings")]
        [SerializeField] private float xOffset;
        [SerializeField] private float yOffset;
        [SerializeField] private float zOffset;
        
        private Transform _playerHead;
        private Camera _smoothCamera;

        private void Awake()
        {
            _smoothCamera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            _playerHead = PlayerManager.Instance.PlayerHead.GetTransform();
            _smoothCamera.fieldOfView = fieldOfView;
        }

        private void Update()
        {
            // Lerp Position
            var tempPosition = _playerHead.position + new Vector3(xOffset, yOffset, zOffset);
            transform.position = Vector3.Lerp(transform.position, tempPosition, Time.deltaTime * positionSmoothing);
            
            // Lerp Rotation
            // Quaternion tempRotation = Quaternion.Euler(playerHead.rotation.x, playerHead.rotation.y, 0);
            Quaternion tempRotation = _playerHead.rotation;
            // Debug.Log(playerHead.rotation.y);
            
            // tempRotation.x = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, tempRotation, Time.deltaTime * rotationSmoothing);
        }
    }
}
