using UnityEngine;

/// <summary>
/// ��ObjectManager�������Ϸ�������̳д��ࣨ���������ࣩ
/// </summary>
public class MyObject : MonoBehaviour
{
    [SerializeField]
    private bool _Active;
    public bool Active
    {
        get => _Active;
        protected set
        {
            if (value == _Active)
                return;
            _Active = value;
            gameObject.SetActive(value);
        }
    }

    protected Vector3 _EulerAngles;
    public Vector3 EulerAngles
    {
        get => _EulerAngles;
        protected set
        {
            _EulerAngles = value;
            transform.eulerAngles = _EulerAngles;
        }
    }

    /// <summary>
    /// ��ֹ��д�˷�����������Ҫ��д��Initialize��
    /// </summary>
    protected void Awake() { }

    /// <summary>
    /// ������ش���ʱ����Ϊ��Ĭ�Ͻ��������
    /// </summary>
    internal virtual void Initialize()
    {
        _Active = true;
        Active = false;
    }

    internal void Activate(Vector3 pos, Vector3 eulerAngles)
    {
        Active = true;
        transform.position = pos;
        EulerAngles = eulerAngles;
        OnActivate();
    }
    /// <summary>
    /// ���屻����ʱ����Ϊ
    /// </summary>
    protected virtual void OnActivate() { }
    /// <summary>
    /// ����������ɶ���ش�����������գ�������ݻ�
    /// </summary>
    public void Recycle()
    {
        Active = false;
        OnRecycle();
    }
    /// <summary>
    /// ������ʱ����Ϊ
    /// </summary>
    protected virtual void OnRecycle() { }
}
