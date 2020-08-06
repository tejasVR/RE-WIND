using System;
using Sirenix.OdinInspector;
using UnityEngine;
using XREngine.Core.Scripts.Utility;
using XREngine.Core.Scripts.VR.Interactive;
using XREngine.Core.Scripts.VR.Player;

namespace XREngine.Core.Scripts.Common
{
    [RequireComponent(typeof(LineRenderer))]
    public abstract class BasePointer : MonoBehaviour
    {
        [SerializeField] protected bool debug;

        [Space(7)]
        [SerializeField] protected LayerMask uIBackgroundLayer;
        [SerializeField] protected LayerMask interactiveLayer;

        [Space(7)]
        [SerializeField] private bool showLine;
        
        protected InteractableItem InteractableItem;
        protected PlayerComponent PointerPlayerComponent;
        
        private LineRenderer _lineRenderer;
        private Material _pointerNormalMaterial;
        private Material _pointerHoverMaterial;
        
        private float _pointerLength;
        
        [HideIf("debug"), PropertySpace]
        [Button(ButtonSizes.Medium), GUIColor(1, 1, 1)]
        private void TurnDebugOn()
        {
            debug = !debug;
        }

        [ShowIf("debug"), PropertySpace]
        [Button(ButtonSizes.Medium), GUIColor(0, 1, 1)]
        private void TurnOffDebug()
        {
            debug = !debug;
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            if (!showLine)
            {
                _lineRenderer.enabled = false;
            }
        }
        
        protected virtual void Update()
        {
            PointerRaycast();
        }

        public virtual void Initialize(PlayerComponent pointerPlayerOrigin, float pointerLength, Material pointerNormalMaterial, Material pointerHoverMaterial)
        {
            SetPointerPlayerComponent(pointerPlayerOrigin);
            
            _pointerLength = pointerLength;
            _pointerNormalMaterial = pointerNormalMaterial;
            _pointerHoverMaterial = pointerHoverMaterial;
            
            SetupLine();
        }

        public virtual void SetPointerPlayerComponent(PlayerComponent playerComponent)
        {
            PointerPlayerComponent = playerComponent;
            transform.parent = PointerPlayerComponent.transform;
            transform.localPosition = Vector3.zero;
        }

        protected virtual void PointerRaycast()
        {
            var ray = new Ray(transform.position, transform.forward);

            var hit = RaycastUtility.RayHit(ray, _pointerLength, interactiveLayer);

            if (hit.collider != null)
            {
                if (!hit.collider.GetComponent<InteractableItem>()) return;
                
                if (hit.collider.GetComponent<InteractableItem>() == InteractableItem)
                {
                    ItemHoverStay();
                }
                else
                {
                    ItemHoverEnter(hit);
                }
            }
            else
            {
                if (InteractableItem != null)
                {
                    ItemHoverExit();
                }
            }
        }
        
        protected virtual void ItemHoverEnter(RaycastHit hit)
        {
            InteractableItem = hit.collider.GetComponent<InteractableItem>();
            InteractableItem.OnItemHoverEnter(PointerPlayerComponent);
            
            HoverLineMaterial();
        }

        protected virtual void ItemHoverStay()
        {
            InteractableItem.OnItemHoverStay(PointerPlayerComponent);
        }

        protected virtual void ItemHoverExit()
        {
            InteractableItem.OnItemHoverExit(PointerPlayerComponent);
            InteractableItem = null;
            
            NormalLineMaterial();
        }
        
        protected virtual void AdjustPointerLength(float length)
        {
            var pointerLength = new Vector3(0, 0, length);
            
            _lineRenderer.SetPosition(1, pointerLength);
        }
        
        private void SetupLine()
        {
            AdjustPointerLength(_pointerLength);

            NormalLineMaterial();
        }
        
        private void NormalLineMaterial()
        {
            _lineRenderer.material = _pointerNormalMaterial;
        }

        private void HoverLineMaterial()
        {
            _lineRenderer.material = _pointerHoverMaterial;
        }
    }
}
