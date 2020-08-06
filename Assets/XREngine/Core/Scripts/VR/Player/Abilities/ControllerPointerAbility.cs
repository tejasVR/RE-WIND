using System;
using UnityEngine;

namespace XREngine.Core.Scripts.VR.Player.Abilities
{
    public class ControllerPointerAbility : PlayerAbility
    {
        [Header("Pointer Prefabs")]
        [SerializeField] private ControllerPointer controllerPointerPrefab;

        [Header("Pointer Settings")] 
        [SerializeField] private float pointerLength;
        [SerializeField] private Material pointerNormalMaterial;
        [SerializeField] private Material pointerHoverMaterial;

        private ControllerPointer _controllerPointer;

        private void Awake()
        {
            InstantiatePointerPrefabs();
        }

        private void InstantiatePointerPrefabs()
        {
            _controllerPointer = Instantiate(controllerPointerPrefab);

            AssignPointerHand();
        }

        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            _controllerPointer.PointerSelect();
        }

        protected override void OnSwitchHands()
        {
            switch(assignedPlayerComponent)
            {
                case AssignedPlayerComponent.DominantHand:
                    _controllerPointer.SetPointerPlayerComponent(PlayerManager.Instance.DominantHand);
                    break;
                case AssignedPlayerComponent.NonDominantHand:
                    _controllerPointer.SetPointerPlayerComponent(PlayerManager.Instance.NonDominantHand);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AssignPointerHand()
        {
            switch(assignedPlayerComponent)
            {
                case AssignedPlayerComponent.DominantHand:
                    _controllerPointer.Initialize(PlayerManager.Instance.DominantHand, pointerLength, pointerNormalMaterial, pointerHoverMaterial);
                    break;
                case AssignedPlayerComponent.NonDominantHand:
                    _controllerPointer.Initialize(PlayerManager.Instance.NonDominantHand, pointerLength, pointerNormalMaterial, pointerHoverMaterial);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
