using System;
using System.Collections;
using UnityEngine;

namespace XRCORE.Scripts.VR.Interactive
{
    public class UIItem : MonoBehaviour
    {
        public enum UIType
        {
            Background,
            Interactive,        
        }

        [Header("UI Settings")]
        [SerializeField] private UIType uIType;
        
        private Canvas _rootCanvas;
        private bool _createdCollider;

        private void Awake()
        {
            CheckIfUILayer();       
        }
        
        private void OnEnable()
        {
            if (!_createdCollider)
            {
                StartCoroutine(Create3DCollider());
            }
        }

        private IEnumerator Create2DCollider()
        {
            // Wait until next frame when layout groups have resized
            yield return new WaitForEndOfFrame();

            // Get current UI dimensions 
            var rectTransform = GetComponent<RectTransform>();
            var width = rectTransform.rect.width;
            var height = rectTransform.rect.height;

            // Size a new 2d collider component with the same dimensions
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(width, height);

            _createdCollider = true;
        }
        
        private IEnumerator Create3DCollider()
        {
            // Wait until next frame when layout groups have resized
            yield return new WaitForEndOfFrame();

            // Get current UI dimensions 
            var rectTransform = GetComponent<RectTransform>();
            var length = rectTransform.rect.width;
            var height = rectTransform.rect.height;
            const float width = .001F;

            // Size a new 2d collider component with the same dimensions
            var collider = gameObject.AddComponent<BoxCollider>();        
            collider.size = new Vector3(length, height, width);

            _createdCollider = true;
        }
        
        private void CheckIfUILayer()
        {
            switch (uIType)
            {
                case UIType.Background:
                    CheckLayer("UI");
                    break;

                case UIType.Interactive:
                    CheckLayer("Interactable");
                    break;
            }        
        }
        
        private void CheckLayer(string layerName)
        {
            // Check is object is on UI layer
            if (gameObject.layer.ToString() != layerName)
            {
                gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
    }
}
