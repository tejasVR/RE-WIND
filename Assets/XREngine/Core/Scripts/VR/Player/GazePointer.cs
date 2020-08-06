using UnityEngine;
using XREngine.Core.Scripts.Common;

namespace XREngine.Core.Scripts.VR.Player
{
    public class GazePointer : BasePointer
    {
        public override void SetPointerPlayerComponent(PlayerComponent playerComponent)
        {
            PointerPlayerComponent = playerComponent;

            transform.parent = PointerPlayerComponent.GetComponent<Head>().GetTransform();
            transform.localPosition = Vector3.zero;
        }
    }
}
