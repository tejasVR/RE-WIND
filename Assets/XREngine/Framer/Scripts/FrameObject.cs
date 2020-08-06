using System;
using System.Collections.Generic;
using UnityEngine;

namespace XREngine.Framer.Scripts
{
    public class FrameObject : MonoBehaviour
    {

        [SerializeField] private GameObject container;
        
        private List<FrameData> _frameData = new List<FrameData>(100);

        private void OnEnable()
        {
            FrameManager.SaveFrameCallback += SaveFrame;
            FrameManager.EnterEditModeCallback += EnterEditMode;
            FrameManager.LoadFrameCallback += LoadFrame;
        }

        private void OnDisable()
        {
            FrameManager.SaveFrameCallback -= SaveFrame;
            FrameManager.EnterEditModeCallback -= EnterEditMode;
            FrameManager.LoadFrameCallback -= LoadFrame;
        }

        public void Show()
        {
            container.SetActive(true);
        }

        public void Hide()
        {
            container.SetActive(false);
        }

        public bool IsShown()
        {
            return container.activeInHierarchy;
        }

        private void SaveFrame(int frameToSave)
        {
            var data = new FrameData();
            data.Initialize(frameToSave, transform, container.activeInHierarchy);

            if (_frameData.Count == 0)
            {
                _frameData.Add(data);
                return;
            }

            if (FrameManager.Instance.InEditMode)
            {
                _frameData[frameToSave] = data;
                return;
            }
            
            if (FrameManager.Instance.IsLastFrame())
            {
                _frameData.Add(data);
            }
            else
            {
                _frameData[frameToSave] = data;
            }
        }
        
        private void EnterEditMode()
        {
            TurnOffPhysics();
            LoadFrame(FrameManager.Instance.CurrentFrame);
        }

        private void LoadFrame(int frameToLoad)
        {
            transform.position = _frameData[frameToLoad].PositionData;
            transform.rotation = _frameData[frameToLoad].RotationData;
            
            container.SetActive(_frameData[frameToLoad].Shown);
        }
        
        private void TurnOffPhysics()
        {
            if (!GetComponent<Rigidbody>()) return;
            
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        
        

        
    }
}
