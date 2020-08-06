using System;
using UnityEngine;

namespace XREngine.Framer.Scripts
{
    [Serializable]
    public struct PlayerFrameData
    {
        public int frame;
        
        // Body
        public Vector3 bodyPositionData;
        public Quaternion bodyRotationData;

        // Head
        public Vector3 headPositionData;
        public Quaternion headRotationData;
        
        // Left Hand
        public Vector3 leftHandPositionData;
        public Quaternion leftHandRotationData;
        
        // Right Hand
        public Vector3 rightHandPositionData;
        public Quaternion rightHandRotationData;

        public void Initialize(int frame, Transform body, Transform head, Transform leftHand, Transform rightHand)
        {
            this.frame = frame;
            
            // Body
            bodyPositionData = body.position;
            bodyRotationData = body.rotation;

            // Head
            headPositionData = head.position;
            headRotationData = head.rotation;
            
            // Left Hand
            leftHandPositionData = leftHand.position;
            leftHandRotationData = leftHand.rotation;
            
            // Right Hand
            rightHandPositionData = rightHand.position;
            rightHandRotationData = rightHand.rotation;
        }
        
    }
}
