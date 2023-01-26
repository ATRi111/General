using Services.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class SceneControllerBase : Service, ISceneController
    {
        [AutoService]
        protected IEventSystem eventSystem;

        protected SceneControllerCore core;

        /// <summary>
        /// �Ƿ��첽���س���
        /// </summary>
        public bool async;
        /// <summary>
        /// �첽���س���ʱ�Ƿ���Ҫȷ��
        /// </summary>
        public bool needConfirm;

        public UnityEvent<AsyncOperation> AsyncLoadScene => core.AsyncLoadScene;

        protected override void Awake()
        {
            base.Awake();
            core = new SceneControllerCore(this, BeforeLoadScene, AfterLoadScene);
        }

        public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
            => core.LoadScene(SceneControllerUtility.ToSceneIndex(name), mode, async, needConfirm);

        public void LoadScene(int index, LoadSceneMode mode = LoadSceneMode.Single)
            => core.LoadScene(index, mode, async, needConfirm);
        public void LoadNextScene(LoadSceneMode mode = LoadSceneMode.Single)
            => LoadScene(SceneControllerUtility.SceneIndex + 1, mode);

        public void Quit()
            => core.Quit();

        private void BeforeLoadScene(int index)
        {
            eventSystem.Invoke(EEvent.BeforeLoadScene, index);
        }

        private void AfterLoadScene(int index)
        {
            eventSystem.Invoke(EEvent.AfterLoadScene, index);
        }
    }
}