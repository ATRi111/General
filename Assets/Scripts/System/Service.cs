using UnityEngine;

/// <summary>
/// ���񣬴ӷ���λ����ȡ
/// </summary>
public abstract class Service : MonoBehaviour
{
    /// <summary>
    /// �ٽű�˳���ܿ��Ƶ�����£���Ҫ��Awake�л�ȡ��������
    /// </summary>
    protected virtual void Awake()
    {
        ServiceLocator.Register(this);
    }
}
