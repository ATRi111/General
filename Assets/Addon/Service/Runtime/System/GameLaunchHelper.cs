using Services.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    //此类禁止访问ServiceLocator类
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
            Debugger.Settings.Copy();
            Debugger.Settings.SetAllowLog(EMessageType.Service, false); 
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string name = SceneControllerUtility.GetSceneName(SceneUtility.GetScenePathByBuildIndex(i));
                if (name == SceneControllerUtility.SceneName)
                {
                    GameLauncher.StartGameIndex = i;
                    SceneManager.LoadScene(0);
                    return;
                }
            }
            Debugger.Settings.Paste();
        }
#endif
    }
}