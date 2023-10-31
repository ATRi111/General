using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Save
{
    /// <summary>
    /// <para>规定了一个对象的存档数据，以及读档存档时存档数据与对象的关系</para>
    /// <para>子类不需要添加[Serializable]，但其字段必须正确使用public和[JsonProperty]</para>
    /// </summary>
    [System.Serializable]
    public abstract class SaveData
    {
        /// <summary>
        /// 默认的用于确定标识符的方法，即对象名+场景名
        /// </summary>
        public static string DefineIdentifier_Default(Object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SceneManager.GetActiveScene().name);
            sb.Append("-");
            sb.Append(obj.name);
            return sb.ToString();
        }

        /// <summary>
        /// 规定此存档数据属于哪一组，编号从自然数开始，负数用于调试
        /// </summary>
        protected abstract int GroupId { get; }
        protected GroupController groupController;

        /// <summary>
        /// 不要在构造函数以外的上下文中使用此属性
        /// </summary>
        protected abstract string Identifier { get; }
        [JsonProperty]
        internal string identifier;

        /// <summary>
        /// <para>与此存档数据绑定的对象，Identifier的确定通常会依赖此字段；Save和Load函数通常与此字段有关</para>
        /// <para>注意：对象与存档数据绑定后，由于场景切换等原因，对象可能不再存在，这种情况下Save和Load函数不会被自动调用</para>
        /// </summary>
        protected Object obj;

        public void Initialize(Object obj)
        {
            this.obj = obj;
            groupController = ServiceLocator.Get<ISaveManager>().GetGroup(GroupId);
            identifier = Identifier;
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