using System;
using Sirenix.OdinInspector;
using UnityEngine;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player;

namespace XREngine.Core.Scripts.VR.Interactive
{
    public class InteractableItem : MonoBehaviour
    {
        protected event Action ItemHoverEnterCallback;
        protected event Action ItemHoverStayCallback;
        protected event Action ItemHoverExitCallback;
        protected event Action ItemSelectedCallback;
        protected event Action ItemUnselectedCallback;

        [SerializeField] private bool debug;

        protected bool Selected;

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
        
        public virtual void OnItemHoverEnter(PlayerComponent playerComponent)
        {
            ItemHoverEnterCallback?.Invoke();

            if (debug)
                Debug.Log(gameObject.name + " called Hover Enter by " + playerComponent.name);
        }

        public virtual void OnItemHoverStay(PlayerComponent playerComponent)
        {
            ItemHoverStayCallback?.Invoke();

            if (debug)
                Debug.Log(gameObject.name + " is calling Hover Stay");
        }

        public virtual void OnItemHoverExit(PlayerComponent playerComponent)
        {
            ItemHoverExitCallback?.Invoke();

            if (debug)
                Debug.Log(gameObject.name + " is calling Hover Exit");
        }

        public virtual void OnItemSelected(PlayerComponent playerComponent)
        {
            ItemSelectedCallback?.Invoke();

            if (debug)
                Debug.Log(gameObject.name + " is calling Item Selected");
        }
        
        public virtual void OnItemUnselected(PlayerComponent playerComponent)
        {
            ItemUnselectedCallback?.Invoke();

            if (debug)
                Debug.Log(gameObject.name + " is calling Item Unselected");
        }
    }
}
