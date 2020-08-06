using System;
using UnityEngine;

namespace XREngine.Framer.Scripts
{
    [Serializable]
    public struct FrameData
    {
        public int Frame;

        public Vector3 PositionData;
        public Quaternion RotationData;

        public bool Shown;

        public void Initialize(int frame, Transform transform, bool shown)
        {
            Frame = frame;

            PositionData = transform.position;
            RotationData = transform.rotation;

            Shown = shown;
        }
    }
}
