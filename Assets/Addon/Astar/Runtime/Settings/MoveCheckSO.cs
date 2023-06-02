using UnityEngine;

namespace AStar
{
    public abstract class MoveCheckSO : ScriptableObject
    {
        public abstract bool MoveCheck(PathNode from, PathNode to);
    }
}