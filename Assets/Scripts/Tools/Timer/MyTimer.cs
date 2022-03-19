using UnityEngine;

[System.Serializable]
/// <summary>
/// 描述一段随时间推进发生的变化
/// </summary>
public class MyTimer<T>
{
    private readonly GameCycle gameCycle;

    [SerializeField]
    private bool paused = true;
    /// <summary>
    /// 是否暂停，被创建时默认暂停,弃用Timer前，一定要将其暂停
    /// </summary>
    public bool Paused
    {
        get => paused;
        set
        {
            if (paused != value)
            {
                if (value)
                    gameCycle.RemoveFromGameCycle(EUpdateMode.Update, OnUpdate);
                else
                    gameCycle.AttachToGameCycle(EUpdateMode.Update, OnUpdate);
                paused = value;
            }
        }
    }

    [SerializeField]
    private bool completed;
    /// <summary>
    /// 是否完成
    /// </summary>
    public bool Completed
    {
        get => completed;
        protected set
        {
            if(completed != value)
            {
                if (value)
                {
                    JustCompleted = true;
                    gameCycle.AttachToGameCycle(EUpdateMode.NextLateUpdate, OnComplete);
                }
                completed = value;
            }
        }
    }
    /// <summary>
    /// 是否在上一次Update之后下一次LateUpdate之前完成
    /// </summary>
    public bool JustCompleted { get; protected set; }

    /// <summary>
    /// 经过的时间
    /// </summary>
    public float Timer { get; protected set; }
    /// <summary>
    /// 到达的百分比（0～1)
    /// </summary>
    public float Percent => Mathf.Clamp(Timer / Duration, 0f, 1f); 
    /// <summary>
    /// 总时间
    /// </summary>
    public float Duration { get; protected set; }
    /// <summary>
    /// 初值
    /// </summary>
    public T Origin { get; protected set; }
    /// <summary>
    /// 终值
    /// </summary>
    public T Target { get; protected set; }
    /// <summary>
    /// 当前值
    /// </summary>
    public virtual T Current => default;

    public MyTimer()
    {
        gameCycle = Service.Get<GameCycle>();
        paused = true;
        completed = false;
    }

    /// <summary>
    /// 为MyTimer设置初始属性及是否启动（默认启动）
    /// </summary>
    public virtual void Initialize(T origin, T target, float duration, bool start = true)
    {
        Duration = duration;
        Origin = origin;
        Target = target;
        Paused = !start;
    }

    protected virtual void OnUpdate()
    {
        Timer += Time.deltaTime;
        if(Timer >= Duration)
        {
            Completed = true;
            Paused = true;
        }
    }

    private void OnComplete()
    {
        JustCompleted = false;
    }

    /// <summary>
    /// 重新开始计时
    /// </summary>
    public virtual void Restart()
    {
        Paused = false;
        Timer = 0;
        Completed = false;
    }

    /// <summary>
    /// 强制快进到完成
    /// </summary>
    public void Complete()
    {
        Timer = Duration;
        Paused = true;
        Completed = true;
    }
}

/// <summary>
/// 基本的往复变化
/// </summary>
public class Circulation<T> : MyTimer<T> where T : struct
{
    protected override void OnUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer >= Duration)
        {
            Timer -= Duration;
            T temp = Origin;
            Origin = Target;
            Target = temp;
        }
    }
}

/// <summary>
/// 基本的反复变化
/// </summary>
public class Repeataion<T> : MyTimer<T> where T : struct
{
    protected override void OnUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer >= Duration)
        {
            Timer -= Duration;
        }
    }
}

