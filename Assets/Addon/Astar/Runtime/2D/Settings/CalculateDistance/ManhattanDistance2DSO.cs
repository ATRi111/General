using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "曼哈顿距离", menuName = "AStar2D/计算两点间距离的方法/曼哈顿距离")]
    public class ManhattanDistance2DSO : CalculateDistance2DSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return PathFinding2DUtility.ManhattanDistance(from, to);
        }
    }
}