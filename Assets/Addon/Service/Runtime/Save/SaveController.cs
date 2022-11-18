namespace Services
{
    /// <summary>
    /// ����ĳ�ֶ���Ϸ��Ĵ浵������ʹ��ʱ�½�����ʵ������Ӱ��ԭ���߼�
    /// </summary>
    /// <typeparam name="Tdata">��Ϸ�����Ӧ�Ĵ浵������</typeparam>
    /// <typeparam name="TObject">��Ϸ������</typeparam>
    public abstract class SaveController<Tdata, TObject> where Tdata : SingleSaveData
    {
        protected SaveManagerBase saveManager;

        protected TObject obj;

        private bool active;
        /// <summary>
        /// �Ƿ����ô浵/������
        /// ͨ�������屻���û����ٺ�Ӧ������
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
                        saveManager.LoadRequest += OnLoad;
                        saveManager.SaveRequest += OnSave;
                        if (saveManager.NeedLoad)
                            OnLoad();
                    }
                    else
                    {
                        saveManager.LoadRequest -= OnLoad;
                        saveManager.SaveRequest -= OnSave;
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
            saveManager = ServiceLocator.Get<SaveManagerBase>();
            if (active_init)
                Active = true;
        }

        protected virtual void OnLoad()
        {
            FromData(FindSelf(saveManager.RuntimeData));
        }

        protected virtual void OnSave()
        {
            ToData(FindSelf(saveManager.RuntimeData));
        }

        /// <summary>
        /// �������浵�������ҵ����������
        /// </summary>
        protected abstract Tdata FindSelf(WholeSaveData whole);

        /// <summary>
        /// ͨ�����������ʱ�浵�����޸����������
        /// </summary>
        public abstract void FromData(Tdata data);

        /// <summary>
        /// ͨ������������޸����������ʱ�浵����
        /// </summary>
        public abstract void ToData(Tdata data);
    }
}