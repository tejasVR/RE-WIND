using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR;

namespace XREngine.Core.Scripts.VR.Player
{
    /// <summary>
    /// An abstract class for the player's component like head, hands, and more
    /// </summary>
    public abstract class PlayerComponent : MonoBehaviour
    {
        [SerializeField] protected bool debug;

        [Space(7)]
        [SerializeField] protected XRNode xrNode;
        protected InputDevice Device;

        [HideIf("debug"), PropertySpace]
        [Button(ButtonSizes.Medium), GUIColor(1, 1, 1)]
        private void TurnDebugOn()
        {
            debug = !debug;
        }

        [ShowIf("debug"), PropertySpace]
        [Button(ButtonSizes.Medium), GUIColor(0, 1, 1)]
        private void TurnOffDebug()
        {
            debug = !debug;
        }
        
        protected virtual void Awake()
        {
            SetDevice();
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }
        
        public Vector3 GetPositionVelocity()
        {
            var nodes = new List<XRNodeState>();

            InputTracking.GetNodeStates(nodes);

            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].nodeType != xrNode) continue;
                
                nodes[i].TryGetVelocity(out var vel);
                return vel;
            }
            
            return Vector3.zero;
        }

        public Vector3 GetAngularVelocity()
        {
            var nodes = new List<XRNodeState>();

            InputTracking.GetNodeStates(nodes);

            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].nodeType != xrNode) continue;
                
                nodes[i].TryGetAngularVelocity(out var angVel);
                return angVel;
            }

            return Vector3.zero;
        }

        private void SetDevice()
        {
            switch (xrNode)
            {
                case XRNode.Head:
                    Device = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                    break;
                case XRNode.LeftHand:
                    Device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    break;
                case XRNode.RightHand:
                    Device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (debug) Debug.Log("Device: " + Device.characteristics);
        }
        
        protected void SetDevicePosAndRot()
        {
            // Components need a constant reference to the current state of the XRNode
            InputDevice deviceAtXrNode = InputDevices.GetDeviceAtXRNode(xrNode);

            if (!deviceAtXrNode.isValid) return;

            deviceAtXrNode.TryGetFeatureValue(CommonUsages.devicePosition, out var position);
            deviceAtXrNode.TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation);

            transform.localRotation = rotation;
            transform.localPosition = position;
        }
    }
}
