using UnityEngine;

namespace ObjectPool
{
    /// <summary>
    /// 由ObjectManager管理的游戏物体必须继承此类（或此类的子类）,此类脱离对象池也可以使用
    /// </summary>
    public class MyObject : MonoBehaviour
    {
        protected internal bool b_createdByPool;

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

        /// <summary>
        /// 由对象池创建时才能调用此方法，直接创建时应该调用OnCreate
        /// </summary>
        protected internal void CreateByPool()
        {
            b_createdByPool = true;
            _Active = true;
            Active = false;
            OnCreate();
        }
        /// <summary>
        /// 激活物体
        /// </summary>
        protected internal void Activate(Vector3 pos, Vector3 eulerAngles)
        {
            Active = true;
            transform.position = pos;
            transform.eulerAngles = eulerAngles;
            OnActivate();
        }
        /// <summary>
        /// 回收物体，如果不是由对象池创建，改为销毁物体
        /// </summary>
        public void Recycle()
        {
            if (b_createdByPool)
            {
                Active = false;
                OnRecycle();
            }
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// 物体首次被创建时的行为
        /// </summary>
        public virtual void OnCreate() { }
        /// <summary>
        /// 物体被激活时的行为
        /// </summary>
        protected virtual void OnActivate() { }
        /// <summary>
        /// 被回收或销毁时的行为
        /// </summary>
        protected virtual void OnRecycle() { }


        //禁止重写以下方法
        protected void Awake() { }
        protected void Start() { }
        protected void OnEnable() { }
        private void OnDisable() { }
    }
}
