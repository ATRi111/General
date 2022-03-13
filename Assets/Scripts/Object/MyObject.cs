using UnityEngine;

namespace ObjectPool
{
    /// <summary>
    /// ��ObjectManager�������Ϸ�������̳д��ࣨ���������ࣩ,������������Ҳ����ʹ��
    /// </summary>
    public class MyObject : MonoBehaviour
    {
        protected internal bool b_createdByPool;

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
        /// �ɶ���ش���ʱ���ܵ��ô˷�����ֱ�Ӵ���ʱӦ�õ���OnCreate
        /// </summary>
        protected internal void CreateByPool()
        {
            b_createdByPool = true;
            _Active = true;
            Active = false;
            OnCreate();
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
                Active = false;
                OnRecycle();
            }
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// �����״α�����ʱ����Ϊ
        /// </summary>
        public virtual void OnCreate() { }
        /// <summary>
        /// ���屻����ʱ����Ϊ
        /// </summary>
        protected virtual void OnActivate() { }
        /// <summary>
        /// �����ջ�����ʱ����Ϊ
        /// </summary>
        protected virtual void OnRecycle() { }


        //��ֹ��д���·���
        protected void Awake() { }
        protected void Start() { }
        protected void OnEnable() { }
        private void OnDisable() { }
    }
}
