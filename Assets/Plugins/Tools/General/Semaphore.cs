using UnityEngine;
using UnityEngine.Events;

namespace Tools
{
    [System.Serializable]
    public class Semaphore
    {
        [SerializeField]
        private int value;

        public bool UnLocked => value <= 0;

        public event UnityAction Lock;
        public event UnityAction UnLock;

        /// <param name="init">初值，设定初值不会引发Lock或UnLock事件</param>
        public Semaphore(int init = 0)
        {
            value = init;
        }

        public static Semaphore operator ++(Semaphore semaphore)
        {
            semaphore.value++;
            if (semaphore.value == 1)
                semaphore.Lock?.Invoke();
            return semaphore;
        }

        public static Semaphore operator --(Semaphore semaphore)
        {
            semaphore.value--;
            if (semaphore.value == 0)
                semaphore.UnLock?.Invoke();
            else if (semaphore.value < 0)
                Debug.LogWarning("信号量不应降低到0以下");
            return semaphore;
        }
    }
}