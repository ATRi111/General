using System;
using UnityEngine;

public class InputManager : Service
{
    /// <summary>
    /// 检测是否连续按下了一串按键
    /// </summary>
    /// <param name="combo">按键序列</param>
    /// <param name="interval">按下两个按键间的最长时间间隔</param>
    public void StartComboCheck(KeyCode[] codes, float interval, Action<bool> callBack)
    {

    }
}
