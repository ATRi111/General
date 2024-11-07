using UnityEngine;

namespace EditorExtend.ShapeEditor
{
    public abstract class Shape2DMono : MonoBehaviour
    {
        public Vector2 Position2D => new Vector3(transform.position.x, transform.position.y, 0f);

        protected virtual void Reset()
        {

        }

        public abstract Rect GetAABB();
    }
}