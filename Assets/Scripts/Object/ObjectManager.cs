using UnityEngine;

namespace ObjectPool
{
    public class ObjectManager : Service
    {
        [SerializeField]
        private ObjectDataDict odd;         //从中获取所需的数据

        private ObjectPool[] cObjectPools;  //对象池的脚本
        private int numOfObjects;           //对象的种类数，即对象池数

        protected override void BeforeRegister()
        {
            eService = EService.ObjectManager;
        }

        private void Start()
        {
            Initialize();
        }

        internal void Initialize()
        {
            if (odd == null || odd.NumOfObjects == 0)
            {
                Initializer initializer = Initializer.Instance;
                initializer.NumOfLoadObject++;
                initializer.NumOfLoadObject--;
                return;
            }
            numOfObjects = odd.NumOfObjects;

            cObjectPools = new ObjectPool[numOfObjects];
            ObjectPool script_pool;
            ObjectData data;
            for (int i = 0; i < numOfObjects; i++)
            {
                data = odd.GetObject((EObject)i);
                //创建游戏物体
                GameObject obj_pool = new GameObject("Pool" + i.ToString());
                obj_pool.transform.parent = transform;
                //挂脚本
                script_pool = obj_pool.AddComponent<ObjectPool>();
                script_pool.Initialize(data.Prefab, data.NumInPool);
                cObjectPools[i] = script_pool;
            }
        }

        /// <summary>
        /// 激活一个游戏物体，若对象池中的对象已用完，直接创建一个游戏物体
        /// </summary>
        /// <param name="eObject">要激活的游戏物体对应的枚举</param>
        /// <param name="position">位置</param>
        /// <param name="eulerAngles">欧拉角</param>
        /// <returns>被激活的游戏物体</returns>
        public CObject Activate(EObject eObject, Vector3 position, Vector3 eulerAngles)
        {
            CObject obj = cObjectPools[(int)eObject].Activate(position, eulerAngles); 
            if (obj == null)
                obj = Instantiate(odd.GetObject(eObject).Prefab).GetComponent<CObject>();
            return obj;
        }

        /// <summary>
        /// 激活一个2D游戏物体，若对象池中的对象已用完，直接创建一个游戏物体
        /// </summary>
        /// <param name="eObject">要激活的游戏物体对应的枚举</param>
        /// <param name="position">位置</param>
        /// <param name="eulerAngleZ">z方向欧拉角</param>
        /// <returns>被激活的游戏物体</returns>
        public CObject Activate(EObject eObject, Vector3 position, float eulerAngleZ = 0f)
        {
            return Activate(eObject,position, new Vector3(0f, 0f, eulerAngleZ));
        }
    }
}

