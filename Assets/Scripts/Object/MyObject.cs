using UnityEngine;

/// <summary>
/// 由ObjectManager管理的游戏物体必须继承此类（或此类的子类）
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
    /// 禁止重写此方法，如有需要，写在Initialize中
    /// </summary>
    protected void Awake() { }

    /// <summary>
    /// 被对象池创建时的行为，默认将对象禁用
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
    /// 物体被激活时的行为
    /// </summary>
    protected virtual void OnActivate() { }
    /// <summary>
    /// 如果该物体由对象池创建，将其回收，否则将其摧毁
    /// </summary>
    public void Recycle()
    {
        Active = false;
        OnRecycle();
    }
    /// <summary>
    /// 被回收时的行为
    /// </summary>
    protected virtual void OnRecycle() { }
}
