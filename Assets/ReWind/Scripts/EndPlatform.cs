using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ReWind.Scripts
{
    public class EndPlatform : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private string nextLevelName;
        
        [Space(7)]
        [SerializeField] private float fillSpeed;
        
        private float _fill;
        private bool _leafLanded;
        private bool _loadingNextLevel;

        private void Start()
        {
            ChangeFill(0);
        }

        private void OnCollisionStay(Collision other)
        {
            if (!other.gameObject.CompareTag("LeafObject")) return;
            
            if (!_leafLanded) _leafLanded = true;
                
            IncreaseFill();
        }

        private void OnCollisionExit(Collision other)
        {
            if (!other.gameObject.CompareTag("LeafObject")) return;
            
            if (_leafLanded) _leafLanded = false;
        }

        private void Update()
        {
            if (!_leafLanded)
            {
                DecreaseFillImage();
            }
        }

        private void IncreaseFill()
        {
            if (_fill >= 1)
            {
                if (!_loadingNextLevel)
                {
                    NextLevel();
                }
                else
                {
                    return;
                }
                
                return;
            }
            
            ChangeFill(Time.deltaTime * fillSpeed);
        }

        private void DecreaseFillImage()
        {
            if (_fill <= 0) return;
            
            ChangeFill(-Time.deltaTime);
        }

        private void ChangeFill(float change)
        {
            _fill += change;
            fillImage.fillAmount = _fill;
        }

        private void NextLevel()
        {
            _loadingNextLevel = true;

            if (nextLevelName.Length == 0)
            {
                Debug.LogError($"End platform for {SceneManager.GetActiveScene().name} doesn't specify the next level");
                return;
            }
            
            // Load next level
            LevelLoader.Instance.SwitchLevel(nextLevelName);
        }
    }
}
