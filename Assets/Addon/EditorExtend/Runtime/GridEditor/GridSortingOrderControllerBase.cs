using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridSortingOrderControllerBase : MonoBehaviour
    {
        [Range(-10, 30)]
        public int extraSortingOrder;

        protected virtual void Start()
        {
            RefreshSortingOrder();
        }

        public void RefreshSortingOrder()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = IsometricGridManager.Instance.CellToSortingOrder(transform.position) + extraSortingOrder;
        }
    }
}