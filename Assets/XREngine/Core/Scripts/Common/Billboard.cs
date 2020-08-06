using UnityEngine;

namespace XREngine.Core.Scripts.Common
{
    public class Billboard : MonoBehaviour
    {
        private enum LookAtDirection
        {
            PlayerHead,
            VectorZero,
            Self
        }

        [Tooltip("An enumeration to set what object this object should look at")]
        [SerializeField] LookAtDirection lookAtDirection;
        [SerializeField] private bool invertDirection;

        private Transform _destinationTransform;

        private void Start()
        {
            GetLookDirection();
        }

        private void Update()
        {
            LookAtDestination();
        }

        private void GetLookDirection()
        {
            switch (lookAtDirection)
            {
                case LookAtDirection.PlayerHead:
                    _destinationTransform = Camera.main.transform;
                    break;

                // case LookAtDirection.VectorZero:
                //     _destinationTransform = Vector3.zero;
                //     break;

                case LookAtDirection.Self:
                    _destinationTransform = transform;
                    break;
            }

            if (invertDirection)
            {
                //transform.LookAt(lookAtVector, Vector3.down);
                transform.LookAt(_destinationTransform, new Vector3(0, -1, -1));
            }
            else
            {
                transform.LookAt(_destinationTransform);
            }

        }

        private void LookAtDestination()
        {
            if (invertDirection)
            {
                //transform.LookAt(lookAtVector, Vector3.down);
                transform.LookAt(_destinationTransform, new Vector3(0, -1, -1));
            }
            else
            {
                transform.LookAt(_destinationTransform);
            }
        }
    }

}

