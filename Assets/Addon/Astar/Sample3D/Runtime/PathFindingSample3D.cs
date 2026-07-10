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

        private Dictionary<Vector3Int, string> obstacleMap;
        /// <summary>由 <see cref="EnsureObstacleMap"/> 顺带统计出的障碍物格子范围，供 <see cref="StartPathFinding"/> 据此自动设置边界</summary>
        private Vector3Int obstacleMin, obstacleMax;

        public int moveAbility = 999;

        /// <summary>
        /// 竖直方向在最高障碍物顶部之上，再额外预留多少格可通行空间（供飞越/爬上最高障碍物顶部）——
        /// 水平范围(x/z)不需要这个字段，会在 <see cref="EnsureObstacleMap"/> 统计障碍物格子时自动算出
        /// （场景里的地面本身也是Block，天然铺满了整块地图的x/z范围，边界正好贴合，不用额外预留）；
        /// 只有"障碍物顶部再往上留多少空间"这件事没法从障碍物本身推出来，需要单独给一个余量
        /// </summary>
        public int verticalBuffer = 3;

        public bool hideNodesOutsidePath = true;

        public void StartPathFinding()
        {
            // 边界依赖障碍物格子的实际范围，必须先统计（EnsureObstacleMap）才能设置
            EnsureObstacleMap();
            process.useBoundary = true;
            process.boundaryMin = obstacleMin;
            process.boundaryMax = new Vector3Int(obstacleMax.x, obstacleMax.y + verticalBuffer, obstacleMax.z);

            process.mover = new MoverBase()
            {
                moveAbility = moveAbility,
            };
            process.Start(WorldToNode(from.position), WorldToNode(to.position));
            NotifyCameraRoamer();
            Repaint();
        }

        public void Next()
        {
            process.NextStep();
            NotifyCameraRoamer();
            Repaint();
        }

        public void Complete()
        {
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            process.Complete();
            stopwatch.Stop();
            Debug.Log($"寻路耗时：{stopwatch.Elapsed.TotalMilliseconds:F3} ms，" +
                $"方法：{process.settings.GetAdjoinedNodesSOName}，" +
                $"生成节点次数：{process.generateCount}，位置查询次数：{process.queryCount}，入堆节点个数：{process.openCount}");
            NotifyCameraRoamer();
            Repaint();
        }

        private void NotifyCameraRoamer()
        {
            if (TryGetComponent<PathCameraRoamer>(out var roamer))
                roamer.SetSample(this);
        }

        internal Vector3Int WorldToNode(Vector3 world)
            => new(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y), Mathf.FloorToInt(world.z));

        internal Vector3 NodeToWorld(Vector3Int node)
            => new(node.x + 0.5f, node.y + 0.5f, node.z + 0.5f);

        public Vector3[] GetOutputPathWorldPoints()
        {
            Vector3[] points = new Vector3[process.output.Count];
            for (int i = 0; i < points.Length; i++)
                points[i] = NodeToWorld(((Node3D)process.output[i]).Position);
            return points;
        }

        public void Repaint()
        {
            Clear();
            GameObject obj = new("debug");

            if (!hideNodesOutsidePath)
            {
                Node3D[] allNodes = process.GetAllNodes();
                for (int i = 0; i < allNodes.Length; i++)
                {
                    PaintNode(allNodes[i], obj.transform);
                }
                PaintParentLinks(allNodes, obj.transform);
            }
            else
            {
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
        /// 每次开始寻路都重新建一遍（不缓存复用）——障碍物字典的key是世界坐标，如果root被移动过，
        /// 缓存的旧字典里全是移动前的世界坐标，重新遍历子物体的开销很小，直接每次都重建。
        /// 顺带统计出这些障碍物格子的坐标范围(<see cref="obstacleMin"/>/<see cref="obstacleMax"/>)，
        /// 供 <see cref="StartPathFinding"/> 据此自动设置边界——不需要再手动填一个gridSize：
        /// 场景里的地面本身也是Block，天然铺满了整块地图的x/z范围，边界正好贴合实际地形；
        /// 唯独"障碍物顶部再往上留多少可通行空间"没法从障碍物本身推出来，用 <see cref="verticalBuffer"/> 补足
        /// </summary>
        private void EnsureObstacleMap()
        {
            obstacleMap = new Dictionary<Vector3Int, string>();
            bool hasAny = false;
            Transform mountPoint = process.mountPoint;
            for (int i = 0; i < mountPoint.childCount; i++)
            {
                Transform child = mountPoint.GetChild(i);
                if (!child.name.StartsWith("Block"))
                    continue;

                Vector3Int pos = WorldToNode(child.position);
                obstacleMap[pos] = child.name;

                if (!hasAny)
                {
                    obstacleMin = pos;
                    obstacleMax = pos;
                    hasAny = true;
                }
                else
                {
                    obstacleMin = Vector3Int.Min(obstacleMin, pos);
                    obstacleMax = Vector3Int.Max(obstacleMax, pos);
                }
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
