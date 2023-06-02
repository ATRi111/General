using UnityEngine;

namespace AStar
{
    public class ChebyshevDistanceSO : CalculateCostSO
    {
        public override float CalculateCost(Vector2Int from, Vector2Int to)
        {
            return PathFindingUtility.ChebyshevDistance(from, to);
        }
    }
}