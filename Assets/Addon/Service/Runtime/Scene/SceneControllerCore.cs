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
        /// ��ʼ�첽���س���ʱ�������첽����
        /// </summary>
        public event UnityAction<AsyncOperation> AsyncLoadScene;

        internal event UnityAction<int> BeforeLoadScene;
        internal event UnityAction<int> AfterLoadScene;
        internal MonoBehaviour mono;

        //���ܴ���UnityAction����Ϊ�൱�ڴ���null������û�а���һ��
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

        /// <param name="confirm">���ص�90%ʱ�Ƿ���Ҫȷ��</param>
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