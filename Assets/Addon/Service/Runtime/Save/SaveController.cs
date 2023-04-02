namespace Services.Save
{
    /// <summary>
    /// ���Կ��Ƶ�����Ϸ��������ݵĴ�ȡ;
    /// Ҫʹ�ô��࣬����ΪҪ���Ƶ���Ϸ�������Ӧ�Ĵ浵�����࣬Ȼ������ʱ����һ�������ʵ��
    /// </summary>
    /// <typeparam name="Tdata">��Ϸ�����Ӧ�Ĵ浵������</typeparam>
    /// <typeparam name="TObject">��Ϸ������</typeparam>
    public abstract class SaveController<Tdata, TObject> where Tdata : SingleSaveData
    {
        protected ISaveManager saveManager;

        protected TObject obj;

        public bool NeedLoad => saveManager.NeedLoad;

        private bool active;
        /// <summary>
        /// �Ƿ����ô浵/������
        /// ͨ�������屻���û�����ǰӦ������
        /// </summary>
        public bool Active
        {
            get => active;
            set
            {
                if (active != value)
                {
                    active = value;
                    if (value)
                    {
                        saveManager.LoadRequest.AddListener(OnLoad);
                        saveManager.SaveRequest.AddListener(OnSave);
                        if (NeedLoad)
                            OnLoad();
                    }
                    else
                    {
                        saveManager.LoadRequest.RemoveListener(OnLoad);
                        saveManager.SaveRequest.RemoveListener(OnSave);
                    }
                }
            }
        }

        /// <summary>
        /// ��ʶ�����������ڹ��캯���оͻᱻ���ã����Բ�������obj����Ķ���
        /// </summary>
        public abstract string Identifier { get; }

        public SaveController(TObject obj, bool active_init = true)
        {
            this.obj = obj;
            saveManager = ServiceLocator.Get<ISaveManager>();
            if (active_init)
                Active = true;
        }

        /// <summary>
        /// ����ʱ����Ϊ
        /// </summary>
        protected virtual void OnLoad()
        {
            FromData(FindSelf(saveManager.RuntimeData));
        }

        /// <summary>
        /// �浵ʱ����Ϊ
        /// </summary>
        protected virtual void OnSave()
        {
            ToData(FindSelf(saveManager.RuntimeData));
        }

        /// <summary>
        /// �ڴ˷����й涨��δ������浵�������ҵ����������
        /// </summary>
        protected abstract Tdata FindSelf(WholeSaveData whole);

        /// <summary>
        /// �ڴ˷����ж���浵���������������Ϸ����
        /// </summary>
        public abstract void FromData(Tdata data);

        /// <summary>
        /// �ڴ˷����ж����������Ϸ����ת��Ϊ�浵����
        /// </summary>
        public abstract void ToData(Tdata data);
    }
}