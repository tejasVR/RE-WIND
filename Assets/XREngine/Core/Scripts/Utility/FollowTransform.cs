using System;
using UnityEngine;

namespace XREngine.Core.Scripts.Utility
{
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform transformToFollow;

        [Header("Follow Settings")]
        [SerializeField] private float positionFollowSpeed; 
        [SerializeField] private float rotationFollowSpeed; 
        
        [Header("Follow Behavior")]
        [SerializeField] private bool lerpFollow;
        
        private void Update()
        {
            if (lerpFollow)
            {
                transform.position = Vector3.Lerp(transform.position, transformToFollow.position, Time.deltaTime * positionFollowSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, transformToFollow.rotation,Time.deltaTime * rotationFollowSpeed);
            }
            else
            {
                transform.position = transformToFollow.position;
                transform.rotation = transformToFollow.rotation;
            }
        }
    }
}
