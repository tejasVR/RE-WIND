using UnityEngine;

namespace ReWind.Scripts
{
    public class Floor : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("LeafObject")) return;

            LevelLoader.Instance.RestartCurrentLevel();
        }
    }
}
