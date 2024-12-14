using UnityEngine;

namespace AStar
{
    [System.Serializable]
    public class CalculateDistanceSO : ScriptableObject
    {
        public virtual float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return default;
        }
    }
}