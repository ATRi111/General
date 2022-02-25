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
//④所有脚本的Start
//⑤所有scripts的Initialize

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
    private int _NumOfLoadObject;
    /// <summary>
    /// 剩余的需要加载的对象池数
    /// </summary>
    public int NumOfLoadObject
    {
        get => _NumOfLoadObject;
        set
        {
            if (value < 0 || value == _NumOfLoadObject) 
                return;
            if (value == 0)
            {
                Debug.Log("对象池初始化完成");
                StartCoroutine(StartGame());
            }
            _NumOfLoadObject = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _NumOfLoadObject = 0;
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
        yield return new WaitForSeconds(1f);
        Debug.Log("开始游戏");
        ServiceLocator.Instance.GetService<LoadManager>(EService.LoadManager).LoadLevel(1);
        Destroy(gameObject);
    }
}

