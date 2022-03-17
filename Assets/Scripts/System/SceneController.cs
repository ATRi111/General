using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : Service
{
    private EventSystem eventSystem;

    private int index_menu;
    private int index_max;
    /// <summary>
    /// �Ƿ��첽���س���
    /// </summary>
    public bool asynchronous;

    /// <summary>
    /// ��ʼ�첽���س���ʱ�������첽����
    /// </summary>
    public event Action<AsyncOperation> AsyncLoadScene;

    [SerializeField]
    private int index;
    public int Index
    {
        get => index;
        private set
        {
            if (value > index_max)
                value = index_menu;
            if (value == index || value <= 0)
                return;
            index = value;
            StartCoroutine(LoadSceneProcess(value, asynchronous));
        }
    }

    protected override void Awake()
    {
        base.Awake();
        index_max = SceneManager.sceneCountInBuildSettings - 1;
        index_menu = SceneManager.GetSceneByName("menu").buildIndex;
        eventSystem = ServiceLocator.GetService<EventSystem>();
        eventSystem.CreateEvent(EEvent.BeforeLoadScene, typeof(UnityAction<int>));
        eventSystem.CreateEvent(EEvent.AfterLoadScene, typeof(UnityAction<int>));
    }

    //��ֹ�ò����ڱ���ķ������س���
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

    /// <param name="confirm">���ص�90%ʱ�Ƿ���Ҫȷ��</param>    
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