using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    [DefaultExecutionOrder(-1000)]
    internal class GameLaunchHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Awake()
        {
            GameObject serviceLocator = GameObject.Find("ServiceLocator");
            if (serviceLocator == null)
                Restart();
            else
                Destroy(gameObject);
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
                    Debug.Log(initSettings.index_startGame);
                    SceneManager.LoadScene(0);
                    return;
                }
            }
        }
#endif
    }
}