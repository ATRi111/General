using UnityEngine;

namespace AStar
{
    public class ManhattanDistanceSO : CalculateCostSO
    {
        public override float CalculateCost(Vector2Int from, Vector2Int to)
        {
            return PathFindingUtility.ManhattanDistance(from, to);
        }
    }
}