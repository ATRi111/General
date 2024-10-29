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

        public abstract void GetStrip(List<Vector3> ret);

        public abstract bool Overlap(Vector3 p);

        protected virtual void Awake()
        {
            GridObject.OverlapFunc = Overlap;
        }
    }
}