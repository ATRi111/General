namespace Services.Save
{
    /// <summary>
    /// 控制某种对游戏象的存档读档，使用时新建此类实例，不影响原有逻辑
    /// </summary>
    /// <typeparam name="Tdata">游戏对象对应的存档数据类</typeparam>
    /// <typeparam name="TObject">游戏对象类</typeparam>
    public abstract class SaveController<Tdata, TObject> where Tdata : SingleSaveData
    {
        protected ISaveManager saveManager;

        protected TObject obj;

        private bool active;
        /// <summary>
        /// 是否启用存档/读档。
        /// 通常，物体被禁用或销毁后应当禁用
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
                        if (saveManager.NeedLoad)
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
        /// 标识符，此属性在构造函数中就会被调用，所以不能依赖obj以外的对象
        /// </summary>
        public abstract string Identifier { get; }

        public SaveController(TObject obj, bool active_init = true)
        {
            this.obj = obj;
            saveManager = ServiceLocator.Get<ISaveManager>();
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
    }
}