using System;
using UnityEngine;

namespace XREngine.Framer.Scripts
{
    public class FrameManager : MonoBehaviour
    {
        public static  event Action<int> SaveFrameCallback = delegate(int i) {  };
        public static  event Action<int> LoadFrameCallback = delegate(int i) {  };
        public static  event Action EnterEditModeCallback = delegate {  };
        public static  event Action EnterPlayModeCallback = delegate {  };

        public static FrameManager Instance
        {
            get
            {
                if (_frameManager) return _frameManager;
                
                _frameManager = FindObjectOfType<FrameManager>();

                if (!_frameManager)
                {
                    Debug.LogError("There needs to be a FrameManager in the scene!");
                }
                else
                {
                    _frameManager.Initialize();
                }

                return _frameManager;
            }
        }

        private static FrameManager _frameManager;

        public bool InEditMode { get; private set; }

        // [SerializeField] private GameObject framePlayer;

        public int CurrentFrame => _currentFrame;

        private int _currentFrame;
        private int _totalFrames;
        private bool _firstFrameTaken;
        
        private void Awake()
        {
            _totalFrames = 0;
            _currentFrame = 0;
        }

        private void Start()
        {
            EnterPlayMode();
        }

        private void Initialize()
        {
            _frameManager = this;
        }

        public void SaveFrame()
        {
            if (!_firstFrameTaken)
            {
                _firstFrameTaken = true;
                SaveFrameCallback(_currentFrame);
                return;                
            }

            if (!InEditMode)
            {
                _currentFrame++;
                
                if (_currentFrame > _totalFrames)
                {
                    _totalFrames = _currentFrame;
                }
            }
            
            SaveFrameCallback(_currentFrame);
        }

        public void LoadPreviousFrame()
        {
            if (_currentFrame <= 0) return;
            
            _currentFrame--;

            LoadFrame(_currentFrame);
        }

        public void LoadNextFrame()
        {
            if (_currentFrame >= _totalFrames) return;

            _currentFrame++;
            
            LoadFrame(_currentFrame);
        }

        public bool IsLastFrame()
        {
            return _currentFrame == _totalFrames;
        }

        private void LoadFrame(int frameToLoad)
        {
            if (!InEditMode)
            {
                EnterEditMode();
            }

            LoadFrameCallback(frameToLoad);
        }

        public void ToggleEditMode()
        {
            InEditMode = !InEditMode;

            if (InEditMode)
            {
                EnterEditMode();
            }
            else
            {
                EnterPlayMode();
            }
        }
        
        private void EnterEditMode()
        {
            EnterEditModeCallback();

            // framePlayer.SetActive(true);
            InEditMode = true;
        }

        private void EnterPlayMode()
        {
            EnterPlayModeCallback();
            // framePlayer.SetActive(false);
            InEditMode = false;
        }
    }
}
