using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "欧几里得距离", menuName = "AStar/计算两点间距离的方法/欧几里得距离")]
    public class EuclidDistanceSO : CalculateDistanceSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return (from - to).magnitude;
        }
    }
}