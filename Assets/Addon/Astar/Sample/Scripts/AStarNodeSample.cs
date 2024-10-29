using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar.Sample
{
    public class AStarNodeSample : AStarNode
    {
        internal AStarNodeSample(PathFindingProcess process, Vector2Int position) 
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
                    map = process.mono.GetComponentInChildren<Tilemap>();
                    sample = process.mono.GetComponentInChildren<PathFindingSample>();
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