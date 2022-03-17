using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EUpdateMode
{
    /// <summary>
    /// 每次FixedUpdate调用
    /// </summary>
    FixedUpdate,
    /// <summary>
    /// 下次FixedUpdate调用
    /// </summary>
    NextFixedUpdate,
    /// <summary>
    /// 每次Update调用
    /// </summary>
    Update,
    /// <summary>
    /// 下次Update调用
    /// </summary>
    NextUpdate,
    /// <summary>
    /// 每次LateUpdate调用
    /// </summary>
    LateUpdate,
    /// <summary>
    /// 下次LateUpdate调用
    /// </summary>
    NextLateUpdate,
}

public class GameCycle : Service
{
    private Dictionary<EUpdateMode, UnityAction> cycle = new Dictionary<EUpdateMode, UnityAction>();

    protected override void Awake()
    {
        base.Awake();
        foreach (EUpdateMode e in System.Enum.GetValues(typeof(EUpdateMode))) 
        {
            cycle.Add(e, null);
        }
    }

    /// <summary>
    /// 用于将非Monobehavior方法加入游戏循环
    /// </summary>
    public void AttachToGameCycle(EUpdateMode mode, UnityAction callBack)
    {
        cycle[mode] += callBack;
    }

    public void RemoveFromGameCycle(EUpdateMode mode,UnityAction callBack)
    {
        cycle[mode] -= callBack;
    }

    private void Update()
    {
        cycle[EUpdateMode.Update]?.Invoke();
        cycle[EUpdateMode.NextUpdate]?.Invoke();
        cycle[EUpdateMode.NextUpdate] = null;
    }

    private void FixedUpdate()
    {
        cycle[EUpdateMode.FixedUpdate]?.Invoke();
        cycle[EUpdateMode.NextFixedUpdate]?.Invoke();
        cycle[EUpdateMode.NextFixedUpdate] = null;
    }

    private void LateUpdate()
    {
        cycle[EUpdateMode.LateUpdate]?.Invoke();
        cycle[EUpdateMode.NextLateUpdate]?.Invoke();
        cycle[EUpdateMode.NextLateUpdate] = null;
    }
}
