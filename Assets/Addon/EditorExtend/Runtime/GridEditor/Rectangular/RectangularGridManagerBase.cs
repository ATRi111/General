using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class RectangularGridManagerBase : GridManagerBase
    {
        public override int CellToSortingOrder(Vector3 position)
        {
            return 0;
        }
    }
}