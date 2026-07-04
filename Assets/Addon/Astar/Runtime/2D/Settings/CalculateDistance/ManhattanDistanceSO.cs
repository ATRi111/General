using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "曼哈顿距离", menuName = "AStar/计算两点间距离的方法/曼哈顿距离")]
    public class ManhattanDistanceSO : CalculateDistanceSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return PathFinding2DUtility.ManhattanDistance(from, to);
        }
    }
}