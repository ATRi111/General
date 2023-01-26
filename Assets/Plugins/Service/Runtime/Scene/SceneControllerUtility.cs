using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public static class SceneControllerUtility
    {
        public static int SceneIndex => SceneManager.GetActiveScene().buildIndex;
        public static string SceneName => SceneManager.GetActiveScene().name;

        private readonly static Dictionary<int, string> sceneNameDict;
        private readonly static Dictionary<string, int> sceneIndexDict;

        static SceneControllerUtility()
        {
            sceneNameDict = new Dictionary<int, string>();
            sceneIndexDict = new Dictionary<string, int>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string name = GetSceneName(SceneUtility.GetScenePathByBuildIndex(i));
                sceneIndexDict.Add(name, i);
                sceneNameDict.Add(i, name);
            }
        }

        public static int ToSceneIndex(string sceneName)
        {
            if (sceneIndexDict.TryGetValue(sceneName, out int index))
                return index;
            Debugger.LogWarning($"BuildSettings中没有名为{sceneName}的场景 ", EMessageType.System);
            return -1;
        }

        public static string ToSceneName(int sceneIndex)
        {
            if (sceneNameDict.TryGetValue(sceneIndex, out string name))
                return name;
            Debugger.LogWarning($"BuildSettings中没有索引为{sceneIndex}的场景 ", EMessageType.System);
            return null;
        }

        public static string GetSceneName(string path)
        {
            int l = path.LastIndexOf('/');
            int r = path.LastIndexOf('.');
            return path.Substring(l + 1, r - l - 1);
        }
    }
}