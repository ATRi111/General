using UnityEngine;

namespace AStar
{
    [System.Serializable]
    public class GenerateNodeSO : ScriptableObject
    {
        public virtual Node GenerateNode(PathFindingProcess process, Vector2Int position)
        {
            return default;
        }
    }
}