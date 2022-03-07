using UnityEngine;
using System;

/// <summary>
/// 描述一段随时间推进发生的变化
/// </summary>
public class Transformation<T>
{
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
    public bool Completed => Timer >= Duration;
    public Transformation() { }
    public Transformation(T origin, T target, float duration)
    {
        Duration = duration;
        Origin = origin;
        Target = target;
    }
    //创建类实例后，在fiexedupdate中调用此方法
    public virtual void OnFixedUpdate()
    {
        if (Paused)
            return;
        Timer += Time.fixedDeltaTime;
        if (Completed)
            Paused = true;
    }

    public virtual void Restart()
    {
        Paused = false;
        Timer = 0;
    }
}

public class ColorChange : Transformation<Color>
{
    public override Color Current => Color.Lerp(Origin, Target, Timer / Duration);
    public ColorChange() { }
    public ColorChange(Color origin, Color target, float duration) : base(origin, target, duration) { }
}

public class BezierCurve : Transformation<Vector3>
{
    private readonly Vector3[] Points;
    public BezierCurve(Vector3[] points, float duration)
    {
        int count = points.Length;
        if (count < 2)
            throw new ArgumentException();
        Origin = points[0];
        Target = points[count - 1];
        Points = points;
        Duration = duration;
    }

    public override Vector3 Current => MathTool.BezierLerp(Points, Timer / Duration);
}