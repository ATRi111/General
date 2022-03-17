using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EUpdateMode
{
    /// <summary>
    /// ÿ��FixedUpdate����
    /// </summary>
    FixedUpdate,
    /// <summary>
    /// �´�FixedUpdate����
    /// </summary>
    NextFixedUpdate,
    /// <summary>
    /// ÿ��Update����
    /// </summary>
    Update,
    /// <summary>
    /// �´�Update����
    /// </summary>
    NextUpdate,
    /// <summary>
    /// ÿ��LateUpdate����
    /// </summary>
    LateUpdate,
    /// <summary>
    /// �´�LateUpdate����
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
    /// ���ڽ���Monobehavior����������Ϸѭ��
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
