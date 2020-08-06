using System.Collections.Generic;
using UnityEngine;

namespace XREngine.Framer.Scripts
{
    public class FramePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        
        [Header("Player References From the Real Player in the Scene")]
        [Space(7)]
        [SerializeField] private Transform bodyReference;
        [SerializeField] private Transform headReference;
        [SerializeField] private Transform leftHandReference;
        [SerializeField] private Transform rightHandReference;
        
        [Header("Player References From the Dummy (This) Player in the Scene")]
        [Space(7)]
        [SerializeField] private Transform body;
        [SerializeField] private Transform head;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        
        private List<PlayerFrameData> _playerFrameData = new List<PlayerFrameData>(100);
        
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
        
        private void SaveFrame(int frameToSave)
        {
            var playerData = new PlayerFrameData();
            
            if (FrameManager.Instance.InEditMode)
            {
                playerData.Initialize(frameToSave, body, head, leftHand, rightHand);
            }
            else
            {
                playerData.Initialize(frameToSave, bodyReference, headReference, leftHandReference, rightHandReference);
            }
            
            if (_playerFrameData.Count == 0)
            {
                _playerFrameData.Add(playerData);
                return;
            }

            if (FrameManager.Instance.InEditMode)
            {
                _playerFrameData[frameToSave] = playerData;
                return;
            }
            
            if (FrameManager.Instance.IsLastFrame())
            {
                _playerFrameData.Add(playerData);
            }
            else
            {
                _playerFrameData[frameToSave] = playerData;
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
            body.position = _playerFrameData[frameToLoad].bodyPositionData;
            body.rotation = _playerFrameData[frameToLoad].bodyRotationData;

            // Head
            head.position = _playerFrameData[frameToLoad].headPositionData;
            head.rotation = _playerFrameData[frameToLoad].headRotationData;
            
            // Left Hand
            leftHand.position = _playerFrameData[frameToLoad].leftHandPositionData;
            leftHand.rotation = _playerFrameData[frameToLoad].leftHandRotationData;
            
            // Right Hand
            rightHand.position = _playerFrameData[frameToLoad].rightHandPositionData;
            rightHand.rotation = _playerFrameData[frameToLoad].rightHandRotationData;
        }
    }
}
