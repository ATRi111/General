using UnityEngine;

namespace ObjectPool
{
    public class ObjectManager : Service
    {
        [SerializeField]
        private ObjectDataDict dict;        //初始化所需的数据

        private ObjectPool[] cObjectPools;  //对象池的脚本
        private int numOfObjects;           //对象的种类数，即对象池数

        protected override void Awake()
        {
            eService = EService.ObjectManager;
            base.Awake();
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (dict == null || dict.NumOfObjects == 0)
            {
                Initializer initializer = Initializer.Instance;
                initializer.NumOfLoadObject++;
                initializer.NumOfLoadObject--;
                return;
            }
            numOfObjects = dict.NumOfObjects;

            cObjectPools = new ObjectPool[numOfObjects];
            ObjectPool script_pool;
            ObjectData data;
            for (int i = 0; i < numOfObjects; i++)
            {
                data = dict.GetObject((EObject)i);
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
        /// <param name="angle">表示朝向的角度</param>
        /// <returns>被激活的游戏物体</returns>
        public CObject Activate(EObject eObject, Vector3 position, float angle = 0f)
        {
            CObject obj = cObjectPools[(int)eObject].Activate(position, angle);
            if (obj == null)
                obj = Instantiate(dict.GetObject(eObject).Prefab).GetComponent<CObject>();
            return obj;
        }
    }
}

