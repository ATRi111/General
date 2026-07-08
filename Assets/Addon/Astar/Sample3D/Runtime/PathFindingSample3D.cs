using System.Collections.Generic;
using AStar.ThreeD;
using UnityEngine;

namespace AStar.Sample
{
    public class PathFindingSample3D : MonoBehaviour
    {
        [SerializeField]
        private PathFinding3DProcess process;
        [SerializeField]
        private GameObject prefab;
        private Transform from;
        private Transform to;

        /// <summary>
        /// 场景里所有"地块"（挂载点下名字以Block/Pawn开头的物体）按格子位置存放的字典，
        /// 由 <see cref="EnsureObstacleMap"/> 直接读取Transform位置建立一次并复用，之后 <see cref="SampleNode3D"/>/
        /// <see cref="SampleMover3D"/> 全部查表判断，不再使用 Physics.OverlapBox（避免物理查询的时序/精度问题导致漏判或误判）
        /// </summary>
        private Dictionary<Vector3Int, string> obstacleMap;

        /// <summary>
        /// 移动力：<see cref="MoverBase.MoveAbilityCheck"/> 会据此过滤最终输出路径（<see cref="Node.Recall"/>），
        /// GCost超出这个值的节点不会进入 <see cref="AStar.PathFindingProcess.output"/>。默认给一个覆盖全图对角线长度的
        /// 富余值，保证Demo默认就能画出完整路径（不像2D场景那样是手工在Inspector里配置好的，这里在代码里给个安全默认值）
        /// </summary>
        public int moveAbility = 999;

        /// <summary>
        /// 场景里可寻路的网格范围（从 (0,0,0) 到 gridSize-1），<see cref="StartPathFinding"/> 时会据此自动启用并设置
        /// <see cref="process"/> 的边界，避免六向JPS等沿单一方向一直扫描到地图外、甚至因为地形边缘外没有生成节点而报错
        /// </summary>
        public Vector3Int gridSize = new(10, 10, 10);

        public void StartPathFinding()
        {
            // 边界/障碍字典依赖 process.mountPoint 与 gridSize，这两者在编辑器一键搭建脚本里是通过
            // SerializedObject 在 AddComponent 之后才写入的——而 AddComponent 会立刻触发 Awake，
            // 若在 Awake 里读取会拿到尚未赋值的默认值（mountPoint 甚至还是 null）。
            // 因此延后到这里（真正开始寻路、且一定已经手动或由编辑器脚本配置完毕时）才初始化
            process.useBoundary = true;
            process.boundaryMin = Vector3Int.zero;
            process.boundaryMax = gridSize - Vector3Int.one;
            EnsureObstacleMap();

            process.mover = new MoverBase()
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
            process.Complete();
            Repaint();
        }

        internal Vector3Int WorldToNode(Vector3 world)
            => new(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y), Mathf.FloorToInt(world.z));

        internal Vector3 NodeToWorld(Vector3Int node)
            => new(node.x + 0.5f, node.y + 0.5f, node.z + 0.5f);

        public void Repaint()
        {
            Clear();
            GameObject obj = new("debug");

            if (process.IsRunning)
            {
                // 寻路仍在单步进行中：展示全部已发现节点及其Parent连线，便于观察搜索过程
                Node3D[] allNodes = process.GetAllNodes();
                for (int i = 0; i < allNodes.Length; i++)
                {
                    PaintNode(allNodes[i], obj.transform);
                }
                PaintParentLinks(allNodes, obj.transform);
            }
            else
            {
                // 寻路已结束：只画最终路径上的节点，隐藏其余所有已发现节点/连线，避免无关信息淹没结果
                foreach (Node node in process.output)
                {
                    PaintNode((Node3D)node, obj.transform);
                }
            }

            PaintOutputPath(obj.transform);
        }

        public void Clear()
        {
            GameObject obj = GameObject.Find("debug");
            Destroy(obj);
        }

        private void PaintNode(Node3D node, Transform parent)
        {
            GameObject obj = Instantiate(prefab);
            obj.name = node.state.ToString();
            obj.transform.SetParent(parent);
            obj.transform.position = NodeToWorld(node.Position);

            obj.GetComponent<SampleDebugNode3D>().Initialize(node);
        }

        private void Awake()
        {
            from = transform.Find("From").transform;
            to = transform.Find("To").transform;
        }

        /// <summary>
        /// 遍历挂载点下的直接子物体，把名字以Block/Pawn开头的物体按其世界坐标换算出的格子位置存入字典，
        /// 建过一次后直接复用。由 <see cref="StartPathFinding"/> 调用（而不是 Awake），
        /// 确保此时 <see cref="process"/>.mountPoint 已经被正确赋值
        /// </summary>
        private void EnsureObstacleMap()
        {
            if (obstacleMap != null)
                return;

            obstacleMap = new Dictionary<Vector3Int, string>();
            Transform mountPoint = process.mountPoint;
            for (int i = 0; i < mountPoint.childCount; i++)
            {
                Transform child = mountPoint.GetChild(i);
                if (child.name.StartsWith("Block"))
                    obstacleMap[WorldToNode(child.position)] = child.name;
            }
        }

        internal bool IsBlockAt(Vector3Int position)
            => obstacleMap.TryGetValue(position, out string name);

        private const int arrowSortingOrder_ParentLink = 10;
        private const int arrowSortingOrder_Output = 11;

        /// <summary>
        /// 画出所有已发现节点的 Parent 连线（黄色箭头，箭头方向为 Parent 指向后续节点）
        /// </summary>
        private void PaintParentLinks(Node3D[] allNodes, Transform parent)
        {
            Color color = new(1f, 1f, 0f, 0.5f);
            foreach (Node3D node in allNodes)
            {
                Node3D parentNode = node.Parent;
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
                Node3D a = (Node3D)process.output[i - 1];
                Node3D b = (Node3D)process.output[i];
                DebugArrow.Create(NodeToWorld(a.Position), NodeToWorld(b.Position), color, parent, arrowSortingOrder_Output);
            }
        }
    }
}
