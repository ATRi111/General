using UnityEngine;

namespace Services
{
    public interface IObjectLocator
    {
        /// <summary>
        /// 获取对象预制体
        /// </summary>
        /// <param name="identifier">对象的标识符</param>
        GameObject Locate(string identifier);
    }
}