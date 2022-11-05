namespace Services
{
    /// <summary>
    /// ����ĳ�ֶ���Ϸ��Ĵ浵������ʹ��ʱ�½�����ʵ������Ӱ��ԭ���߼�
    /// </summary>
    /// <typeparam name="Tdata">��Ϸ�����Ӧ�Ĵ浵������</typeparam>
    /// <typeparam name="TObject">��Ϸ������</typeparam>
    public abstract class Savable<Tdata,TObject> where Tdata :SingleSaveData
    {
        protected SaveManagerBase saveManager;

        protected TObject obj;

        /// <summary>
        /// ��ʶ�����������ڹ��캯���оͻᱻ���ã����Բ�������obj����Ķ���
        /// </summary>
        public abstract string Identifier { get; }

        public Savable(TObject obj)
        {
            this.obj = obj;
            saveManager = ServiceLocator.Get<SaveManagerBase>();
            saveManager.AfterLoad += OnLoad;
            saveManager.BeforeSave += OnSave;
            if (saveManager.NeedLoad)
                OnLoad();
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

        public void Dispose()
        {
            saveManager.AfterLoad -= OnLoad;
            saveManager.BeforeSave -= OnSave;
        }
    }
}