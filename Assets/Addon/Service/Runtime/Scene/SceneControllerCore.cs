using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class SceneControllerCore
    {
        /// <summary>
        /// 开始异步加载场景时，发送异步操作
        /// </summary>
        public readonly UnityEvent<AsyncOperation> AsyncLoadScene = new UnityEvent<AsyncOperation>();

        internal readonly UnityEvent<int> BeforeLoadScene = new UnityEvent<int>();
        internal readonly UnityEvent<int> AfterLoadScene = new UnityEvent<int>();
        internal MonoBehaviour mono;

        internal SceneControllerCore(MonoBehaviour mono, UnityAction<int> BeforeLoadScene, UnityAction<int> AfterLoadScene)
        {
            this.mono = mono;
            this.BeforeLoadScene.AddListener(BeforeLoadScene);
            this.AfterLoadScene.AddListener(AfterLoadScene);
        }

        public void LoadScene(int index, LoadSceneMode mode, bool async, bool confirm)
        {
            StartLoadScene(new LoadSceneRequest(index, mode, async, confirm));
        }
        private void StartLoadScene(LoadSceneRequest request)
        {
            mono.StartCoroutine(LoadScene_(request));
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        private IEnumerator LoadScene_(LoadSceneRequest request)
        {
            BeforeLoadScene?.Invoke(request.index);
            if (request.async)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(request.index);
                operation.allowSceneActivation = !request.needConfirm;
                AsyncLoadScene?.Invoke(operation);
                yield return operation;
            }
            else
            {
                SceneManager.LoadScene(request.index);
            }
            yield return null;
            AfterLoadScene?.Invoke(request.index);
        }
    }
}