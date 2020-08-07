using System;
using System.Collections;
using UnityEngine;

namespace ReWind.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class MicOutput : MonoBehaviour
    {
        public static MicOutput Instance
        {
            get
            {
                if (_micOutput) return _micOutput;
                
                _micOutput = FindObjectOfType<MicOutput>();

                if (!_micOutput)
                {
                    Debug.LogError("There needs to be a PlayerManager in the scene!");
                }
                else
                {
                    _micOutput.Initialize();
                }

                return _micOutput;
            }
        }
        private static MicOutput _micOutput;
        
        public float MicVolume => _clipLoudness;

        [SerializeField] private int sampleDataLength = 1024;

        private AudioSource _micAudio;
        private const int OutputSampleRate = 44100;
        private float[] _clipSampleData;
        private float _clipLoudness;
        
        private void Awake()
        {
            _micAudio = GetComponent<AudioSource>();
        }
        
        private void Start()
        {
            StartCoroutine(CaptureMicAudio());
        }

        private void Initialize()
        {
            _micOutput = this;
        }

        private void FixedUpdate()
        {
            MeasureMicOutputVolume();
        }

        private IEnumerator CaptureMicAudio()
        {
            _micAudio.clip = Microphone.Start(null, true, 1, OutputSampleRate);
            _micAudio.loop = true;
            while (!(Microphone.GetPosition(null) > 0)) { }
            
            //_micAudio.Play();
            
            yield return null;
        }

        private void MeasureMicOutputVolume()
        {
            if (!_micAudio) return;
            
            _clipSampleData = new float[sampleDataLength];
            
            _micAudio.clip.GetData(_clipSampleData, _micAudio.timeSamples);
                
            _clipLoudness = 0f;
                
            foreach (var sample in _clipSampleData)
            {
                _clipLoudness += Mathf.Abs(sample);
            }

            _clipLoudness = Mathf.Clamp(_clipLoudness, 0, 100);
        }


    }
}
