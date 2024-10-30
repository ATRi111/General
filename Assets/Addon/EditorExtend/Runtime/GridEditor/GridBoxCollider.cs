using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridBoxCollider : GridCollider
    {
        public float height = 1;
        public override bool Overlap(Vector3 p)
        {
            return GridUtility.BoxOverlap(CellPosition, Vector3.one.ResetZ(height), p);
        }

        public override void GetStrip(List<Vector3> ret)
        {
            Vector3 temp = Vector3.zero;
            void Add(Vector3 delta)
            {
                temp += delta;
                ret.Add(temp);
            }
            
            ret.Clear();
            Vector3 vz = height * Vector3.forward;
            ret.Clear();
            Add(Vector3.zero);
            Add(Vector3.up);
            Add(vz);
            Add(Vector3.down);
            Add(-vz);
            Add(Vector3.right);
            Add(vz);
            Add(Vector3.left);
            Add(Vector3.up);
            Add(Vector3.right);
            Add(Vector3.down);
        }
    }
}