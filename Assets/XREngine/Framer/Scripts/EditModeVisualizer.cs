using System;
using UnityEngine;

namespace XREngine.Framer.Scripts
{
    public class EditModeVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject visualizer;

        private void OnEnable()
        {
            FrameManager.EnterEditModeCallback += ShowVisualizer;
            FrameManager.EnterPlayModeCallback += HideVisualizer;
        }

        private void OnDisable()
        {
            FrameManager.EnterEditModeCallback -= ShowVisualizer;
            FrameManager.EnterPlayModeCallback -= HideVisualizer;
        }

        private void ShowVisualizer()
        {
            visualizer.SetActive(true);
        }

        private void HideVisualizer()
        {
            visualizer.SetActive(false);
        }
    }
}
