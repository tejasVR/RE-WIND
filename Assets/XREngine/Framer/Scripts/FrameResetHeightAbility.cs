using UnityEngine;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XREngine.Framer.Scripts
{
    public class FrameResetHeightAbility : PlayerAbility
    {
        private float _originalPlayerHeight;
        private GameObject _playerBody;
        
        protected override void Start()
        {
            base.Start();

            _playerBody = PlayerManager.Instance.gameObject;
            _originalPlayerHeight = _playerBody.transform.position.y;
        }

        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            ResetHeight();
        }

        private void ResetHeight()
        {
            var tempPlayerPosition = _playerBody.transform.position;

            tempPlayerPosition.y = _originalPlayerHeight;

            _playerBody.transform.position = tempPlayerPosition;
        }
    }
}
