using UnityEngine;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XREngine.Framer.Scripts
{
    public class FrameLoadAbility : PlayerAbility
    {
        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            if (FrameManager.Instance == null) return;
            
            FrameManager.Instance.LoadNextFrame();
        }

        protected override void SecondaryAction(InputEventArgs eventArgs)
        {
            if (FrameManager.Instance == null) return;
            
            FrameManager.Instance.LoadPreviousFrame();
        }
    }
}
