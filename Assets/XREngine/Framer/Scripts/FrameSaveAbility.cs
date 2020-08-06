using UnityEngine;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XREngine.Framer.Scripts
{
    public class FrameSaveAbility : PlayerAbility
    {
        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            if (FrameManager.Instance == null) return;
            
            FrameManager.Instance.SaveFrame();
        }
    }
}
