using UnityEngine;
using XREngine.Core.Scripts.Common;
using XREngine.Core.Scripts.Utility;
using XREngine.Core.Scripts.VR.Interactive;

namespace XREngine.Core.Scripts.VR.Player
{
    public class ControllerPointer : BasePointer
    {
        public void PointerSelect()
        {
            if (InteractableItem == null) return;
            
            InteractableItem.OnItemSelected(PointerPlayerComponent);
        }
    }
}
