using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace XREngine.Core.Scripts.VR.Player
{
    public class Hand : PlayerComponent
    {
        #region VARIABLES

        public enum HandOrientation
        {
            Left,
            Right
        }
        
        public HandOrientation Handedness { get; private set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            
            SetHandOrientation();
        }
        
        private void LateUpdate()
        {
            SetDevicePosAndRot();
        }

        private void SetHandOrientation()
        {
            switch (xrNode)
            {
                case XRNode.LeftHand:
                    Handedness = HandOrientation.Left;
                    break;
                case XRNode.RightHand:
                    Handedness = HandOrientation.Right;
                    break;
            }
        }
    }
}