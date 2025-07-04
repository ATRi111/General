using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridSortingOrderControllerBase : MonoBehaviour
    {
        public static void RefreshChildren(Component obj)
        {
            if (obj != null)
            {
                GridSortingOrderControllerBase[] controllers = obj.GetComponentsInChildren<GridSortingOrderControllerBase>();
                for (int k = 0; k < controllers.Length; k++)
                {
                    controllers[k].RefreshSortingOrder();
                }
            }
        }

        protected SpriteRenderer spriteRenderer;
        [Range(-10, 30)]
        public int extraSortingOrder;

        protected GridManagerBase manager;
        protected virtual GridManagerBase Manager
        {
            get
            {
                if (manager == null)
                    manager = GetComponentInParent<GridManagerBase>();
                return manager;
            }
        }

        protected virtual void Start()
        {
            RefreshSortingOrder();
        }

        public virtual void RefreshSortingOrder()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = Manager.CellToSortingOrder(transform.position) + extraSortingOrder;
        }
    }
}