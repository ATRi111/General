using UnityEngine;

namespace ObjectPool
{
    /// <summary>
    /// ��ObjectManager�������Ϸ�������̳д��ࣨ���������ࣩ,������������Ҳ����ʹ��
    /// </summary>
    public class MyObject : MonoBehaviour
    {
        /// <summary>
        /// �ô˷�������MyObject���ص���Ϸ���壬������Object.Instantiate
        /// </summary>
        /// <param name="prefab">Ҫ��¡����Ϸ����</param>
        /// <param name="byPool">�Ƿ��ɶ��������</param>
        /// <param name="pool">�����Ķ����</param>
        /// <returns>���ɵ���Ϸ�����MyObject�ű�</returns>
        public static MyObject Create(GameObject prefab, bool byPool = false, ObjectPool pool = null)
        {
            GameObject obj = Instantiate(prefab);
            MyObject myObject = obj.GetComponent<MyObject>();
            myObject.b_createdByPool = byPool;
            if (byPool)
            {
                if (pool == null)
                {
                    Debug.LogWarning("δ��������");
                }
                else
                {
                    myObject._Active = true;
                    myObject.Active = false;
                    myObject.objectPoolAttached = pool;
                }
            }
            myObject.OnCreate();
            return myObject;
        }

        protected bool b_createdByPool;
        protected ObjectPool objectPoolAttached;

        [SerializeField]
        private bool _Active;
        public bool Active
        {
            get => _Active;
            protected set
            {
                if (value == _Active)
                    return;
                _Active = value;
                gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        protected internal void Activate(Vector3 pos, Vector3 eulerAngles)
        {
            Active = true;
            transform.position = pos;
            transform.eulerAngles = eulerAngles;
            OnActivate();
        }
        /// <summary>
        /// �������壬��������ɶ���ش�������Ϊ��������
        /// </summary>
        public void Recycle()
        {
            if (b_createdByPool)
            {
                OnRecycle();
                Active = false;
                objectPoolAttached.Recycle(this);
            }
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// ������ʱ����Ϊ
        /// </summary>
        public virtual void OnCreate() { }
        /// <summary>
        /// ������ʱ����Ϊ
        /// </summary>
        protected virtual void OnActivate() { }
        /// <summary>
        /// ������ʱ����Ϊ
        /// </summary>
        protected virtual void OnRecycle() { }



        //��ֹ��д���·���
        protected void Awake() { return; }
        protected void Start() { return; }
        protected void OnEnable() { return; }
        private void OnDisable() { return; }
    }
}
