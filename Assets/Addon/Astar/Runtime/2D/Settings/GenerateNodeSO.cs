using UnityEngine;

namespace AStar.TwoD
{
    [System.Serializable]
    public class GenerateNodeSO : ScriptableObject
    {
        public virtual Node2D GenerateNode(PathFindingProcess process, Vector2Int position)
        {
            return default;
        }
    }
}