using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar.Sample
{
    public class AStarMoverSample : AStarMover
    {
        private readonly Tilemap map;
        private readonly PathFindingSample sample;

        public AStarMoverSample(PathFindingProcess process)
        {
            map = process.mono.GetComponentInChildren<Tilemap>();
            sample = process.mono.GetComponentInChildren<PathFindingSample>();
        }

        public override bool StayCheck(AStarNode node)
        {
            if(!base.StayCheck(node))
                return false;

            Vector3 world = sample.NodeToWorld(node.Position);
            Vector3Int tilePos = map.WorldToCell(world);
            RuleTile tile = map.GetTile(tilePos) as RuleTile;
            if (tile != null)
            {
                if (tile.m_DefaultSprite.name == "Pawn")
                    return false;
            }
            return true;
        }
    }
}