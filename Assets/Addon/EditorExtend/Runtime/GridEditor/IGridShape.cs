using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public interface IGridShape
    {
        void GetStrip(List<Vector3> ret);

        static void GetStrip_Default(List<Vector3> ret)
        {
            Vector3 temp = Vector3.zero;
            void Add(Vector3 delta)
            {
                temp += delta;
                ret.Add(temp);
            }

            ret.Clear();
            Add(Vector3.zero);
            Add(Vector3.up);
            Add(Vector3.forward);
            Add(Vector3.down);
            Add(Vector3.back);
            Add(Vector3.right);
            Add(Vector3.forward);
            Add(Vector3.left);
            Add(Vector3.up);
            Add(Vector3.right);
            Add(Vector3.down);
        }
    }
}