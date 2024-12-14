using UnityEngine;

namespace AStar.Sample
{
    public class PathFindingSample : MonoBehaviour
    {
        [SerializeField]
        private PathFindingProcess process;
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
            process.Complete();
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

            Node[] allNodes = process.GetAllNodes();
            for (int i = 0; i < allNodes.Length; i++)
            {
                PaintNode(allNodes[i], obj.transform);
            }
        }

        public void Clear()
        {
            GameObject obj = GameObject.Find("debug");
            Destroy(obj);
        }

        private void PaintNode(Node node, Transform parent)
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
    }
}