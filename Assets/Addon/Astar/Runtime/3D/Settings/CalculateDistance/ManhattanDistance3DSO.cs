using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "曼哈顿距离", menuName = "AStar3D/计算两点间距离的方法/曼哈顿距离")]
    public class ManhattanDistance3DSO : CalculateDistance3DSO
    {
        public override float CalculateDistance(Vector3Int from, Vector3Int to)
        {
            return PathFinding3DUtility.ManhattanDistance(from, to);
        }
    }
}
