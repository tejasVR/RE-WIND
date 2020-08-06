using UnityEngine;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XREngine.Framer.Scripts
{
    public class FramePointerAbility : PlayerAbility
    {
        [Header("Frame Pointer Settings")]
        [SerializeField] private Pointer pointer;
        
        private bool _isPointerShown;

        protected override void Start()
        {
            base.Start();
            
            HidePointer();
        }

        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            if (!_isPointerShown)
            {
                ShowPointer();
            }
            else
            {
                HidePointer();
            }
        }

        private void ShowPointer()
        {
            pointer.Show();
            _isPointerShown = true;
        }

        private void HidePointer()
        {
            pointer.Hide();
            _isPointerShown = false;
        }
    }
}
