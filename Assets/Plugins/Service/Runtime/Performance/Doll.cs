using UnityEngine;

namespace Services.Performance
{
    /// <summary>
    /// �����ݳ��м������Ϸ����,��Ϸ��ʼǰ�˽ű������Ǽ���״̬
    /// </summary>
    public class Doll : MonoBehaviour
    {
        protected IPerformanceManager manager;
        [SerializeField]
        protected EDollType dollType;

        protected void Awake()
        {
            manager = ServiceLocator.Get<IPerformanceManager>();
            enabled = false;
        }

        protected virtual bool OnUseDoll(EDollType type)
        {
            if (dollType == type && !enabled)
            {
                enabled = true;
                Use();
                return true;
            }
            return false;
        }

        protected virtual bool OnStopUseDoll(EDollType type)
        {
            if (dollType == type && enabled)
            {
                StopUse();
                enabled = false;
                return true;
            }
            return false;
        }

        protected virtual void Use()
        {

        }

        protected virtual void StopUse()
        {

        }
    }
}