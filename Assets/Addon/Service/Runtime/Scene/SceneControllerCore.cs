using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneControllerCore
    {
        /// <summary>
        /// 开始异步加载场景时，发送异步操作
        /// </summary>
        public event UnityAction<AsyncOperation> AsyncLoadScene;

        internal event UnityAction<int> BeforeLoadScene;
        internal event UnityAction<int> AfterLoadScene;
        internal MonoBehaviour mono;

        //不能传空UnityAction，因为相当于传了null，二者没有绑定在一起
        internal SceneControllerCore(MonoBehaviour mono, UnityAction<int> BeforeLoadScene, UnityAction<int> AfterLoadScene)
        {
            this.mono = mono;
            this.BeforeLoadScene = BeforeLoadScene;
            this.AfterLoadScene = AfterLoadScene;
        }

        public void LoadScene(int index, LoadSceneMode mode, bool async, bool confirm)
        {
            StartLoadScene(new LoadSceneProcess(index, mode, async, confirm));
        }
        private void StartLoadScene(LoadSceneProcess process)
        {
            mono.StartCoroutine(LoadScene_(process));
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        /// <param name="confirm">加载到90%时是否需要确认</param>
        private IEnumerator LoadScene_(LoadSceneProcess process)
        {
            BeforeLoadScene?.Invoke(process.index);
            if (process.async)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(process.index);
                operation.allowSceneActivation = !process.needConfirm;
                AsyncLoadScene?.Invoke(operation);
                yield return operation;
            }
            else
            {
                SceneManager.LoadScene(process.index);
            }
            yield return null;
            AfterLoadScene?.Invoke(process.index);
        }
    }
}