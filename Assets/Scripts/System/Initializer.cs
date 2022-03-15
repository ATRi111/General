using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInitialize
{
    void Initialize();
}

//游戏开始时部分方法执行顺序：
//①Initializer和SerciviceLocator的Awake
//②EventSystem的Awake
//③所有Service的Awake
//④所有脚本的Start和所有scripts的Initialize

/// <summary>
/// 游戏初始化器，游戏启动后自毁
/// </summary>
[DefaultExecutionOrder(-200)]
public class Initializer : Singleton<Initializer>
{
    //需要在游戏刚开始时初始化的非Monobehavior脚本
    [SerializeField]
    private List<ScriptableObject> scripts;

    [SerializeField]
    private int _Count_Initializations;
    /// <summary>
    /// 剩余的初始化任务数
    /// </summary>
    public int Count_Initializations
    {
        get => _Count_Initializations;
        set
        {
            if (value < 0 || value == _Count_Initializations)
                return;
            if (value == 0)
                StartCoroutine(StartGame());
            _Count_Initializations = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _Count_Initializations = 0;
        Random.InitState(System.DateTime.Now.Second);
    }

    private void Start()
    {
        foreach (ScriptableObject script in scripts)
        {
            (script as IInitialize)?.Initialize();
        }
    }

    private IEnumerator StartGame()
    {
        yield return null;
        Debug.Log("初始化完成");
        ServiceLocator.Instance.GetService<SceneManager>().LoadLevel(1);
        Destroy(gameObject);
    }
}

