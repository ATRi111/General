using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar.Sample
{
    public class SampleMover : MoverBase
    {
        private readonly Tilemap map;
        private readonly PathFindingSample sample;

        public SampleMover(PathFindingProcess process)
        {
            map = process.mountPoint.GetComponentInChildren<Tilemap>();
            sample = process.mountPoint.GetComponentInChildren<PathFindingSample>();
        }

        public override bool StayCheck(Node node)
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