using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace AStarOnGrid
{
    public class PathFinding
    {
        /// <summary>
        /// HCost权重
        /// </summary>
        public static float Weight { get; private set; }

        public static float CalculateWeight(float fcost, float fcost_min)
        {
            return fcost / fcost_min;
        }

        public readonly Dictionary<Vector2Int, AStarNode> map = new Dictionary<Vector2Int, AStarNode>();
        private readonly int capacity;          //节点容量
        private const int MaxDepth = 1000;

        /// <param name="_capacity">最大访问节点数量</param>
        public PathFinding(int _capacity)
        {
            capacity = _capacity;
        }

        //地图上的节点首次获取时才创建
        internal AStarNode GetNode(Vector2Int pos)
        {
            if (map.ContainsKey(pos))
                return map[pos];
            AStarNode node = new AStarNode(pos);
            map.Add(pos, node);
            return node;
        }

        /// <summary>
        /// 获取与一个节点相邻且可通行且不为Close的节点
        /// </summary>
        internal List<AStarNode> GetAdjoinNodes(AStarNode node, Func<Vector2Int, Vector2Int, bool> Passable)
        {
            List<Vector2Int> around = EDirectionTool.GetDirections();
            List<AStarNode> adjoins = new List<AStarNode>();
            Vector2Int targetPos;
            AStarNode target;
            foreach (Vector2Int position in around)
            {
                targetPos = node.Position + position;
                target = GetNode(targetPos);
                if (target.Type != ENodeType.Close && Passable(node.Position, targetPos))
                    adjoins.Add(target);
            }
            return adjoins;
        }

        public IEnumerator FindPath(Vector2Int startPos, Vector2Int endPos, Func<Vector2Int, Vector2Int, bool> CanMove, List<Vector2Int> ret, bool display = false)
        {
            map.Clear();
            if (ret != null)
                ret.Clear();
            int maxDepth = Mathf.Min(capacity * 2 + 2, MaxDepth);
            Heap<AStarNode> open = new Heap<AStarNode>(capacity, new Comparer_Cost());
            Weight = 1f;

            AStarNode start = GetNode(startPos);
            AStarNode end = GetNode(endPos);
            start.Type = ENodeType.Open;
            start.Parent = null;
            start.CalculateHCost(end);
            end.Type = ENodeType.Route;
            open.Push(start);

            AStarNode nearest = start;
            AStarNode temp;
            List<AStarNode> adjoins;
            int depth = 0;
            float fcost_min = start.FCost;

            for (; !open.IsEmpty;)
            {
                temp = open.Pop();
                temp.Type = ENodeType.Close;
                adjoins = GetAdjoinNodes(temp, CanMove);
                Weight = CalculateWeight(temp.FCost, fcost_min);

                foreach (AStarNode node in adjoins)
                {
                    switch (node.Type)
                    {
                        case ENodeType.Blank:
                            node.CalculateHCost(end);
                            node.Parent = temp;
                            node.Type = ENodeType.Open;
                            open.Push(node);
                            break;
                        case ENodeType.Route:
                            node.Parent = temp;
                            node.Recall(ret);
                            yield break;
                        case ENodeType.Open:
                            if (node.GCost > temp.GCost + temp.CalculateDistance(node))
                                node.Parent = temp;
                            break;
                    }
                    if (node.HCost < nearest.HCost)
                        nearest = node;
                    depth++;
                    if (depth > maxDepth)
                    {
                        nearest.Recall(ret);
                        yield break;
                    }
                    if (display)
                        yield return null;
                }
            }
            Debug.Log("找不到路径");
            nearest.Recall(ret);
            yield break;
        }
    }
}