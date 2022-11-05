using Services;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class GameLauncher : MonoBehaviour
{
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
        GameInitSettings settings = Resources.Load<GameInitSettings>("GameInitSettings");
        ServiceLocator.Get<SceneControllerBase>().LoadScene(settings == null ? 1 : settings.index_startGame);
    }
}