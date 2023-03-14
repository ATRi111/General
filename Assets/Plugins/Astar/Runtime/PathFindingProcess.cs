using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// һ��Ѱ·����
    /// </summary>
    [Serializable]
    public class PathFindingProcess
    {
        [SerializeField]
        private PathFindingSettings settings;
        public PathFindingSettings Settings => settings;

        private List<PathNode> output;

        #region ��������
        //�ڵ��ڱ�����ʱ�Żᱻ����
        internal PathNode GetNode(Vector2Int pos)
        {
            if (discoveredNodes.ContainsKey(pos))
                return discoveredNodes[pos];
            PathNode node = new PathNode(this, pos)
            {
                Type = settings.DefineNodeType(pos)
            };
            discoveredNodes.Add(pos, node);
            return node;
        }

        /// <summary>
        /// ��ȡ��һ���ڵ������ҿ�ͨ���Ҳ�ΪClose�Ľڵ�
        /// </summary>
        internal void GetAdjoinPassableNodes(PathNode from)
        {
            adjoins_original.Clear();
            adjoins_handled.Clear();
            settings.GetAdjoinNodes.Invoke(this, from, adjoins_original);
            foreach (PathNode to in adjoins_original)
            {
                if (to.Type != ENodeType.Close && settings.CheckPassable(from, to))
                    adjoins_handled.Add(to);
            }
        }

        public PathNode[] GetAllNodes()
        {
            return discoveredNodes.Values.ToArray();
        }
        #endregion

        #region ״̬��
        [SerializeField]
        private bool isRunning;
        /// <summary>
        /// �Ƿ����ڽ���Ѱ·
        /// </summary>
        public bool IsRunning => isRunning;

        /// <summary>
        /// ���
        /// </summary>
        public PathNode From { get; private set; }
        /// <summary>
        /// �յ�
        /// </summary>
        public PathNode To { get; private set; }

        internal readonly Dictionary<Vector2, PathNode> discoveredNodes = new Dictionary<Vector2, PathNode>();

        internal readonly List<PathNode> adjoins_original = new List<PathNode>();
        internal readonly List<PathNode> adjoins_handled = new List<PathNode>();

        internal Heap<PathNode> open;

        [SerializeField]
        internal PathNode currentNode;
        /// <summary>
        /// ��ǰ���ʵĵ�
        /// </summary>
        public PathNode CurrentNode => currentNode;

        internal PathNode nearest;
        /// <summary>
        /// ��ǰ�ѷ��ʵ����յ�����ĵ�
        /// </summary>
        public PathNode Nearest => nearest;
        [SerializeField]
        internal int depth;
        /// <summary>
        /// ��������
        /// </summary>
        public int Depth => depth;

        [SerializeField]
        internal float currentWeight;
        /// <summary>
        /// Ѱ·�ĵ�ǰһ����,HCost��Ȩ��
        /// </summary>
        public float CurrentWeight => currentWeight;

        #endregion

        #region ���й���
        public void Start(Vector2Int fromPos, Vector2Int toPos, List<PathNode> ret = null)
        {
            isRunning = true;
            depth = 0;
            currentWeight = 1f;

            discoveredNodes.Clear();
            open = new Heap<PathNode>(settings.capacity, new Comparer_Cost());
            output = ret;
            
            To = GetNode(toPos);
            To.Type = ENodeType.Route;

            From = GetNode(fromPos);
            From.Type = ENodeType.Route;
            From.Parent = null;
            From.CalculateHCost(To);

            open.Push(From);
            nearest = From;
        }

        public void NextStep()
        {
            if (!CheckNextStep())
                return;

            currentNode = open.Pop();
            currentNode.Type = ENodeType.Close;
            GetAdjoinPassableNodes(currentNode);
            currentWeight = settings.CalculateWeight(this);

            
            foreach (PathNode node in adjoins_handled)
            {
                switch (node.Type)
                {
                    case ENodeType.Blank:
                        node.CalculateHCost(To);
                        node.Parent = currentNode;
                        node.Type = ENodeType.Open;
                        open.Push(node);
                        break;
                    case ENodeType.Route:
                        node.Parent = currentNode;
                        nearest = node;
                        Stop();
                        break;
                    case ENodeType.Open:
                        if (node.GCost > currentNode.GCost + currentNode.CalculateDistance(node))
                            node.Parent = currentNode;
                        break;
                }
                if (node.HCost < nearest.HCost)
                    nearest = node;
                depth++;
            }
        }
        public void LastStep()
        {
            if (settings.debugMode == false)
                return;
        }
        public void Stop()
        {
            isRunning = false;
            nearest.Recall(output);
        }
        private bool CheckNextStep()
        {
            if (!isRunning)
            {
                Debug.LogWarning("Ѱ·δ��ʼ");
                return false;
            }
            if (depth > settings.maxDepth)
            {
                Debug.LogWarning("������������");
                Stop();
                return false;
            }
            if (open.IsEmpty)
            {
                Debug.LogWarning("�Ҳ���·��");
                Stop();
                return false;
            }
            return true;
        }
        #endregion

        public PathFindingProcess(PathFindingSettings settings)
        {
            this.settings = settings;
        }
    }
}