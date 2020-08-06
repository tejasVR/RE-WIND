using System;
using UnityEngine;
using XREngine.Core.Scripts.Utility;
using XREngine.Core.Scripts.VR.Interactive;

namespace XREngine.Core.Scripts.VR.Player.Abilities
{
    public class GazeAbility : PlayerAbility
    {
        [Header("Pointer Prefabs")]
        [SerializeField] private GazePointer gazePointerPrefab;

        [Header("Pointer Settings")] 
        [SerializeField] private float pointerLength;
        [SerializeField] private Material pointerNormalMaterial;
        [SerializeField] private Material pointerHoverMaterial;
        
        private GazePointer _gazePointer;
        
        private void Awake()
        {
            InstantiatePointerPrefabs();
        }
        
        
        private void InstantiatePointerPrefabs()
        {
            _gazePointer = Instantiate(gazePointerPrefab);

            _gazePointer.Initialize(PlayerManager.Instance.PlayerHead, pointerLength, pointerNormalMaterial, pointerHoverMaterial);
        }
    }
}
