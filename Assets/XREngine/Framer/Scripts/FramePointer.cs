using System;
using System.Collections.Generic;
using UnityEngine;

namespace XREngine.Framer.Scripts
{
    public class FramePointer : MonoBehaviour
    {
        [SerializeField] private GameObject container;

        [SerializeField] private Pointer pointerRefernece;
        
        private List<FrameData> _pointerFrameData = new List<FrameData>(100);
        
        private void OnEnable()
        {
            FrameManager.SaveFrameCallback += SaveFrame;
            FrameManager.EnterEditModeCallback += EnterEditMode;
            FrameManager.EnterPlayModeCallback += EnterPlayMode;
            FrameManager.LoadFrameCallback += LoadFrame;
        }

        private void OnDisable()
        {
            FrameManager.SaveFrameCallback -= SaveFrame;
            FrameManager.EnterEditModeCallback -= EnterEditMode;
            FrameManager.EnterPlayModeCallback -= EnterPlayMode;
            FrameManager.LoadFrameCallback -= LoadFrame;
        }

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            container.SetActive(true);
        }

        public void Hide()
        {
            container.SetActive(false);
        }

        private void SaveFrame(int frameToSave)
        {
            var pointerData = new FrameData();
            
            if (FrameManager.Instance.InEditMode)
            {
                pointerData.Initialize(frameToSave, transform, container.activeSelf);
            }
            else
            {
                pointerData.Initialize(frameToSave, pointerRefernece.transform, pointerRefernece.IsShown());
            }

            if (_pointerFrameData.Count == 0)
            {
                _pointerFrameData.Add(pointerData);
                return;
            }

            if (FrameManager.Instance.InEditMode)
            {
                _pointerFrameData[frameToSave] = pointerData;
                return;
            }
           
            if (FrameManager.Instance.IsLastFrame())
            {
                _pointerFrameData.Add(pointerData);
            }
            else
            {
                _pointerFrameData[frameToSave] = pointerData;
            }
        }
        
        private void EnterPlayMode()
        {
            container.SetActive(false);
        }

        private void EnterEditMode()
        {
            container.SetActive(true);
            LoadFrame(FrameManager.Instance.CurrentFrame);
        }
        
        private void LoadFrame(int frameToLoad)
        {
            // Body
            transform.position = _pointerFrameData[frameToLoad].PositionData;
            transform.rotation = _pointerFrameData[frameToLoad].RotationData;

           container.SetActive(_pointerFrameData[frameToLoad].Shown);
        }
    }
}
