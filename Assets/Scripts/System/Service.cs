using UnityEngine;

/// <summary>
/// ���񣬴ӷ���λ����ȡ
/// </summary>
public abstract class Service : MonoBehaviour
{
    /// <summary>
    /// ���Ҫ��Awake�л�ȡ��������ע��ű�ִ��˳��
    /// </summary>
    protected virtual void Awake()
    {
        ServiceLocator.Register(this);
    }
}
