using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridSortingOrderController : GridSortingOrderControllerBase
    {
        private IsometricGridManager igm;

        private SpriteRenderer spriteRenderer;

        protected override void Start()
        {
            igm = IsometricGridManager.Instance;
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        protected virtual void Update()
        {
            spriteRenderer.sortingOrder = igm.CellToSortingOrder(transform.position) + extraSortingOrder;
        }
    }
}