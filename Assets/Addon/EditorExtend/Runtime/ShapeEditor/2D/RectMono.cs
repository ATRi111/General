using UnityEngine;

namespace EditorExtend.ShapeEditor
{
    public class RectMono : Shape2DMono
    {
        public Vector2 offset;
        public Vector2 size;

        public Rect LocalRect => new(offset - size / 2, size);
        public Rect WorldRect => new(offset - size / 2 + Position2D, size);

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