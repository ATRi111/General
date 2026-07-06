using AStar.TwoD;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar.Sample
{
    public class SampleMover : MoverBase
    {
        private readonly Tilemap map;
        private readonly PathFindingSample sample;

        public SampleMover(PathFinding2DProcess process)
        {
            map = process.mountPoint.GetComponentInChildren<Tilemap>();
            sample = process.mountPoint.GetComponentInChildren<PathFindingSample>();
        }

        public override bool StayCheck(Node node)
        {
            if (!base.StayCheck(node))
                return false;

            Node2D node2D = (Node2D)node;
            Vector3 world = sample.NodeToWorld(node2D.Position);
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
