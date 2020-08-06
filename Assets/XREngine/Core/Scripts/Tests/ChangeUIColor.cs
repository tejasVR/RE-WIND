using System;
using UnityEngine;
using UnityEngine.UI;
using XRCORE.Scripts.VR.Interactive;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Interactive;
using XREngine.Core.Scripts.VR.Player;

namespace XRCORE.Scripts.Tests
{
    [RequireComponent(typeof(Image))]
    public class ChangeUIColor : InteractableItem
    {
        [SerializeField] private Color normalColor; 
        [SerializeField] private Color selectedColor;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public override void OnItemSelected(PlayerComponent playerComponent)
        {
            base.OnItemSelected(playerComponent);

            if (Selected)
            {
                _image.color = normalColor;
                Selected = false;
            }
            else
            {
                _image.color = selectedColor;
                Selected = true;
            }
        }
    }
}
