using UnityEngine;

/// <summary>
/// ����һ����ʱ���ƽ������ı仯
/// </summary>
public class MyTimer<T> where T : struct
{
    private readonly GameCycle gameCycle;
    /// <summary>
    /// ��ͣʱ��OnFixedUpdate����Ϊ
    /// </summary>
    public bool Paused { get; set; }
    /// <summary>
    /// ������ʱ��
    /// </summary>
    public float Timer { get; set; }
    /// <summary>
    /// ��ʱ��
    /// </summary>
    public float Duration { get; protected set; }
    /// <summary>
    /// ��ֵ
    /// </summary>
    public T Origin { get; protected set; }
    /// <summary>
    /// ��ֵ
    /// </summary>
    public T Target { get; protected set; }
    /// <summary>
    /// ��ǰֵ
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
    /// ����ֹͣ��֮��һ��Ҫ����
    /// </summary>
    public void Dispose()
    {
        gameCycle.RemoveFromGameCycle(EUpdateMode.Update, OnUpdate);
    }

}

/// <summary>
/// �����������仯
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
/// �����ķ����仯
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

