using System;
using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "固定扫描深度", menuName = "AStar2D/确定扫描深度的方法/固定深度")]
    public class FixedDepthSO : ScanCheck2DSO
    {
        public int depth;

        public override bool ScanCheck(PathFinding2DProcess process, Node2D start, Node2D current)
        {
            Vector2Int v = current.Position - start.Position;
            return Math.Max(Math.Abs(v.x), Math.Abs(v.y)) < depth;
        }
    }
}
