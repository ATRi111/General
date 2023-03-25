using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public interface ISceneController : IService
    {
        /// <summary>
        /// 开始异步加载场景时，发送异步操作，如果不关心加载进度，使用EventSystem中的事件即可
        /// </summary>
        UnityEvent<AsyncOperation> AsyncLoadScene { get; }

        /// <summary>
        /// 加载索引号为当前场景索引号+1的场景
        /// </summary>
        void LoadNextScene(LoadSceneMode mode = LoadSceneMode.Single);
        /// <summary>
        /// 加载指定索引号的场景
        /// </summary>
        void LoadScene(int index, LoadSceneMode mode = LoadSceneMode.Single);
        /// <summary>
        /// 加载指定名称的场景
        /// </summary>
        void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single);
        /// <summary>
        /// 结束游戏
        /// </summary>
        void Quit();
    }
}