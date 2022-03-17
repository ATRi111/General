using UnityEngine;

/// <summary>
/// 打字机效果
/// </summary>
public class TypeWriter : MyTimer<string>
{
    /// <param name="text">文本</param>
    /// <param name="letterPerSecond">每秒打的字符数</param>
    /// <param name="start">是否启动计时器</param>
    public void Initialize(string text, float letterPerSecond, bool start = true)
    {
        base.Initialize("", text, text.Length / letterPerSecond, start);
    }

    /// <summary>
    /// 当前打出的文本
    /// </summary>
    public override string Current => Target.Substring(0, Mathf.FloorToInt(Percent * Target.Length));
}
