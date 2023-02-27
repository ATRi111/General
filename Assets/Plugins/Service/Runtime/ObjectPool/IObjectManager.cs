using UnityEngine;

namespace Services.ObjectPools
{
    public interface IObjectManager : IService
    {

        /// <summary>
        /// 激活一个带有IMyObject的游戏物体（若对象池中有未激活的游戏物体，则将其激活;否则创建一个物体，将其添加到对象池中，然后激活）
        /// </summary>
        /// <param name="identifier">要激活的游戏物体的标识符，通过标识符确定prefab的方式由ObjectLocatorBase规定</param>
        /// <param name="position">激活游戏物体后，将其位置设为此值</param>
        /// <param name="eulerAngles">激活游戏物体后，将其旋转设为此值</param>
        /// <param name="parent">激活游戏物体后，将其父物体设为此值（可以为空，即没有父物体）</param>
        /// <returns>激活的游戏物体上的IMyObject脚本</returns>
        IMyObject Activate(string identifier, Vector3 position, Vector3 eulerAngles, Transform parent = null);

        /// <summary>
        /// 预创建某种游戏物体若干个，并将其设为未激活状态（这会被安排在一帧中完成）
        /// </summary>
        /// <param name="identifier">要创建的游戏物体的标识符</param>
        /// <param name="count">个数</param>
        void PreCreate(string identifier, int count);
    }
}