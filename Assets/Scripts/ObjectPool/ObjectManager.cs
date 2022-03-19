using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectManager : Service
    {
        [SerializeField]
        private ObjectManagerData initData;

        private Dictionary<EObject, ObjectPool> objectPools; //对象池的脚本
        private int numOfPool;                              //对象池数

        private void Start()
        {
            Initialize();
        }

        internal void Initialize()
        {
            Initializer initializer = Initializer.Instance;
            ObjectPoolData[] datas = initData.datas;
            numOfPool = datas.Length;
            initializer.Count_Initializations += numOfPool;

            objectPools = new Dictionary<EObject, ObjectPool>();
            GameObject obj_pool;
            ObjectPool pool;
            ObjectPoolData data;
            for (int i = 0; i < numOfPool; i++)
            {
                data = datas[i];
                obj_pool = new GameObject($"Pool:{data.eObject}");
                obj_pool.transform.parent = transform;
                pool = obj_pool.AddComponent<ObjectPool>();
                pool.Initialize(data.prefab, data.size);
                objectPools.Add(data.eObject, pool);
            }
            initData = null;
        }

        /// <summary>
        /// 激活一个游戏物体，若对象池中的对象用完，创建一个对象并添加到对象池中，再激活
        /// </summary>
        /// <param name="eObject">要激活的游戏物体对应的枚举</param>
        /// <param name="position">位置</param>
        /// <param name="eulerAngles">欧拉角</param>
        /// <returns>被激活的游戏物体</returns>
        public MyObject Activate(EObject eObject, Vector3 position, Vector3 eulerAngles)
        {
            if (objectPools.ContainsKey(eObject))
            {
                MyObject obj = objectPools[eObject].Activate(position, eulerAngles);
                return obj;
            }
            Debug.LogWarning($"{eObject}没有对应的对象池");
            return null;
        }

        /// <summary>
        /// (用于2D游戏)激活一个游戏物体，若对象池中的对象用完，再创建一个对象并添加到对象池中
        /// </summary>
        /// <param name="eObject">要激活的游戏物体对应的枚举</param>
        /// <param name="position">位置</param>
        /// <param name="eulerAngleZ">z方向欧拉角</param>
        /// <returns>被激活的游戏物体</returns>
        public MyObject Activate(EObject eObject, Vector3 position, float eulerAngleZ = 0f)
        {
            return Activate(eObject, position, new Vector3(0f, 0f, eulerAngleZ));
        }
    }
}

