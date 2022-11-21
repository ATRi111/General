using Services;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class GameLauncher : MonoBehaviour
{
    public static int StartGameIndex;

    private static int count_incomplete;
    /// <summary>
    /// 剩余的初始化任务数
    /// </summary>
    public static int Count_Incomplete
    {
        get => count_incomplete;
        set
        {
            if (value < 0 || value == count_incomplete)
                return;
            if (value == 0)
                StartGame();
            count_incomplete = value;
        }
    }

    static GameLauncher()
    {
        StartGameIndex = -1;
    }

    private void Awake()
    {
        Count_Incomplete = 1;
    }

    private void Start()
    {
        Invoke(nameof(Initialize), 1f);
    }

    private void Initialize()
    {
        Random.InitState(System.DateTime.Now.Second);
        Count_Incomplete--;
    }

    private static void StartGame()
    {
        if (StartGameIndex == -1)
        {
            GameInitSettings settings = Resources.Load<GameInitSettings>("GameInitSettings");
            StartGameIndex = settings == null ? 1 : settings.startGameIndex;
        }
        ServiceLocator.Get<SceneControllerBase>().LoadScene(StartGameIndex);
    }
}