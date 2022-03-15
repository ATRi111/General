using System;
using System.Collections;
using UnityEngine;

public class SceneManager : Service
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
        index_max = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        index_menu = UnityEngine.SceneManagement.SceneManager.GetSceneByName("menu").buildIndex;
        eventSystem = ServiceLocator.Instance.GetService<EventSystem>();
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
        eventSystem.ActivateEvent(EEvent.BeforeLoadScene, index);
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
        eventSystem.ActivateEvent(EEvent.AfterLoadScene, index);
    }
}