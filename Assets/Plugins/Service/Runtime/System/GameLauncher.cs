using Services;
using Services.SceneManagement;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class GameLauncher : MonoBehaviour
{
    public static int StartGameIndex;

    private static int taskCount;
    /// <summary>
    /// 剩余的初始化任务数
    /// </summary>
    public static int TaskCount
    {
        get => taskCount;
        set
        {
            if (value < 0 || value == taskCount)
                return;
            if (value == 0)
                StartGame();
            taskCount = value;
        }
    }

    static GameLauncher()
    {
        StartGameIndex = -1;
    }

    private void Awake()
    {
        TaskCount = 1;
    }

    private void Start()
    {
        Invoke(nameof(Initialize), 1f);
    }

    private void Initialize()
    {
        Random.InitState(System.DateTime.Now.Second);
        TaskCount--;
    }

    private static void StartGame()
    {
        if (StartGameIndex == -1)
        {
            GameInitSettings settings = Resources.Load<GameInitSettings>(nameof(GameInitSettings));
            StartGameIndex = settings == null ? 1 : settings.startGameIndex;
        }
        ServiceLocator.Get<ISceneController>().LoadScene(StartGameIndex);
    }
}