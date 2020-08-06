using UnityEngine;

namespace XREngine.Framer.Scripts
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        
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
            return container.activeSelf;
        }
    }
}
