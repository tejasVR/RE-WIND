using System;
using System.Linq.Expressions;
using UnityEngine;
using XREngine.Core.Scripts.Common;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XRCORE.Scripts.VR.Player
{
    /// <summary>
    /// Teleports the player to different locations along a bezier curve
    /// </summary>
    public class TeleportAbility : PlayerAbility
    {
        public bool TeleportEnabled { get; private set; }

        [Header("Teleportation Prefabs")]
        [SerializeField] private Bezier bezierLinePrefab;
        [SerializeField] private TeleportPuck teleportPuckPrefab;
        
        [Header("Teleport Puck Materials")]
        [SerializeField] private Material availablePuckMat;
        [SerializeField] private Material unavailablePuckMat;

        [Header("Line Materials")]
        [SerializeField] private Material availableLineMat;
        [SerializeField] private Material unavailableLineMat;

        private Bezier _bezierLine;
        private TeleportPuck _teleportPuck;
        
        private float _playerHeight;

        private void Awake()
        {
            InstantiateTeleportPrefabs();
        }

        protected override void Start()
        {
            base.Start();
            
            TeleportEnabled = false;

            _playerHeight = PlayerManager.Instance.transform.position.y;
        }

        private void Update()
        {
            if (!TeleportEnabled) return;
            
            HandleTeleport();
        }
        
        private void InstantiateTeleportPrefabs()
        {
            // Initialize Teleport Puck
            _teleportPuck = Instantiate(teleportPuckPrefab);
            _teleportPuck.Initialize(availablePuckMat, unavailablePuckMat);
            _teleportPuck.HidePuck();


            _teleportPuck.TeleportNormalCallback += TeleportAvailable;
            _teleportPuck.TeleportCollidingCallback += TeleportUnavailable;

            // Initialize Bezier Curve
            _bezierLine = Instantiate(bezierLinePrefab);
        }

        protected override void SetUpHandsOnStart()
        {
            SetUpBezierCurveHand();
        }

        protected override void OnSwitchHands()
        {
            SetUpBezierCurveHand();
        }

        private void SetUpBezierCurveHand()
        {
            var unavailMat = unavailableLineMat;
            var availMat = availableLineMat;
            
            switch (assignedPlayerComponent)
            {
                case AssignedPlayerComponent.DominantHand:
                    _bezierLine.Initialize(PlayerManager.Instance.DominantHand.transform, availMat, unavailMat);
                    break;
                case AssignedPlayerComponent.NonDominantHand:
                    _bezierLine.Initialize(PlayerManager.Instance.NonDominantHand.transform,  availMat, unavailMat);
                    break;
            }
        }

      

        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            ToggleTeleportMode();
            
            if (debug) Debug.Log("Teleporting with " + eventArgs.Hand.name);
        }

        protected override void SecondaryAction(InputEventArgs eventArgs)
        {
            if (_teleportPuck.IsColliding)
            {
                ToggleTeleportMode();
                
                return;
            }
                    
            TeleportToPosition(_bezierLine.EndPoint);
            ToggleTeleportMode();

        }

        private void TeleportAvailable()
        {
            _teleportPuck.AvailableMaterial();
            _bezierLine.AvailableMaterial();
        }
        
        private void TeleportUnavailable()
        {
            _teleportPuck.UnavailableMaterial();
            _bezierLine.UnavailableMaterial();
        }

        private void HandleTeleport()
        {
            if (!_bezierLine.endPointDetected) return;
            
            // There is a point to teleport to. Display the teleport point.
            _teleportPuck.ShowPuck();
            _teleportPuck.transform.position = _bezierLine.EndPoint;
        }

        private void TeleportToPosition(Vector3 teleportPos)
        {
            var player = PlayerManager.Instance.gameObject;
            
            player.transform.position = teleportPos + (Vector3.up * _playerHeight);
        }

        private void ToggleTeleportMode()
        {
            TeleportEnabled = !TeleportEnabled;
            
            _bezierLine.ToggleDraw(TeleportEnabled);
            
            if (!TeleportEnabled)
            {
                _teleportPuck.HidePuck();
            }
        }


    }
}


