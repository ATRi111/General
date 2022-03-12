using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : Service
{
    private EventSystem eventSystem;

    private int index_menu;
    private int index_max;
    /// <summary>
    /// 是否异步加载场景
    /// </summary>
    public bool asynchronous;

    /// <summary>
    /// 开始异步加载场景时，发送异步操作
    /// </summary>
    public event Action<AsyncOperation> AsyncLoadScene;

    [SerializeField]
    private int _Index;
    public int Index
    {
        get => _Index;
        private set
        {
            if (value > index_max)
                value = index_menu;
            if (value == _Index || value <= 0)
                return;
            _Index = value;
            StartCoroutine(LoadSceneProcess(value, asynchronous));
        }
    }

    protected override void Awake()
    {
        base.Awake();
        index_max = SceneManager.sceneCountInBuildSettings;
        index_menu = SceneManager.GetSceneByName("menu").buildIndex;
        eventSystem = ServiceLocator.Instance.GetService<EventSystem>();
    }

    //禁止用不属于本类的方法加载场景
    public void LoadLevel(int index)
    {
        Index = index;
    }
    public void LoadNextLevel()
    {
        Index++;
    }
    public void RetuenToMenu()
    {
        Index = index_menu;
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
    private IEnumerator LoadSceneProcess(int index, bool asynchronous, bool confirm = false)
    {
        eventSystem.ActivateEvent(EEvent.BeforeLoadScene, index);
        if (asynchronous)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(index);
            AsyncLoadScene?.Invoke(operation);
            operation.allowSceneActivation = !confirm;
            operation.allowSceneActivation = false;
            for (; !operation.isDone;)
            {
                yield return null;
            }
        }
        else
        {
            SceneManager.LoadScene(index);
        }
        yield return null;
        eventSystem.ActivateEvent(EEvent.AfterLoadScene, index);
    }
}