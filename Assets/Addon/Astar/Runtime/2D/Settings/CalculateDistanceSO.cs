using UnityEngine;

namespace AStar.TwoD
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