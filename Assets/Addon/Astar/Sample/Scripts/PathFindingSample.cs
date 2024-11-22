using UnityEngine;

namespace AStar.Sample
{
    public class PathFindingSample : MonoBehaviour
    {
        [SerializeField]
        private PathFindingProcess process;
        private GameObject prefab;
        private Transform from;
        private Transform to;

        public GetAdjoinedNodesSO getAdjoinedNodesSO;
        public float hCostWeight;
        public float moveAbility;

        public void StartPathFinding()
        {
            PathFindingSettings settings = new(getAdjoinedNodesSO.GetAdjoinedNodes, null, GenerateNode, hCostWeight);
            AStarMoverSample mover = new(process)
            {
                MoveAbility = () => moveAbility,
            };
            process = new(settings, mover)
            {
                mono = this
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
            process.Compelete();
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

            AStarNode[] allnodes = process.GetAllNodes();
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

        private void PaintNode(AStarNode node, Transform parent)
        {
            GameObject obj = Instantiate(prefab);
            obj.name = node.state.ToString();
            obj.transform.SetParent(parent);
            obj.transform.position = NodeToWorld(node.Position);

            obj.GetComponent<DebugNodeSample>().Initialize(node);
        }

        private AStarNodeSample GenerateNode(PathFindingProcess process, Vector2Int position)
        {
            return new AStarNodeSample(process, position);
        }

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("DebugNode");
            from = transform.Find("From").transform;
            to = transform.Find("To").transform;
            process.mono = this;
        }
    }
}