using Oculus.Platform.Samples.VrHoops;
using UnityEngine;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XREngine.Framer.Scripts
{
    public class FrameAdjustHeightAbility : PlayerAbility
    {
        [SerializeField] private float heightAdjustSpeed;

        private GameObject _playerBody;

        protected override void Start()
        {
            base.Start();

            _playerBody = PlayerManager.Instance.gameObject;
        }

        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            RaiseHeight();
        }

        protected override void SecondaryAction(InputEventArgs eventArgs)
        {
            LowerHeight();
        }

        private void RaiseHeight()
        {
            var tempPlayerPosition = PlayerManager.Instance.transform.position;

            tempPlayerPosition.y += heightAdjustSpeed * Time.deltaTime;

            _playerBody.transform.position = tempPlayerPosition;
        }

        private void LowerHeight()
        {
            var tempPlayerPosition = PlayerManager.Instance.transform.position;

            tempPlayerPosition.y -= heightAdjustSpeed * Time.deltaTime;

            _playerBody.transform.position = tempPlayerPosition;
        }
        
    }
}
