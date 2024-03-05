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

        /// <summary>
        /// 预生成
        /// </summary>
        internal void Create(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                IMyObject newObject = ObjectPoolUtility.Clone(prefab, true, this);
                newObject.Transform.SetParent(transform);
                myObjects.Enqueue(newObject);
            }
        }
    }
}