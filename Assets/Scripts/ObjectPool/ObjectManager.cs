using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectManager : Service
    {
        [SerializeField]
        private ObjectManagerData initData;

        private Dictionary<EObject, ObjectPool> objectPools; //����صĽű�
        private int numOfPool;                              //�������

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
        /// ����һ����Ϸ���壬��������еĶ������꣬����һ��������ӵ�������У��ټ���
        /// </summary>
        /// <param name="eObject">Ҫ�������Ϸ�����Ӧ��ö��</param>
        /// <param name="position">λ��</param>
        /// <param name="eulerAngles">ŷ����</param>
        /// <returns>���������Ϸ����</returns>
        public MyObject Activate(EObject eObject, Vector3 position, Vector3 eulerAngles)
        {
            if (objectPools.ContainsKey(eObject))
            {
                MyObject obj = objectPools[eObject].Activate(position, eulerAngles);
                return obj;
            }
            Debug.LogWarning($"{eObject}û�ж�Ӧ�Ķ����");
            return null;
        }

        /// <summary>
        /// (����2D��Ϸ)����һ����Ϸ���壬��������еĶ������꣬�ٴ���һ��������ӵ��������
        /// </summary>
        /// <param name="eObject">Ҫ�������Ϸ�����Ӧ��ö��</param>
        /// <param name="position">λ��</param>
        /// <param name="eulerAngleZ">z����ŷ����</param>
        /// <returns>���������Ϸ����</returns>
        public MyObject Activate(EObject eObject, Vector3 position, float eulerAngleZ = 0f)
        {
            return Activate(eObject, position, new Vector3(0f, 0f, eulerAngleZ));
        }
    }
}

