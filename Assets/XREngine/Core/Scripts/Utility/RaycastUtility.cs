using UnityEngine;

namespace XREngine.Core.Scripts.Utility
{
    public class RaycastUtility : MonoBehaviour
    {
        public static RaycastHit RayHit(Ray ray, float rayLength, LayerMask layerMask)
        {
            return Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask) ? hit : default;
        }
    }
}
