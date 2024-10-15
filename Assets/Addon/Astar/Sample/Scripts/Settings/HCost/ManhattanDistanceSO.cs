using UnityEngine;

namespace AStar.Sample
{
    public class ManhattanDistanceSO : CalculateCostSO
    {
        public override float CalculateCost(Vector2Int from, Vector2Int to)
        {
            return PathFindingUtility.ManhattanDistance(from, to);
        }
    }
}