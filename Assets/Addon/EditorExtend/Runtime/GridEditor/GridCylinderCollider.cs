using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridCylinderCollider : GridCollider
    {
        public static Vector3 BottomCenterOffset = 0.5f * Vector2.one;
        public float height = 3f;
        public float radius = 0.3f;

        public override Vector3 Center => CellPosition + new Vector3(0.5f, 0.5f, 0.5f * height);

        public override Vector3 TopCenter => CellPosition + new Vector3(0.5f, 0.5f, height);

        public override Vector3 BottomCenter => CellPosition + new Vector3(0.5f, 0.5f, 0f);

        public override bool Overlap(Vector3 p)
        {
            return GridPhysics.CylinderOverlap(CellPosition + BottomCenterOffset, height, radius, p);
        }
        public override bool OverlapLineSegment(ref Vector3 from, ref Vector3 to)
        {
            return GridPhysics.LineSegmentCastCylinder(CellPosition + BottomCenterOffset, height, radius, ref from, ref to);
        }

        public override void GetStrip(List<Vector3> ret)
        {
            void AddArc(Vector3 center, float radius, float from, float to, int count)
            {
                Vector3 r;
                float angle;
                for (int i = 0; i < count; i++)
                {
                    angle = Mathf.Lerp(from, to, (float)i / count);
                    r = angle.ToDirection();
                    ret.Add(center + radius * r);
                }
            }
            
            ret.Clear();
            Vector3 center = new(0.5f, 0.5f, height);
            Vector3 vertical = new(0, 0, height);

            AddArc(center, radius, -135, 45, 30);
            center -= vertical;
            AddArc(center, radius, 45, 225, 30);
            center += vertical;
            AddArc(center, radius, 225, 45, 30);
        }
    }
}