using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "切比雪夫距离", menuName = "AStar/计算两点间距离的方法/切比雪夫距离")]
    public class ChebyshevDistanceSO : CalculateDistanceSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return PathFindingUtility.ChebyshevDistance(from, to);
        }
    }
}