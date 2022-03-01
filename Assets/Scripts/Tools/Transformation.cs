using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 描述一段随时间推进发生的变化
/// </summary>
public class Transformation<T>
{
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
    public virtual Color Current => default;
    public bool Completed => Timer >= Duration;
    public Transformation() { }
    public Transformation(float duration, T origin, T target)
    {
        Duration = duration;
        Origin = origin;
        Target = target;
    }
    protected virtual void OnFixedUpdate()
    {
        Timer += Time.fixedDeltaTime;
    }
}

public class ColorChange : Transformation<Color>
{
    public override Color Current => Color.Lerp(Origin, Target, Timer / Duration);
    public ColorChange() { }
    public ColorChange(float duration, Color origin, Color target) : base(duration, origin, target) { }
}