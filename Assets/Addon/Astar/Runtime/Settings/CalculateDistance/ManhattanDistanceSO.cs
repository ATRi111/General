using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "�����پ���", menuName = "AStar/������������ķ���/�����پ���")]
    public class ManhattanDistanceSO : CalculateDistanceSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return PathFindingUtility.ManhattanDistance(from, to);
        }
    }
}