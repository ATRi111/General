using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public abstract class GridCollider : MonoBehaviour,IGridShape
    {
        private GridObject gridObject;
        protected GridObject GridObject
        {
            get
            {
                if (gridObject == null)
                    gridObject = GetComponentInParent<GridObject>();
                return gridObject;
            }
        }
        protected Vector3Int CellPosition => GridObject.CellPosition;
        public abstract Vector3 Center { get; }
        public abstract Vector3 TopCenter { get; }
        public abstract Vector3 BottomCenter { get; }

        public abstract bool Overlap(Vector3 p);

        public abstract bool OverlapLineSegment(ref Vector3 from, ref Vector3 to);

        public abstract void GetStrip(List<Vector3> ret);
    }
}