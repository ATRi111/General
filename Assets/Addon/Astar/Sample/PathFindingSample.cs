using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar
{
    public class PathFindingSample : MonoBehaviour
    {
        public PathFindingProcess process;
        public Transform from;
        public Transform to;
        public GameObject prefab;
        public Tilemap map;

        private void Start()
        {
            PathFindingSettings settings = new PathFindingSettings()
            {
                CalculateWeight = (x) => 2,
                DefineNodeType = DefineNodeType
            };
            process = new PathFindingProcess(settings);
            process.Start(WorldToNode(from.position), WorldToNode(to.position));
            Repaint();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                process.NextStep();
                Repaint();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                process.Compelete();
                Repaint();
            }
        }

        private Vector2Int WorldToNode(Vector3 world)
            => new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));

        private Vector3 NodeToWorld(Vector2Int node)
            => new Vector3(node.x + 0.5f, node.y + 0.5f, -1f);

        private void Repaint()
        {
            GameObject obj = GameObject.Find("debug");
            Destroy(obj);
            obj = new GameObject("debug");

            PathNode[] allnodes = process.GetAllNodes();
            for (int i = 0; i < allnodes.Length; i++)
            {
                PaintNode(allnodes[i], obj.transform);
            }
        }

        private void PaintNode(PathNode node, Transform parent)
        {
            GameObject obj = Instantiate(prefab);
            obj.name = node.Type.ToString();
            obj.transform.SetParent(parent);
            obj.transform.position = NodeToWorld(node.Position);

            obj.GetComponent<DebugNodeSample>().Initialize(node);
        }

        private ENodeType DefineNodeType(Vector2Int nodePos)
        {
            Vector3 world = NodeToWorld(nodePos);
            Vector3Int tilePos = map.WorldToCell(world);
            RuleTile tile = map.GetTile(tilePos) as RuleTile;
            if (tile != null && tile.m_DefaultGameObject != null && tile.m_DefaultGameObject.GetComponent<AstarBlockSample>() != null)
                return ENodeType.Obstacle;
            return ENodeType.Blank;
        }
    }
}