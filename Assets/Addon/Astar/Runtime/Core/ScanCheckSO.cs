using UnityEngine;

namespace AStar
{
    /// <summary>
    /// 用于JPS中，确定扫描何时停止
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public abstract class ScanCheckSO<TProcess, TNode> : ScriptableObject
        where TNode : Node
        where TProcess : PathFindingProcess
    {
        public abstract bool ScanCheck(TProcess process, TNode start, TNode current);
    }
}
