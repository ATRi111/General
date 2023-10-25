namespace Services.Save
{
    /// <summary>
    /// 可以控制单个游戏对象的数据的存取;
    /// 要使用此类，首先为要控制的游戏对象定义对应的存档数据类，然后运行时创建一个此类的实例
    /// </summary>
    /// <typeparam name="Tdata">游戏对象对应的存档数据类</typeparam>
    /// <typeparam name="TObject">游戏对象类</typeparam>
    public abstract class SaveController<Tdata, TObject> where Tdata : SingleSaveData
    {
        protected ISaveManager saveManager;

        protected TObject obj;

        public bool NeedLoad => saveManager.NeedLoad;

        private bool active;
        /// <summary>
        /// 是否启用存档/读档。
        /// 通常，物体被禁用或销毁前应当禁用
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
                        saveManager.AfterLoadRequest.AddListener(OnLoad);
                        saveManager.AfterSaveRequest.AddListener(OnSave);
                        if (NeedLoad)
                            OnLoad();
                    }
                    else
                    {
                        saveManager.AfterLoadRequest.RemoveListener(OnLoad);
                        saveManager.AfterSaveRequest.RemoveListener(OnSave);
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

        /// <summary>
        /// 读档时的行为
        /// </summary>
        protected virtual void OnLoad()
        {
            FromData(FindSelf(saveManager.RuntimeData));
        }

        /// <summary>
        /// 存档时的行为
        /// </summary>
        protected virtual void OnSave()
        {
            ToData(FindSelf(saveManager.RuntimeData));
        }

        /// <summary>
        /// 在此方法中规定如何从整个存档数据中找到自身的数据
        /// </summary>
        protected abstract Tdata FindSelf(WholeSaveData whole);

        /// <summary>
        /// 在此方法中定义存档数据如何作用于游戏对象
        /// </summary>
        public abstract void FromData(Tdata data);

        /// <summary>
        /// 在此方法中定义如何由游戏对象转化为存档数据
        /// </summary>
        public abstract void ToData(Tdata data);
    }
}