using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridCylinderCollider : GridCollider
    {
        public float height = 3f;
        public float radius = 0.3f;

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

        public override bool Overlap(Vector3 p)
        {
            return GridUtility.CylinderOverlap(CellPosition, height, radius, p);
        }
    }
}