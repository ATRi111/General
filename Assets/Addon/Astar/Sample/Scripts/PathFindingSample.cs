using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar
{
    public class PathFindingSample : MonoBehaviour
    {
        [SerializeField]
        private PathFindingProcess process;
        private GameObject prefab;
        private Transform from;
        private Transform to;
        private Tilemap map;

        public CalculateWeightSO calculateWeightSO;
        public CalculateCostSO calculateHCostSO;
        public GetAdjoinedNodesSO getAdjoinedNodesSO;

        public void StartPathFinging()
        {
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
            process.Compelete();
            Repaint();
        }

        private Vector2Int WorldToNode(Vector3 world)
            => new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));

        private Vector3 NodeToWorld(Vector2Int node)
            => new Vector3(node.x + 0.5f, node.y + 0.5f, -1f);

        private ENodeType DefineNodeType(Vector2Int nodePos)
        {
            Vector3 world = NodeToWorld(nodePos);
            Vector3Int tilePos = map.WorldToCell(world);
            RuleTile tile = map.GetTile(tilePos) as RuleTile;
            if (tile != null)
            {
                if (tile.m_DefaultSprite.name == "Block")
                    return ENodeType.Obstacle;
            }
            return ENodeType.Blank;
        }

        public void Repaint()
        {
            Clear();
            GameObject obj = new GameObject("debug");

            PathNode[] allnodes = process.GetAllNodes();
            for (int i = 0; i < allnodes.Length; i++)
            {
                PaintNode(allnodes[i], obj.transform);
            }
        }

        public void Clear()
        {
            GameObject obj = GameObject.Find("debug");
            Destroy(obj);
        }

        private void PaintNode(PathNode node, Transform parent)
        {
            GameObject obj = Instantiate(prefab);
            obj.name = node.Type.ToString();
            obj.transform.SetParent(parent);
            obj.transform.position = NodeToWorld(node.Position);

            obj.GetComponent<DebugNodeSample>().Initialize(node);
        }

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("DebugNode");
            from = transform.Find("From").transform;
            to = transform.Find("To").transform;
            map = GetComponentInChildren<Tilemap>();
        }

        private void Start()
        {
            PathFindingSettings settings = new PathFindingSettings(
                1000,
                2000,
                calculateWeightSO.CalculateWeight,
                calculateHCostSO.CalculateCost,
                null,
                getAdjoinedNodesSO.GetAdjoinedNodes,
                null,
                DefineNodeType
                );
            process = new PathFindingProcess(settings);
        }
    }
}