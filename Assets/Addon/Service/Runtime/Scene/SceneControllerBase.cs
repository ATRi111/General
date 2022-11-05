using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneControllerBase : Service
    {
        [Other]
        protected EventSystem eventSystem;

        protected SceneControllerCore core;

        /// <summary>
        /// 是否异步加载场景
        /// </summary>
        public bool async;
        /// <summary>
        /// 异步加载场景时是否需要确认
        /// </summary>
        public bool needConfirm;

        /// <summary>
        /// 开始异步加载场景时，发送异步操作，如果不关心加载进度，使用EventSystem中的事件即可
        /// </summary>
        public event UnityAction<AsyncOperation> AsyncLoadScene
        {
            add => core.AsyncLoadScene += value;
            remove => core.AsyncLoadScene -= value;
        }

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