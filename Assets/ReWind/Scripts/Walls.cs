using UnityEngine;

namespace ReWind.Scripts
{
    public class Walls : MonoBehaviour
    {
        private float _pushForce = 30F;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("LeafObject")) return;
            
            var leafObject = other.GetComponent<LeafObject>();
                
          leafObject.PushLeaf(-transform.forward * _pushForce);
        }
    }
}
