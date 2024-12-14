using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "ŷ����þ���", menuName = "AStar/������������ķ���/ŷ����þ���")]
    public class EuclidDistanceSO : CalculateDistanceSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return (from - to).magnitude;
        }
    }
}