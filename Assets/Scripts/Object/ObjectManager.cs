using UnityEngine;

namespace ObjectPool
{
    public class ObjectManager : Service
    {
        [SerializeField]
        private ObjectDataDict dict;        //��ʼ�����������

        private ObjectPool[] cObjectPools;  //����صĽű�
        private int numOfObjects;           //����������������������

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
                //������Ϸ����
                GameObject obj_pool = new GameObject("Pool" + i.ToString());
                obj_pool.transform.parent = transform;
                //�ҽű�
                script_pool = obj_pool.AddComponent<ObjectPool>();
                script_pool.Initialize(data.Prefab, data.NumInPool);
                cObjectPools[i] = script_pool;
            }
        }

        /// <summary>
        /// ����һ����Ϸ���壬��������еĶ��������ֱ꣬�Ӵ���һ����Ϸ����
        /// </summary>
        /// <param name="eObject">Ҫ�������Ϸ�����Ӧ��ö��</param>
        /// <param name="position">λ��</param>
        /// <param name="angle">��ʾ����ĽǶ�</param>
        /// <returns>���������Ϸ����</returns>
        public CObject Activate(EObject eObject, Vector3 position, float angle = 0f)
        {
            CObject obj = cObjectPools[(int)eObject].Activate(position, angle);
            if (obj == null)
                obj = Instantiate(dict.GetObject(eObject).Prefab).GetComponent<CObject>();
            return obj;
        }
    }
}

