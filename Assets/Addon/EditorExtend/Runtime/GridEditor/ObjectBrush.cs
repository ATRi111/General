using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [RequireComponent(typeof(GridManagerBase))]
    public abstract class ObjectBrush : MonoBehaviour
    {
#if UNITY_EDITOR
        private GridManagerBase manager;
        public GridManagerBase Manager
        {
            get
            {
                if (manager == null)
                    manager = GetComponentInParent<GridManagerBase>();
                return manager;
            }
        }

        public GameObject prefab;
        public Vector3Int cellPosition;

        public abstract Vector3Int CalculateCellPosition(Vector3 worldPosition);

        private readonly List<Vector3> gizmoPoints = new();
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(prefab == null)
            {
                IGridShape.GetStrip_Default(gizmoPoints);
            }
            else
            {
                if (!prefab.TryGetComponent(out GridCollider collider))
                    IGridShape.GetStrip_Default(gizmoPoints);
                else
                    collider.GetStrip(gizmoPoints);
            }
            Vector3[] temp = new Vector3[gizmoPoints.Count];
            for(int i = 0; i < gizmoPoints.Count; i++)
            {
                temp[i] = Manager.CellToWorld(gizmoPoints[i] + cellPosition).ResetZ();
            }
            Gizmos.DrawLineStrip(temp, false);
        }
#endif
    }
}