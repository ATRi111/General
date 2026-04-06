using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public abstract class GridCollider : MonoBehaviour, IGridShape
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

        /// <summary>
        /// 判断是否与一点重合
        /// </summary>
        public abstract bool Overlap(Vector3 p);

        /// <summary>
        /// 求与线段的交点
        /// </summary>
        /// <returns>是否有交点</returns>
        public abstract bool OverlapLineSegment(ref Vector3 from, ref Vector3 to);

        /// <summary>
        /// 获取绘制用的点序列(本地坐标系，不会自动连成闭合折线段)
        /// </summary>
        public abstract void GetStrip(List<Vector3> ret);
    }
}