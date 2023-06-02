using UnityEngine;

namespace AStar
{
    public abstract class CalculateWeightSO : ScriptableObject
    {
        public abstract float CalculateWeight(PathFindingProcess process);
    }
}