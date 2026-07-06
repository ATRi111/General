using UnityEngine;

namespace AStar
{
    /// <summary>
    /// 计算两点间距离的方法，位置类型由具体空间表示决定
    /// </summary>
    [System.Serializable]
    public class CalculateDistanceSO<TPosition> : ScriptableObject
    {
        public virtual float CalculateDistance(TPosition from, TPosition to)
        {
            return default;
        }
    }
}
