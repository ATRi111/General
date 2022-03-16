using UnityEngine;

/// <summary>
/// 描述一段随时间推进发生的变化
/// </summary>
public class MyTimer<T> where T : struct
{
    private readonly GameCycle gameCycle;
    /// <summary>
    /// 暂停时，OnFixedUpdate无行为
    /// </summary>
    public bool Paused { get; set; }
    /// <summary>
    /// 经过的时间
    /// </summary>
    public float Timer { get; set; }
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
    public virtual bool Completed => Timer >= Duration;
    public MyTimer(T origin, T target, float duration)
    {
        Duration = duration;
        Origin = origin;
        Target = target;
        gameCycle = ServiceLocator.GetService<GameCycle>();
        gameCycle.AttachToGameCycle(EUpdateMode.Update,OnUpdate);
    }

    protected virtual void OnUpdate()
    {
        if (Paused)
            return;
        Timer += Time.deltaTime;
        if (Completed)
        {
            Paused = true;
            Timer = Duration;
        }
    }

    public virtual void Restart()
    {
        Paused = false;
        Timer = 0;
    }

    /// <summary>
    /// 永久停止，之后一般要回收
    /// </summary>
    public void Dispose()
    {
        gameCycle.RemoveFromGameCycle(EUpdateMode.Update, OnUpdate);
    }

}

/// <summary>
/// 基本的往复变化
/// </summary>
public class Circulation<T> : MyTimer<T> where T : struct
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Completed)
        {
            Timer -= Duration;
            T temp = Origin;
            Origin = Target;
            Target = temp;
            Paused = false;
        }
    }

    public Circulation(T origin, T target, float duration) : base(origin, target, duration) { }
}

/// <summary>
/// 基本的反复变化
/// </summary>
public class Repeataion<T> : MyTimer<T> where T : struct
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Completed)
        {
            Timer -= Duration;
            Paused = false;
        }
    }
    public Repeataion(T origin, T target, float duration) : base(origin, target, duration) { }
}

