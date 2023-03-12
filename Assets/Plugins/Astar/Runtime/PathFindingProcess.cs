using System;
using System.Collections.Generic;
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
            if (state.discoveredNodes.ContainsKey(pos))
                return state.discoveredNodes[pos];
            PathNode node = new PathNode(this, pos)
            {
                Type = settings.DefineNodeType(pos)
            };
            state.discoveredNodes.Add(pos, node);
            return node;
        }

        /// <summary>
        /// ��ȡ��һ���ڵ������ҿ�ͨ���Ҳ�ΪClose�Ľڵ�
        /// </summary>
        internal void GetAdjoinPassableNodes(PathNode from)
        {
            state.adjoins_handled.Clear();
            settings.GetAdjoinNodes.Invoke(this, from, state.adjoins_original);
            foreach (PathNode to in state.adjoins_original)
            {
                if (to.Type != ENodeType.Close && settings.CheckPassable(from, to))
                    state.adjoins_handled.Add(to);
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

        /// <summary>
        /// ���
        /// </summary>
        public PathNode From { get; private set; }
        /// <summary>
        /// �յ�
        /// </summary>
        public PathNode To { get; private set; }

        [SerializeField]
        private PathFindingState state;
        /// <summary>
        /// ��ǰ״̬
        /// </summary>
        public PathFindingState State => state;

        #endregion

        #region ���й���
        public void Start(Vector2Int fromPos, Vector2Int toPos, List<PathNode> ret)
        {
            isRunning = true;
            state.depth = 0;

            state.discoveredNodes.Clear();
            state.open = new Heap<PathNode>(settings.capacity, new Comparer_Cost());
            output = ret;
            
            To = GetNode(toPos);
            To.Type = ENodeType.Route;

            From = GetNode(fromPos);
            From.Type = ENodeType.Route;
            From.Parent = null;
            From.CalculateHCost(To);

            state.open.Push(From);
            state.nearest = From;
        }

        public void NextStep()
        {
            if (!CheckNextStep())
                return;

            PathNode temp = state.open.Pop();
            temp.Type = ENodeType.Close;
            GetAdjoinPassableNodes(temp);
            state.currentWeight = settings.CalculateWeight(this);

            foreach (PathNode node in state.adjoins_handled)
            {
                switch (node.Type)
                {
                    case ENodeType.Blank:
                        node.CalculateHCost(To);
                        node.Parent = temp;
                        node.Type = ENodeType.Open;
                        state.open.Push(node);
                        break;
                    case ENodeType.Route:
                        node.Parent = temp;
                        state.nearest = node;
                        Stop();
                        break;
                    case ENodeType.Open:
                        if (node.GCost > temp.GCost + temp.CalculateDistance(node))
                            node.Parent = temp;
                        break;
                }
                if (node.HCost < state.nearest.HCost)
                    state.nearest = node;
                state.depth++;
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
            state.nearest.Recall(output);
        }
        private bool CheckNextStep()
        {
            if (!isRunning)
            {
                Debug.LogWarning("Ѱ·δ��ʼ");
                return false;
            }
            if (state.depth > settings.maxDepth)
            {
                Debug.LogWarning("������������");
                Stop();
                return false;
            }
            if (state.open.IsEmpty)
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
            state = new PathFindingState();
        }
    }
}