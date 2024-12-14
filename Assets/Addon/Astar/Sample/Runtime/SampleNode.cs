using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar.Sample
{
    public class SampleNode : Node
    {
        internal SampleNode(PathFindingProcess process, Vector2Int position) 
            : base(process, position)
        {

        }

        private Tilemap map;
        private PathFindingSample sample;
        private bool isObstacle;

        public override bool IsObstacle
        {
            get
            {
                if(map == null)
                {
                    isObstacle = false;
                    map = process.mountPoint.GetComponentInChildren<Tilemap>();
                    sample = process.mountPoint.GetComponentInChildren<PathFindingSample>();
                    Vector3 world = sample.NodeToWorld(Position);
                    Vector3Int tilePos = map.WorldToCell(world);
                    RuleTile tile = map.GetTile(tilePos) as RuleTile;
                    if (tile != null)
                    {
                        if (tile.m_DefaultSprite.name == "Block")
                            isObstacle = true;
                    }
                }
                return isObstacle;
            }
        }
    }
}