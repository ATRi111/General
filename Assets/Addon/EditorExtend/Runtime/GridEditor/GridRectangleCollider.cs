using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridRectangleCollider : GridCollider
    {
        public override Vector3 Center => CellPosition + new Vector3(0.5f, 0.5f);
        public override Vector3 TopCenter => CellPosition + new Vector3(0.5f, 0.5f);

        public override Vector3 BottomCenter => CellPosition + new Vector3(0.5f, 0.5f);

        public override bool Overlap(Vector3 p)
        {
            return GridPhysics.RectangleOverlap(CellPosition, Vector2.one, p);
        }

        public override bool OverlapLineSegment(ref Vector3 from, ref Vector3 to)
        {
            return GridPhysics.LineSegmentCastRectangle(CellPosition, Vector2.one, ref from, ref to);
        }

        public override void GetStrip(List<Vector3> ret)
        {
            ret.Clear();
            ret.Add(Vector2.zero);
            ret.Add(Vector3.right);
            ret.Add(Vector3.one);
            ret.Add(Vector3.up);
            ret.Add(Vector3.zero);
        }
    }
}