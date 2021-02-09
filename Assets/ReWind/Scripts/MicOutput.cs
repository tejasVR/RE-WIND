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
        private bool _microphoneInitialized;
        AudioClip _microphoneInput;
        private string _microphone;
        
        private void Awake()
        {
            _micAudio = GetComponent<AudioSource>();
        }
        
        private void Start()
        {
            // StartCoroutine(CaptureMicAudio());
            StartMicrophone();
            UpdateMicrophone();

            _clipSampleData = new float[sampleDataLength];

        }

        private void Initialize()
        {
            _micOutput = this;
        }

        private void Update()
        {
            MeasureMicOutputVolume();
        }

        private void StartMicrophone()
        {
            if (Microphone.devices.Length <= 0) return;

            foreach (var device in Microphone.devices)
            {
                if (device == null) continue;
                
                _microphone = device;
                break;
            }
        }

        private IEnumerator CaptureMicAudio()
        {
            _micAudio.clip = Microphone.Start(null, true, 1, OutputSampleRate);
            _micAudio.loop = true;
            while (!(Microphone.GetPosition(null) > 0)) { }
            
            //_micAudio.Play();
            
            yield return null;
        }

        private void UpdateMicrophone()
        {
            _micAudio.Stop();
            _microphoneInput = Microphone.Start(_microphone, true, 999, AudioSettings.outputSampleRate);
            _micAudio.loop = true;
            
            _microphoneInitialized = true;
            
            Debug.Log($"Microphone Initialized = {_microphoneInitialized}");

            if (!Microphone.IsRecording(_microphone)) return;
            
            while (!(Microphone.GetPosition(_microphone) > 0)) {}

            _micAudio.clip = _microphoneInput;
            
            _micAudio.Play();
        }

        private void MeasureMicOutputVolume()
        {
            /*int dec = 128;
            float[] waveData = new float[dec];
            int micPosition = Microphone.GetPosition(null)-(dec+1); // null means the first microphone*/
            
            // Debug.Log($"Mic position is {micPosition}");
            
            // Debug.Log($"Wave Data is {waveData[0]}");
            
            _microphoneInput.GetData(_clipSampleData, _micAudio.timeSamples);

            float levelMax = 0;
            for (int i = 0; i < sampleDataLength; i++) {
                float wavePeak = _clipSampleData[i] * _clipSampleData[i];
                if (levelMax < wavePeak) {
                    levelMax = wavePeak;
                }
            }
            
            /*foreach (var sample in _clipSampleData)
            {
                _clipLoudness += Math.Abs(sample);
            }
            
            _clipLoudness = sampleDataLength * 10000;*/
            
            
            // Getting a peak on the last 128 samples
        
            _clipLoudness = Mathf.Sqrt(Mathf.Sqrt(levelMax));
            
            Debug.Log($"Microphone Loudness = {_clipLoudness}");

            /*if (!_micAudio) return;
            
            _clipSampleData = new float[sampleDataLength];
            
            _micAudio.clip.GetData(_clipSampleData, _micAudio.timeSamples);
                
            _clipLoudness = 0f;
                
            foreach (var sample in _clipSampleData)
            {
                _clipLoudness += Mathf.Abs(sample);
            }

            _clipLoudness = Mathf.Clamp(_clipLoudness, 0, 100);*/
        }


    }
}
