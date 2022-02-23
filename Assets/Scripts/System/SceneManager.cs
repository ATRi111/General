using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Service
{
    private EventSystem eventSystem;

    /// <summary>
    /// 主菜单对应的INDEX
    /// </summary>
    public const int MENUINDEX = 2;

    [SerializeField]
    private int _Index;
    public int Index
    {
        get => _Index;
        private set
        {
            if (value > MENUINDEX)
                value = MENUINDEX;
            if (value == _Index || value <= 0)
                return;
            _Index = value;
            eventSystem.ActivateEvent(EEvent.BeforeLoadScene, value);
            LoadScene(value);
        }
    }

    protected override void BeforeRegister()
    {
        eService = EService.SceneManager;
        eventSystem = ServiceLocator.Instance.GetService<EventSystem>(EService.EventSystem);
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

    public void Exit()
    {
        Index = MENUINDEX;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
