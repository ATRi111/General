using UnityEngine;

namespace ObjectPool
{
    /// <summary>
    /// 由ObjectManager管理的游戏物体必须继承此类（或此类的子类）,此类脱离对象池也可以使用
    /// </summary>
    public class MyObject : MonoBehaviour
    {
        /// <summary>
        /// 用此方法生成MyObject挂载的游戏物体，而不是Object.Instantiate
        /// </summary>
        /// <param name="prefab">要克隆的游戏物体</param>
        /// <param name="byPool">是否由对象池生成</param>
        /// <param name="pool">所属的对象池</param>
        /// <returns>生成的游戏物体的MyObject脚本</returns>
        public static MyObject Create(GameObject prefab, bool byPool = false, ObjectPool pool = null)
        {
            GameObject obj = Instantiate(prefab);
            MyObject myObject = obj.GetComponent<MyObject>();
            myObject.b_createdByPool = byPool;
            if (byPool)
            {
                if (pool == null)
                {
                    Debug.LogWarning("未分配对象池");
                }
                else
                {
                    myObject._Active = true;
                    myObject.Active = false;
                    myObject.objectPoolAttached = pool;
                }
            }
            myObject.OnCreate();
            return myObject;
        }

        protected bool b_createdByPool;
        protected ObjectPool objectPoolAttached;

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
                OnRecycle();
                Active = false;
                objectPoolAttached.Recycle(this);
            }
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// 被创建时的行为
        /// </summary>
        public virtual void OnCreate() { }
        /// <summary>
        /// 被激活时的行为
        /// </summary>
        protected virtual void OnActivate() { }
        /// <summary>
        /// 被回收时的行为
        /// </summary>
        protected virtual void OnRecycle() { }



        //禁止重写以下方法
        protected void Awake() { return; }
        protected void Start() { return; }
        protected void OnEnable() { return; }
        private void OnDisable() { return; }
    }
}
