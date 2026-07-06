using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "对角线距离", menuName = "AStar3D/计算两点间距离的方法/对角线距离")]
    public class DiagonalDistance3DSO : CalculateDistance3DSO
    {
        public override float CalculateDistance(Vector3Int from, Vector3Int to)
        {
            return PathFinding3DUtility.DiagonalDistance(from, to);
        }
    }
}
