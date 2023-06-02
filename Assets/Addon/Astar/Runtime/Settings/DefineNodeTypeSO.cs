using UnityEngine;

namespace AStar
{
    public abstract class DefineNodeTypeSO : ScriptableObject
    {
        public abstract ENodeType DefineNodeType(Vector2Int position);
    }
}