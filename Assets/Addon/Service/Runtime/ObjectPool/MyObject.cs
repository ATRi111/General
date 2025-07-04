using UnityEngine;
using UnityEngine.Events;

namespace Services.ObjectPools
{
    /// <summary>
    /// 使用时，直接添加此组件（而不是继承）
    /// </summary>
    public sealed class MyObject : MonoBehaviour, IMyObject
    {
        public event UnityAction OnRecycle;
        public event UnityAction OnActivate;

        internal bool b_createdByPool;
        internal ObjectPool objectPoolAttached;

        public Transform Transform => transform;

        [SerializeField]
        internal bool active;
        public bool Active
        {
            get => active;
            internal set
            {
                if (value == active)
                    return;
                active = value;
                gameObject.SetActive(value);
            }
        }

        [SerializeField]
        internal int recycleCount;
        /// <summary>
        /// 此计数变为0时，回收物体（可以不使用此计数，而是手动回收）
        /// </summary>
        public int RecycleCount
        {
            get => recycleCount;
            set
            {
                recycleCount = value;
                if (value == 0)
                    Recycle();
            }
        }

        /// <summary>
        /// 激活物体
        /// </summary>
        public void Activate(Vector3 pos, Vector3 eulerAngles, Transform parent = null)
        {
            if (parent != null)
                transform.SetParent(parent);
            transform.position = pos;
            transform.eulerAngles = eulerAngles;
            Active = true;
            OnActivate?.Invoke();
        }
        /// <summary>
        /// 回收物体，如果不是由对象池创建，改为销毁物体
        /// </summary>
        public void Recycle()
        {
            if (this == null)   //避免关闭程序时报错
                return;

            if (b_createdByPool && objectPoolAttached != null)
            {
                transform.SetParent(objectPoolAttached.transform, false);
                OnRecycle?.Invoke();
                if (recycleCount != 0)
                {
                    Debug.LogWarning($"{gameObject.name}被回收时，引用计数为{recycleCount}");
                    recycleCount = 0;
                }
                Active = false;
                objectPoolAttached.Recycle(this);
            }
            else
                Destroy(gameObject);
        }
    }
}
