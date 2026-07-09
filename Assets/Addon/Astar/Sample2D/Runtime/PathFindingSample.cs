using AStar.TwoD;
using UnityEngine;

namespace AStar.Sample
{
    public class PathFindingSample : MonoBehaviour
    {
        [SerializeField]
        private PathFinding2DProcess process;
        [SerializeField]
        private GameObject prefab;
        private Transform from;
        private Transform to;

        public int moveAbility;

        public void StartPathFinding()
        {
            process.mover = new SampleMover(process)
            {
                moveAbility = moveAbility,
            };
            process.Start(WorldToNode(from.position), WorldToNode(to.position));
            Repaint();
        }

        public void Next()
        {
            process.NextStep();
            Repaint();
        }

        public void Complete()
        {
            // 只统计寻路算法本身耗时，不包含Repaint的可视化开销；用完全限定名避免和UnityEngine.Debug产生命名冲突（不额外加using System.Diagnostics）
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            process.Complete();
            stopwatch.Stop();
            // 顺带输出用的是哪种"获取相邻节点的方法"（朴素8向/JPS跳点等）+三个统计量，方便同一份耗时数据能直接对应到具体策略，
            // 不需要再切回Inspector里的PathFindingProcessDrawer去对照——统计量字段参考自PathFindingProcessDrawer里展示的那三个
            Debug.Log($"寻路耗时：{stopwatch.Elapsed.TotalMilliseconds:F3} ms，" +
                $"方法：{process.settings.GetAdjoinedNodesSOName}，" +
                $"生成节点次数：{process.generateCount}，位置查询次数：{process.queryCount}，入堆节点个数：{process.openCount}");
            Repaint();
        }

        internal Vector2Int WorldToNode(Vector3 world)
            => new(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));

        internal Vector3 NodeToWorld(Vector2Int node)
            => new(node.x + 0.5f, node.y + 0.5f, -1f);

        public void Repaint()
        {
            Clear();
            GameObject obj = new("debug");

            Node2D[] allNodes = process.GetAllNodes();
            for (int i = 0; i < allNodes.Length; i++)
            {
                PaintNode(allNodes[i], obj.transform);
            }

            PaintParentLinks(allNodes, obj.transform);
            PaintOutputPath(obj.transform);
        }

        public void Clear()
        {
            GameObject obj = GameObject.Find("debug");
            Destroy(obj);
        }

        private void PaintNode(Node2D node, Transform parent)
        {
            GameObject obj = Instantiate(prefab);
            obj.name = node.state.ToString();
            obj.transform.SetParent(parent);
            obj.transform.position = NodeToWorld(node.Position);

            obj.GetComponent<SampleDebugNode>().Initialize(node);
        }

        private void Awake()
        {
            from = transform.Find("From").transform;
            to = transform.Find("To").transform;
        }

        private const int arrowSortingOrder_ParentLink = 10;
        private const int arrowSortingOrder_Output = 11;

        /// <summary>
        /// 画出所有已发现节点的 Parent 连线（黄色箭头，箭头方向为 Parent 指向后续节点），
        /// 用 <see cref="DebugArrow"/>（LineRenderer+Mesh）而非 Gizmos 实现，因此不受
        /// Scene 视图 Gizmos 总开关/下拉框状态影响，Game 视图也能看到
        /// </summary>
        private void PaintParentLinks(Node2D[] allNodes, Transform parent)
        {
            Color color = new(1f, 1f, 0f, 0.5f);
            foreach (Node2D node in allNodes)
            {
                Node2D parentNode = node.Parent;
                if (parentNode == null)
                    continue;

                DebugArrow.Create(NodeToWorld(parentNode.Position), NodeToWorld(node.Position), color, parent, arrowSortingOrder_ParentLink);
            }
        }

        /// <summary>
        /// 画出最终输出路径上相邻节点的连线（红色箭头，方向为起点指向终点）
        /// </summary>
        private void PaintOutputPath(Transform parent)
        {
            Color color = new(1f, 0f, 0f, 0.5f);
            for (int i = 1; i < process.output.Count; i++)
            {
                Node2D a = (Node2D)process.output[i - 1];
                Node2D b = (Node2D)process.output[i];
                DebugArrow.Create(NodeToWorld(a.Position), NodeToWorld(b.Position), color, parent, arrowSortingOrder_Output);
            }
        }
    }
}
