using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "对角线距离", menuName = "AStar/计算两点间距离的方法/对角线距离")]
    public class DiagonalDistanceSO : CalculateDistanceSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return PathFinding2DUtility.DiagonalDistance(from, to);
        }
    }
}