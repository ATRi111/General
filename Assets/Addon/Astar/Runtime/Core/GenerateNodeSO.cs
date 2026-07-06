using UnityEngine;

namespace AStar
{
    /// <summary>
    /// 生成新节点的方法，寻路过程/位置/节点类型由具体空间表示决定
    /// </summary>
    [System.Serializable]
    public class GenerateNodeSO<TProcess, TPosition, TNode> : ScriptableObject
        where TNode : Node
    {
        public virtual TNode GenerateNode(TProcess process, TPosition position)
        {
            return default;
        }
    }
}
