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
        public readonly UnityEvent<AsyncOperation> AsyncLoadScene = new();

        /// <summary>
        /// 开始异步卸载场景时，发送异步操作
        /// </summary>
        public readonly UnityEvent<AsyncOperation> AsyncUnLoadScene = new();

        internal readonly UnityEvent<int> BeforeLoadScene = new();
        internal readonly UnityEvent<int> AfterLoadScene = new();
        internal readonly UnityEvent<int> BeforeUnLoadScene = new();
        internal readonly UnityEvent<int> AfterUnLoadScene = new();
        internal MonoBehaviour mono;

        internal SceneControllerCore(
            MonoBehaviour mono,
            UnityAction<int> BeforeLoadScene,
            UnityAction<int> AfterLoadScene,
            UnityAction<int> BeforeUnLoadScene,
            UnityAction<int> AfterUnLoadScene)
        {
            this.mono = mono;
            this.BeforeLoadScene.AddListener(BeforeLoadScene);
            this.AfterLoadScene.AddListener(AfterLoadScene);
            this.BeforeUnLoadScene.AddListener(BeforeUnLoadScene);
            this.AfterUnLoadScene.AddListener(AfterUnLoadScene);
        }

        public void LoadScene(int index, LoadSceneMode mode, bool async, bool confirm)
        {
            mono.StartCoroutine(LoadScene_(index, mode, async, confirm));
        }
        public void UnLoadScene(int index, UnloadSceneOptions options)
        {
            mono.StartCoroutine(UnLoadScene_(index, options));
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private IEnumerator LoadScene_(int index, LoadSceneMode mode, bool async, bool needConfirm)
        {
            BeforeLoadScene?.Invoke(index);
            if (async)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(index, mode);
                operation.allowSceneActivation = !needConfirm;
                AsyncLoadScene?.Invoke(operation);
                yield return operation;
            }
            else
            {
                SceneManager.LoadScene(index, mode);
            }
            yield return null;
            AfterLoadScene?.Invoke(index);
        }

        private IEnumerator UnLoadScene_(int index, UnloadSceneOptions options)
        {
            BeforeUnLoadScene?.Invoke(index);
            AsyncOperation operation = SceneManager.UnloadSceneAsync(index, options);
            AsyncUnLoadScene?.Invoke(operation);
            yield return operation;
            AfterUnLoadScene?.Invoke(index);
        }
    }
}