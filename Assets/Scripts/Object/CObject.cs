using UnityEngine;

/// <summary>
/// 由ObjectManager管理的游戏物体必须继承此类（或此类的子类）
/// </summary>
public class CObject : MonoBehaviour
{
    public bool b_createdInPool;   //是否由对象池创建,若不是则直接摧毁而不是回收

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

    protected float _Angle;
    protected float Angle
    {
        get => _Angle;
        set
        {
            _Angle = value;
            transform.eulerAngles = new Vector3(0, 0, 360f - value);
        }
    }

    /// <summary>
    /// 禁止重写此方法，如有需要，写在Initialize中
    /// </summary>
    protected void Awake()
    {
        return;
    }

    /// <summary>
    /// 被对象池创建时的行为
    /// </summary>
    internal virtual void Initialize()
    {
        _Active = true;
        Active = false;
    }

    internal void Activate(Vector3 pos, float angle)
    {
        b_createdInPool = true;
        Active = true;
        transform.position = pos;
        Angle = angle;
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
        if (b_createdInPool)
        {
            Active = false;
            OnRecycle();
        }
        else
            Destroy(gameObject);
    }
    /// <summary>
    /// 被回收时的行为
    /// </summary>
    protected virtual void OnRecycle() { }
}
