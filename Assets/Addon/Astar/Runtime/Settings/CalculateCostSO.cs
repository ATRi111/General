using UnityEngine;

namespace AStar
{
    public abstract class CalculateCostSO : ScriptableObject
    {
        public abstract float CalculateCost(Vector2Int from, Vector2Int to);
    }
}