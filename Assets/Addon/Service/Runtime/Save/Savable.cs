namespace Services
{
    /// <summary>
    /// 控制某种对游戏象的存档读档，使用时新建此类实例，不影响原有逻辑
    /// </summary>
    /// <typeparam name="Tdata">游戏对象对应的存档数据类</typeparam>
    /// <typeparam name="TObject">游戏对象类</typeparam>
    public abstract class Savable<Tdata,TObject> where Tdata :SingleSaveData
    {
        protected SaveManagerBase saveManager;

        protected TObject obj;

        /// <summary>
        /// 标识符，此属性在构造函数中就会被调用，所以不能依赖obj以外的对象
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
        /// 从整个存档数据中找到自身的数据
        /// </summary>
        protected abstract Tdata FindSelf(WholeSaveData whole);

        /// <summary>
        /// 通过自身的运行时存档数据修改自身的数据
        /// </summary>
        public abstract void FromData(Tdata data);

        /// <summary>
        /// 通过自身的数据修改自身的运行时存档数据
        /// </summary>
        public abstract void ToData(Tdata data);

        public void Dispose()
        {
            saveManager.AfterLoad -= OnLoad;
            saveManager.BeforeSave -= OnSave;
        }
    }
}