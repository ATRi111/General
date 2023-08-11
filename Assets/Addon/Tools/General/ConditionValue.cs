using UnityEngine;
using UnityEngine.Events;

namespace Tools
{
    /// <summary>
    /// ��������ĳ���¼���Ҫͬʱ��������������ִ��ʱ������ʹ�ô���
    /// </summary>
    [System.Serializable]
    public class ConditionValue
    {
        [SerializeField]
        private int count;

        /// <summary>
        /// ��δ������������������Ϊ0����ʾ������������
        /// </summary>
        public int Count => count;

        /// <summary>
        /// �ź���С�ڵ���0����ʾ��������
        /// </summary>
        public bool Locked => Count <= 0;

        /// <summary>
        /// �����Ӳ�ȫ�����Ϊȫ����ʱ�����ô��¼�
        /// </summary>
        public event UnityAction AfterSatisfied;
        /// <summary>
        /// ������ȫ�����Ϊ��ȫ����ʱ�����ô��¼�
        /// </summary>
        public event UnityAction AfterNotSatisfied;

        /// <summary>
        /// �趨��ֵ,��������AfterSatisfied��AfterNotSatisfied�¼�
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
                Debug.LogWarning("�κ������,�ź�����Ӧ���͵�0����");
            return semaphore;
        }
    }
}