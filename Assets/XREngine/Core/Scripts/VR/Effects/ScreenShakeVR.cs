using System.Collections.Generic;
using UnityEngine;

namespace XREngine.Core.Scripts.VR.Effects
{
    [ExecuteInEditMode]
    public class ScreenShakeVR : MonoBehaviour
    {
        public static ScreenShakeVR Instance
        {
            get
            {
                if (_screenShakeVr) return _screenShakeVr;

                _screenShakeVr = FindObjectOfType<ScreenShakeVR>();

                if (!_screenShakeVr)
                {
                    Debug.Log("There is no ScreenShakeVR object in the scene");
                }
                else
                {
                    _screenShakeVr.Initialize();
                }

                return _screenShakeVr;
            }
        }

        private static ScreenShakeVR _screenShakeVr;

        private Material _material;

        [SerializeField] private float shakeMagnitude = 0.1f;
        [SerializeField] private  float shakeFrequency = 20f;

        private float _shakeVal;
        private float _shakeAccumulation;

        [Tooltip("Shake the screen when the space key is pressed")]
        public bool debug = false;

        public class ShakeEvent
        {
            public float magnitude;
            public float length;

            private float _exponent;

            public bool Finished { get { return _time >= length; } }
            public float CurrentStrength { get { return magnitude * Mathf.Clamp01(1 - _time / length); } }

            public ShakeEvent(float mag, float len, float exp = 2)
            {
                magnitude = mag;
                length = len;
                _exponent = exp;
            }

            private float _time;

            public void Update(float deltaTime)
            {
                _time += deltaTime;
            }
        }

        public List<ShakeEvent> activeShakes = new List<ShakeEvent>();

        // Creates a private material used to the effect

        private void Initialize()
        {
            _screenShakeVr = this;

            if (_material != null)
            {
                _material.shader = Shader.Find("Hidden/ScreenShakeVR");
            }
            else
            {
                _material = new Material(Shader.Find("Hidden/ScreenShakeVR"));
            }
        }

        private void OnEnable()
        {
            Initialize();
        }

        /// <summary>
        /// Trigger a shake event
        /// </summary>
        /// <param name="magnitude">Magnitude of the shaking. Should range from 0 - 1</param>
        /// <param name="length">Length of the shake event.</param>
        /// <param name="exponent">Falloff curve of the shaking</param>
        public void Shake(float magnitude, float length, float exponent = 2)
        {
            activeShakes.Add(new ShakeEvent(magnitude, length, exponent));
        }


        /// <summary>
        /// Trigger a global shake event
        /// </summary>
        /// <param name="magnitude">Magnitude of the shaking. Should range from 0 - 1</param>
        /// <param name="length">Length of the shake event.</param>
        /// <param name="exponent">Falloff curve of the shaking</param>
        public static void TriggerShake(float magnitude, float length, float exponent = 2)
        {
            Instance.Shake(magnitude, length, exponent);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && debug)
            {
                Shake(shakeMagnitude, shakeFrequency);
            }

            _shakeAccumulation = 0;
            //iterate through all active shake events
            for (int i = activeShakes.Count - 1; i >= 0; i--)
            {
                //accumulate their current magnitude
                activeShakes[i].Update(Time.deltaTime);
                _shakeAccumulation += activeShakes[i].CurrentStrength;
                //and remove them if they've finished
                if (activeShakes[i].Finished)
                {
                    activeShakes.RemoveAt(i);
                }
            }

            if (_shakeAccumulation > 0)
            {
                _shakeVal = Mathf.PerlinNoise(Time.time * shakeFrequency, 10.234896f) * _shakeAccumulation * shakeMagnitude;
            }
            else
            {
                _shakeVal = 0;
            }
        }

        // Postprocess the image
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (Mathf.Approximately(_shakeVal, 0) == false)
            {
                _material.SetFloat("_ShakeFac", _shakeVal);
                Graphics.Blit(source, destination, _material);
            }
            else
            {
                //no shaking currently
                Graphics.Blit(source, destination);
            }
        }
    }
}