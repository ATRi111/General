using UnityEngine;

namespace ObjectPool
{
    public class ObjectManager : Service
    {
        [SerializeField]
        private ObjectDataDict odd;         //���л�ȡ���������

        private ObjectPool[] cObjectPools;  //����صĽű�
        private int numOfObjects;           //����������������������

        private void Start()
        {
            Initialize();
        }

        internal void Initialize()
        {
            if (odd == null || odd.NumOfObjects == 0)
            {
                Initializer initializer = Initializer.Instance;
                initializer.Count_Initializations++;
                initializer.Count_Initializations--;
                return;
            }
            numOfObjects = odd.NumOfObjects;

            cObjectPools = new ObjectPool[numOfObjects];
            ObjectPool script_pool;
            ObjectData data;
            for (int i = 0; i < numOfObjects; i++)
            {
                data = odd.GetObject((EObject)i);
                //���������
                GameObject obj_pool = new GameObject("Pool" + i.ToString());
                obj_pool.transform.parent = transform;
                //�ڶ�����Ϲҽű�
                script_pool = obj_pool.AddComponent<ObjectPool>();
                script_pool.Initialize(data.Prefab, data.Num);
                cObjectPools[i] = script_pool;
            }
        }

        /// <summary>
        /// ����һ����Ϸ���壬��������еĶ��󼸺����꣬����һ��������ӵ�������У��ټ���
        /// </summary>
        /// <param name="eObject">Ҫ�������Ϸ�����Ӧ��ö��</param>
        /// <param name="position">λ��</param>
        /// <param name="eulerAngles">ŷ����</param>
        /// <returns>���������Ϸ����</returns>
        public MyObject Activate(EObject eObject, Vector3 position, Vector3 eulerAngles)
        {
            MyObject obj = cObjectPools[(int)eObject].Activate(position, eulerAngles);
            return obj;
        }

        /// <summary>
        /// (����2D��Ϸ)����һ����Ϸ���壬��������еĶ��������꣬�ٴ���һ��������ӵ��������
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

