using UnityEngine;

/// <summary>
/// ��ObjectManager�������Ϸ�������̳д��ࣨ���������ࣩ
/// </summary>
public class MyObject : MonoBehaviour
{
    public bool b_createdInPool;   //�Ƿ��ɶ���ش���,��������ֱ�Ӵݻٶ����ǻ���

    [SerializeField]
    private bool _Active;
    public bool Active
    {
        get => _Active;
        private set
        {
            if (value == _Active) 
                return;
            _Active = value;
            gameObject.SetActive(value);
        }
    }

    protected Vector3 _EulerAngles;
    protected Vector3 EulerAngles
    {
        get => _EulerAngles;
        set
        {
            _EulerAngles = value;
            transform.eulerAngles = _EulerAngles;
        }
    }

    /// <summary>
    /// ��ֹ��д�˷�����������Ҫ��д��Initialize��
    /// </summary>
    protected void Awake()
    {
        return;
    }

    /// <summary>
    /// ������ش���ʱ����Ϊ
    /// </summary>
    internal virtual void Initialize()
    {
        _Active = true;
        Active = false;
    }

    internal void Activate(Vector3 pos, Vector3 eulerAngles)
    {
        b_createdInPool = true;
        Active = true;
        transform.position = pos;
        OnActivate();
        EulerAngles = eulerAngles;
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
        if (b_createdInPool)
        {
            Active = false;
            OnRecycle();
        }
        else
            Destroy(gameObject);
    }
    /// <summary>
    /// ������ʱ����Ϊ
    /// </summary>
    protected virtual void OnRecycle() { }
}
