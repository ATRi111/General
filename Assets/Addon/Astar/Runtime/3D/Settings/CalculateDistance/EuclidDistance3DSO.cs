using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "欧几里得距离", menuName = "AStar3D/计算两点间距离的方法/欧几里得距离")]
    public class EuclidDistance3DSO : CalculateDistance3DSO
    {
        public override float CalculateDistance(Vector3Int from, Vector3Int to)
        {
            return (from - to).magnitude;
        }
    }
}
