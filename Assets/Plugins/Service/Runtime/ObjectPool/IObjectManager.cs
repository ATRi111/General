using UnityEngine;

namespace Services.ObjectPools
{
    public interface IObjectManager : IService
    {
        /// <summary>
        /// 激活一个游戏物体，若对象池中的对象用完，创建一个对象并添加到对象池中，再激活
        /// </summary>
        /// <param name="identifier">要激活的游戏物体的标识符</param>
        /// <param name="position">位置</param>
        /// <param name="eulerAngles">欧拉角</param>
        /// <param name="parent">将激活的游戏物体设为某个游戏物体的子物体，默认情况下是对象池的子物体</param>
        /// <returns>被激活的游戏物体</returns>
        IMyObject Activate(string identifier, Vector3 position, Vector3 eulerAngles, Transform parent = null);

        /// <summary>
        /// (用于2D游戏)激活一个游戏物体，若对象池中的对象用完，创建一个对象并添加到对象池中，再激活
        /// </summary>
        /// <param name="identifier">要激活的游戏物体的标识符</param>
        /// <param name="position">位置</param>
        /// <param name="eulerAngleZ">z方向欧拉角</param>
        /// <param name="parent">将激活的游戏物体设为某个游戏物体的子物体，默认情况下是对象池的子物体</param>
        /// <returns>被激活的游戏物体</returns>
        IMyObject Activate(string identifier, Vector3 position, float eulerAngleZ = 0, Transform parent = null);

        /// <summary>
        /// 预生成物体（在一帧内）
        /// </summary>
        /// <param name="identifier">要生成的游戏物体的标识符</param>
        /// <param name="count">数量</param>
        void PreCreate(string identifier, int count);
    }
}