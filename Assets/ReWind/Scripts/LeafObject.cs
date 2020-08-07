using UnityEngine;

namespace ReWind.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class LeafObject : MonoBehaviour
    {
        [SerializeField] private float leafGravityMagnitude = 50F;
    
        private Rigidbody _rb;
        private Vector3 _leafGravity;
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _leafGravity = (-transform.up * 9.8F) / leafGravityMagnitude;
        }

        private void FixedUpdate()
        {
            ApplyLeafGravity();
        }

        public void PushLeaf(Vector3 pushDirection)
        {
            _rb.AddForce(pushDirection, ForceMode.Impulse);
            
            _rb.AddTorque(pushDirection /2, ForceMode.Impulse);

        }

        private void ApplyLeafGravity()
        {
            _rb.AddForce(_leafGravity);
            
            
            //transform.Translate(_leafGravity);
        }
    }
}
