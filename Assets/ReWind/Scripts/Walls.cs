using UnityEngine;

namespace ReWind.Scripts
{
    public class Walls : MonoBehaviour
    {
        private float _pushForce = .5F;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("LeafObject")) return;
            
            var leafObject = other.gameObject.GetComponent<LeafObject>();
                
          leafObject.PushLeaf(-transform.forward * _pushForce);
        }
    }
}
