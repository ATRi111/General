using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [Serializable]
    public class PathFindingProcess
    {
        #region �趨
        [SerializeField]
        private PathFindingSettings settings;
        public PathFindingSettings Settings => settings;

        [SerializeField]
        private float currentWeight;
        /// <summary>
        /// Ѱ·�ĵ�ǰһ����,HCost��Ȩ��
        /// </summary>
        public float CurrentWeight => currentWeight;
        #endregion

        #region ��������
        //�ڵ��ڱ�����ʱ�Żᱻ����
        internal PathNode GetNode(Vector2 pos)
        {
            if (discoveredNodes.ContainsKey(pos))
                return discoveredNodes[pos];
            PathNode node = new PathNode(this, pos);
            node.Type = settings.DefineNodeType(pos);
            discoveredNodes.Add(pos, node);
            return node;
        }

        /// <summary>
        /// ��ȡ��һ���ڵ������ҿ�ͨ���Ҳ�ΪClose�Ľڵ�
        /// </summary>
        internal void GetAdjoinPassableNodes(PathNode from)
        {
            adjoins_handled.Clear();
            settings.GetAdjoinNodes.Invoke(this, from, adjoins_original);
            foreach (PathNode to in adjoins_original)
            {
                if (to.Type != ENodeType.Close && settings.CheckPassable(from, to))
                    adjoins_handled.Add(to);
            }
        }
        #endregion

        #region ״̬��
        [SerializeField]
        private bool isRunning;
        /// <summary>
        /// �Ƿ����ڽ���Ѱ·
        /// </summary>
        public bool IsRunning => isRunning;

        private readonly Dictionary<Vector2, PathNode> discoveredNodes = new Dictionary<Vector2, PathNode>();
        /// <summary>
        /// ���
        /// </summary>
        public PathNode From { get; private set; }
        /// <summary>
        /// �յ�
        /// </summary>
        public PathNode To { get; private set; }

        private readonly List<PathNode> adjoins_original = new List<PathNode>();
        private readonly List<PathNode> adjoins_handled = new List<PathNode>();

        private Heap<PathNode> open;
        /// <summary>
        /// ��ǰ��Ϊ������·��ĩ�˵ĵ�
        /// </summary>
        public PathNode Nearest { get; private set; }
        [SerializeField]
        private int depth;
        /// <summary>
        /// ��������
        /// </summary>
        public int Depth => depth;

        private List<PathNode> output;

        #endregion

        #region ���й���
        public void Start(Vector2 fromPos, Vector2 toPos, List<PathNode> ret)
        {
            isRunning = true;
            depth = 0;

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
            Nearest = From;
        }

        public void NextStep()
        {
            if (!CheckNextStep())
                return;

            PathNode temp = open.Pop();
            temp.Type = ENodeType.Close;
            GetAdjoinPassableNodes(temp);
            currentWeight = settings.CalculateWeight(this);

            foreach (PathNode node in adjoins_handled)
            {
                switch (node.Type)
                {
                    case ENodeType.Blank:
                        node.CalculateHCost(To);
                        node.Parent = temp;
                        node.Type = ENodeType.Open;
                        open.Push(node);
                        break;
                    case ENodeType.Route:
                        node.Parent = temp;
                        Nearest = node;
                        Stop();
                        break;
                    case ENodeType.Open:
                        if (node.GCost > temp.GCost + temp.CalculateDistance(node))
                            node.Parent = temp;
                        break;
                }
                if (node.HCost < Nearest.HCost)
                    Nearest = node;
                depth++;
            }
        }
        public void Stop()
        {
            isRunning = false;
            Nearest.Recall(output);
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

        public IEnumerator FindPath(Vector2Int startPos, Vector2Int endPos, List<PathNode> ret, bool display = false)
        {
            if (ret != null)
                ret.Clear();
            int maxDepth = Mathf.Min(settings.capacity * 2 + 2, settings.maxDepth);
            
            currentWeight = 1f;

            PathNode end = GetNode(endPos);
            end.Type = ENodeType.Route;

            PathNode start = GetNode(startPos);
            start.Type = ENodeType.Open;
            start.Parent = null;
            start.CalculateHCost(end);

            open.Push(start);

            PathNode nearest = start;
            PathNode temp;
            int depth = 0;
            float fcost_min = start.FCost;

            for (; !open.IsEmpty;)
            {
                temp = open.Pop();
                temp.Type = ENodeType.Close;
                GetAdjoinPassableNodes(temp);
                currentWeight = settings.CalculateWeight(this);

                foreach (PathNode node in adjoins_handled)
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
            Debug.Log("�Ҳ���·��");
            nearest.Recall(ret);
            yield break;
        }


        public PathFindingProcess(PathFindingSettings settings)
        {
            this.settings = settings;
            currentWeight = 1;
        }
    }
}