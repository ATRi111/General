using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "欧几里得距离", menuName = "AStar2D/计算两点间距离的方法/欧几里得距离")]
    public class EuclidDistance2DSO : CalculateDistance2DSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return (from - to).magnitude;
        }
    }
}