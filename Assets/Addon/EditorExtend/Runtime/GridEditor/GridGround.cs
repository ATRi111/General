using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridGround : MonoBehaviour
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

        public bool isObstacle;
        public int height = 1;
        public virtual int GroundHeight()
        {
            if (isObstacle)
                return GridUtility.MaxHeight;
            return height;
        }

        protected virtual void Awake()
        {
            GridObject.GroundHeightFunc = GroundHeight;
        }
    }
}