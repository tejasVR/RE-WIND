using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReWind.Scripts
{
    public class LevelLoader: MonoBehaviour
    {
        public static LevelLoader Instance
        {
            get
            {
                if (_levelLoader) return _levelLoader;
                
                _levelLoader = FindObjectOfType<LevelLoader>();

                if (!_levelLoader)
                {
                    Debug.LogError("There needs to be a PlayerManager in the scene!");
                }
                else
                {
                    _levelLoader.Initialize();
                }

                return _levelLoader;
            }
        }
        private static LevelLoader _levelLoader;

        private string _currentLevel;
        
        private void Initialize()
        {
            _levelLoader = this;
        }
        
        

        public void AddScene(string sceneToLoad)
        {
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            // if (setAsActiveScene) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

            // StartCoroutine(AddSceneRoutine(sceneToLoad, setAsActiveScene));

            _currentLevel = sceneToLoad;

        }

        private IEnumerator AddSceneRoutine(string sceneToLoad, bool setAsActiveScene)
        {
            var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

            yield return new WaitUntil(() => asyncLoadLevel.isDone);
            
            if (setAsActiveScene) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
        }
        
        public void SwitchLevel(string levelToLoad)
        {
            // StartCoroutine(LoadLevelRoutine(levelToUnload, levelToLoad));
            
            SceneManager.UnloadSceneAsync(_currentLevel);

            SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
            
            _currentLevel = levelToLoad;

        }

        static IEnumerator LoadLevelRoutine(string levelToUnload, string levelToLoad)
        {
            SceneManager.LoadSceneAsync(levelToLoad);

            SceneManager.UnloadSceneAsync(levelToUnload);
         
            yield return null;

            // yield return WaitUntil(() => SceneManager.lo)
        }

        public void RestartCurrentLevel()
        {
            StartCoroutine(RestartCurrentLevelRoutine());
        }

        IEnumerator RestartCurrentLevelRoutine()
        {
            var asyncLoadLevel = SceneManager.UnloadSceneAsync(_currentLevel);

            yield return new WaitUntil(() => asyncLoadLevel.isDone);
            
            SceneManager.LoadSceneAsync(_currentLevel, LoadSceneMode.Additive);
        }
    }
}
