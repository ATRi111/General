using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : Service
{
    private EventSystem eventSystem;

    public static int Index_Boot { get; private set; } = 1;
    public static int Index_Max { get; private set; }
    /// <summary>
    /// 是否异步加载场景
    /// </summary>
    public bool asynchronous;

    /// <summary>
    /// 开始异步加载场景时，发送异步操作
    /// </summary>
    public event UnityAction<AsyncOperation> AsyncLoadScene;

    [SerializeField]
    private int index;
    public int Index
    {
        get => index;
        private set
        {
            if (value > Index_Max)
                value = Index_Boot;
            if (value == index || value <= 0)
                return;
            index = value;
            StartCoroutine(LoadSceneProcess(value, asynchronous));
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Index_Max = SceneManager.sceneCountInBuildSettings - 1;
        eventSystem = ServiceLocator.GetService<EventSystem>();
        eventSystem.CreateEvent(EEvent.BeforeLoadScene, typeof(UnityAction<int>));
        eventSystem.CreateEvent(EEvent.AfterLoadScene, typeof(UnityAction<int>));
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
        Index = Index_Boot;
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
    private IEnumerator LoadSceneProcess(int index, bool async, bool confirm = false)
    {
        eventSystem.Invoke(EEvent.BeforeLoadScene, index);
        if (async)
        {
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
            AsyncLoadScene?.Invoke(operation);
            operation.allowSceneActivation = !confirm;
            operation.allowSceneActivation = false;
            yield return operation;
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        }
        yield return null;
        eventSystem.Invoke(EEvent.AfterLoadScene, index);
    }
}