using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "曼哈顿距离", menuName = "AStar/计算两点间距离的方法/曼哈顿距离")]
    public class ManhattanDistanceSO : CalculateDistanceSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return PathFindingUtility.ManhattanDistance(from, to);
        }
    }
}