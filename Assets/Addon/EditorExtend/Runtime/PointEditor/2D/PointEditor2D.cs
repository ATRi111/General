using UnityEngine;

namespace EditorExtend.PointEditor
{
    public abstract class PointEditor2D : MonoBehaviour
    {
        public Vector2 Position2D => new Vector3(transform.position.x, transform.position.y, 0f);

        protected virtual void Reset()
        {

        }

        public abstract Rect GetAABB();
    }
}