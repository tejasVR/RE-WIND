using UnityEngine;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XRCORE.Scripts.Tests
{
    public class ChangeCubeAbility : PlayerAbility
    {
        [Space(7)]
        [SerializeField] private Material cubeMaterial;
        
        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            var randomColor = new Color(
                Random.Range(0f, 1f), 
                Random.Range(0f, 1f), 
                Random.Range(0f, 1f)
            );

            cubeMaterial.color = randomColor;
        }
    }
}
