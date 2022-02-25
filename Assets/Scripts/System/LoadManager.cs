using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : Service
{
    private EventSystem eventSystem;

    [Header("主菜单的场景号")]
    public int index_menu;
    [Header("最大场景号")]
    public int index_max;
    [Header("是否异步加载")]
    public bool asynchronous;

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
            eventSystem.ActivateEvent(EEvent.BeforeLoadScene, value);
            SceneManager.LoadScene(value);
        }
    }

    protected override void BeforeRegister()
    {
        eService = EService.SceneManager;
        eventSystem = ServiceLocator.Instance.GetService<EventSystem>(EService.EventSystem);
    }

    //禁止用不属于本类的方法加载场景
    public void LoadScene(int index)
    {
        Index = index;
    }
    public void LoadNextScene()
    {
        Index++;
    }

    public void Exit()
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
}
