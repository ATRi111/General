using Newtonsoft.Json;
using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// <para>规定了一个对象的存档数据，以及读档存档时存档数据与对象的关系</para>
    /// <para>子类不需要添加[Serializable]，但其字段必须正确使用public和[JsonProperty]</para>
    /// </summary>
    [System.Serializable]
    public abstract class SaveData
    {
        [JsonProperty]
        internal string identifier;

        /// <summary>
        /// <para>与此存档数据绑定的对象，Identifier的确定通常会依赖此字段；Save和Load函数通常与此字段有关</para>
        /// <para>注意：对象与存档数据绑定后，由于场景切换等原因，对象可能不再存在，这种情况下Save和Load函数不会被自动调用</para>
        /// </summary>
        protected Object obj;

        public void Initialize(string identifier, Object obj)
        {
            this.obj = obj;
            this.identifier = identifier;
        }

        /// <summary>
        /// 无法确定obj是否存在时，使用此方法避免发生错误
        /// </summary>
        public void LoadIfExist()
        {
            if (obj != null)
                Load();
        }
        /// <summary>
        /// 无法确定obj是否存在时，使用此方法避免发生错误
        /// </summary>
        public void SaveIfExist()
        {
            if (obj != null)
                Save();
        }

        /// <summary>
        /// 读档时的行为
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// 存档时的行为
        /// </summary>
        public abstract void Save();

        public override string ToString()
        {
            return identifier;
        }
    }
}