using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// 获取相邻可达节点的方法，寻路过程/节点类型由具体空间表示决定
    /// </summary>
    [Serializable]
    public abstract class GetMovableNodesSO<TProcess, TNode> : ScriptableObject
        where TNode : Node
    {
        public abstract void GetMovableNodes(TProcess process, TNode from, Func<TNode, TNode, bool> moveCheck, List<Node> ret);
    }
}
