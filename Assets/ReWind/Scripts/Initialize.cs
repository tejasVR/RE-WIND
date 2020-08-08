using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReWind.Scripts
{
    public static class Initialize
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            LevelLoader.Instance.AddScene("Level 1");
            // LevelLoader.AddSceneRoutine("Level 1", true);
        } 
    }
}
