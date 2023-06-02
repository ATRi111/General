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
        
        /// <summary>
        /// ��ȡ��ͼ��ĳ��λ�õĽڵ㣬���Զ�ȷ����ڵ�����
        /// </summary>
        internal PathNode GetNode(Vector2Int pos)
        {
            if (discoveredNodes.ContainsKey(pos))
                return discoveredNodes[pos];
            PathNode node = new PathNode(this, pos)
            {
                Type = settings.DefineNodeType(pos)
            };
            discoveredNodes.Add(pos, node);
            countOfQuery++;
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
                if (to.Type != ENodeType.Close && settings.MoveCheck(from, to))
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
        /// <summary>
        /// �����ѷ��ֽڵ�
        /// </summary>
        internal readonly Dictionary<Vector2, PathNode> discoveredNodes = new Dictionary<Vector2, PathNode>();

        private readonly List<PathNode> adjoins_original = new List<PathNode>();
        private readonly List<PathNode> adjoins_handled = new List<PathNode>();
        /// <summary>
        /// �����ʽڵ��
        /// </summary>
        internal Heap<PathNode> open;

        /// <summary>
        /// ��ǰ���ʵĵ�
        /// </summary>
        [SerializeField]
        internal PathNode currentNode;
        public PathNode CurrentNode => currentNode;

        internal PathNode nearest;
        /// <summary>
        /// ��ǰ�ѷ��ʵ����յ�����ĵ�
        /// </summary>
        public PathNode Nearest => nearest;
        [SerializeField]
        internal int countOfTestedNode;
        /// <summary>
        /// ���Թ��Ľڵ���
        /// </summary>
        public int CountOfTestedNode => countOfTestedNode;

        [SerializeField]
        internal int countOfQuery;
        /// <summary>
        /// ��ѯ�ڵ����
        /// </summary>
        public int CountOfQuery => countOfQuery;

        [SerializeField]
        internal float currentWeight;
        /// <summary>
        /// Ѱ·�ĵ�ǰһ����,HCost��Ȩ��
        /// </summary>
        public float CurrentWeight => currentWeight;

        #endregion

        #region ���й���
        /// <summary>
        /// ��ʼѰ·
        /// </summary>
        /// <param name="fromPos">���</param>
        /// <param name="toPos">�յ�</param>
        /// <param name="ret">���ս��</param>
        public void Start(Vector2Int fromPos, Vector2Int toPos, List<PathNode> ret = null)
        {
            if (fromPos == toPos)
            {
                Debug.LogWarning("������յ���ͬ");
                return;
            }

            isRunning = true;
            countOfQuery = 0;
            countOfTestedNode = 0;
            currentWeight = 1f;

            discoveredNodes.Clear();
            open = new Heap<PathNode>(settings.capacity, new Comparer_Cost());
            output = ret;

            To = GetNode(toPos);
            To.Type = ENodeType.Route;

            From = GetNode(fromPos);
            From.Type = ENodeType.Route;
            From.Parent = null;
            From.UpdateHCost(To);

            open.Push(From);
            nearest = From;
        }
        /// <summary>
        /// �������Ѱ·
        /// </summary>
        public void Compelete()
        {
            for(; ; )
            {
                if (!NextStep())
                    return;
            }
        }
        /// <summary>
        /// ����һ��Ѱ·
        /// </summary>
        public bool NextStep()
        {
            if (!CheckNextStep())
            {
                if(isRunning)
                    Stop();
                return false;
            }

            currentNode = open.Pop();
            currentNode.Type = ENodeType.Close;
            GetAdjoinPassableNodes(currentNode);
            currentWeight = settings.CalculateWeight(this);

            foreach (PathNode node in adjoins_handled)
            {
                switch (node.Type)
                {
                    case ENodeType.Blank:
                        node.UpdateHCost(To);
                        node.Parent = currentNode;
                        node.Type = ENodeType.Open;
                        open.Push(node);
                        break;
                    case ENodeType.Route:
                        node.Parent = currentNode;
                        nearest = node;
                        Stop();
                        return false;
                    case ENodeType.Open:
                        if (node.GCost > currentNode.GCost + currentNode.CalculateGCost(node))
                            node.Parent = currentNode;
                        break;
                }
                if (node.HCost < nearest.HCost)
                    nearest = node;
                countOfTestedNode++;
            }
            return true;
        }
        /// <summary>
        /// ֹͣѰ·�����ؽ��
        /// </summary>
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
            if (countOfTestedNode > settings.maxDepth)
            {
                Debug.LogWarning("������������");
                return false;
            }
            if (open.IsEmpty)
            {
                Debug.LogWarning("�Ҳ���·��");
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