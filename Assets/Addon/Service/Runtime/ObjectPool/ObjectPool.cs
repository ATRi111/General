using System.Collections.Generic;
using UnityEngine;

namespace Services.ObjectPools
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;
        private Queue<IMyObject> myObjects;  //对象上的脚本
        public int Count => myObjects.Count;
        [SerializeField]
        private int accumulateCount;

        internal void Initialize(GameObject prefab)
        {
            if (prefab.GetComponent<IMyObject>() == null)
            {
                Debugger.LogError("对象池中的物体未实现IMyObject", EMessageType.System);
                return;
            }
            this.prefab = prefab;
            myObjects = new Queue<IMyObject>();
        }

        internal IMyObject Activate(Vector3 position, Vector3 eulerAngles, Transform parent = null)
        {
            if (myObjects.Count == 0)
                Create();

            IMyObject ret = myObjects.Dequeue();
            ret.Activate(position, eulerAngles, parent);
            return ret;
        }

        internal void Recycle(MyObject myObject)
        {
            myObjects.Enqueue(myObject);
        }

        internal void Create(int count = 1)
        {
            accumulateCount += count;
            if (count > 100)
                Debugger.LogWarning("大量的预生成应分散到多帧执行", EMessageType.System);
            for (int i = 0; i < count; i++)
            {
                IMyObject newObject = ObjectPoolUtility.Clone(prefab, true, this);
                newObject.Transform.parent = transform;
                myObjects.Enqueue(newObject);
            }
        }
    }
}