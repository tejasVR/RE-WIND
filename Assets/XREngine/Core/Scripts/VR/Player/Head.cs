using System;
using UnityEngine;

namespace XREngine.Core.Scripts.VR.Player
{
    public class Head : PlayerComponent
    {
        private void LateUpdate()
        {
            SetDevicePosAndRot();
        }
    }
}
