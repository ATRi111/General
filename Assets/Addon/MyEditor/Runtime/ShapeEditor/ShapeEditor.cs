using UnityEngine;

namespace MyEditor.ShapeEditor
{
    public abstract class ShapeEditor : MonoBehaviour
    {
        public Vector2 Position2D => new Vector3(transform.position.x, transform.position.y, 0f);

        protected virtual void Reset()
        {
            
        }

        public abstract Rect GetAABB();
    }
}