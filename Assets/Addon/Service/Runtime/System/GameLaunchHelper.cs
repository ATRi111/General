using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    [DefaultExecutionOrder(-1000)]
    internal class GameLaunchHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RestartCheck()
        {
            GameObject serviceLocator = GameObject.Find("ServiceLocator");
            if (serviceLocator == null)
                Restart();
        }

        private static void Restart()
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string name = SceneControllerUtility.GetSceneName(SceneUtility.GetScenePathByBuildIndex(i));
                if (name == SceneControllerUtility.SceneName)
                {
                    GameInitSettings initSettings = Resources.Load<GameInitSettings>("GameInitSettings");
                    initSettings.index_startGame = i;
                    SceneManager.LoadScene(0);
                    return;
                }
            }

            Debug.LogError("请将当前场景添加到BuildSettings中");
            Application.Quit();
        }
#endif
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}