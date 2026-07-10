using System;
using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "固定扫描深度", menuName = "AStar3D/确定扫描深度的方法/固定深度")]
    public class FixedDepth3DSO : ScanCheck3DSO
    {
        public int depth = 10;

        public override bool ScanCheck(PathFinding3DProcess process, Node3D start, Node3D current)
        {
            Vector3Int v = current.Position - start.Position;
            return Math.Max(Math.Max(Math.Abs(v.x), Math.Abs(v.y)), Math.Abs(v.z)) < depth;
        }
    }
}
