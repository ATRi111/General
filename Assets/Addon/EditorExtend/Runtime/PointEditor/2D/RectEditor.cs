using UnityEngine;

namespace EditorExtend.PointEditor
{
    public class RectEditor : PointEditor2D
    {
        public Vector2 offset;
        public Vector2 size;

        public Rect LocalRect => new Rect(offset - size / 2, size);
        public Rect WorldRect => new Rect(offset - size / 2 + Position2D, size);

        public override Rect GetAABB()
        {
            return WorldRect;
        }

        protected override void Reset()
        {
            base.Reset();
            offset = Vector2.zero;
            size = Vector2.one;
        }
    }
}