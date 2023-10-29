using System;
using UnityEngine.SceneManagement;

namespace Services.Save
{
    /// <summary>
    /// <para>规定了一个对象的存档数据，以及读档存档时存档数据与对象的关系</para>
    /// <para>子类不需要添加[Serializable]，但其字段必须正确使用public和[SerializedField]</para>
    /// </summary>
    [Serializable]
    public abstract class SaveData
    {
        /// <summary>
        /// 默认的用于确定标识符的方法，即对象名+场景名
        /// </summary>
        public static string DefineIdentifier_Default(UnityEngine.Object obj)
        {
            return obj.name + SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// 规定此存档数据属于哪一组
        /// </summary>
        protected abstract GroupController Controller { get; }

        public abstract string Identifier { get; }

        /// <summary>
        /// 与此存档数据绑定的对象
        /// </summary>
        internal UnityEngine.Object obj;

        /// <summary>
        /// 读档时,通过存档数据修改游戏物体的数据
        /// </summary>
        internal abstract void OnLoad();

        /// <summary>
        /// 存档时,通过游戏物体的数据生成存档数据
        /// </summary>
        internal abstract void OnSave();
    }
}