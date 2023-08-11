using UnityEngine;
using UnityEngine.Events;

namespace Tools
{
    /// <summary>
    /// 条件量；某个事件需要同时满足多个条件才能执行时，可以使用此类
    /// </summary>
    [System.Serializable]
    public class ConditionValue
    {
        [SerializeField]
        private int count;

        /// <summary>
        /// 尚未满足的条件数量；如果为0，表示满足所有条件
        /// </summary>
        public int Count => count;

        /// <summary>
        /// 信号量小于等于0即表示“上锁”
        /// </summary>
        public bool Locked => Count <= 0;

        /// <summary>
        /// 条件从不全满足变为全满足时，调用此事件
        /// </summary>
        public event UnityAction AfterSatisfied;
        /// <summary>
        /// 条件从全满足变为不全满足时，调用此事件
        /// </summary>
        public event UnityAction AfterNotSatisfied;

        /// <summary>
        /// 设定初值,不会引发AfterSatisfied或AfterNotSatisfied事件
        /// </summary>
        public ConditionValue(int init = 0)
        {
            count = init;
        }

        public static ConditionValue operator ++(ConditionValue semaphore)
        {
            semaphore.count++;
            if (semaphore.count == 1)
                semaphore.AfterNotSatisfied?.Invoke();
            return semaphore;
        }

        public static ConditionValue operator --(ConditionValue semaphore)
        {
            semaphore.count--;
            if (semaphore.count == 0)
                semaphore.AfterSatisfied?.Invoke();
            else if (semaphore.count < 0)
                Debug.LogWarning("任何情况下,信号量不应降低到0以下");
            return semaphore;
        }
    }
}