using UnityEditor;
using UnityEngine;

namespace EditorExtend.PointEditor
{
    [CustomEditor(typeof(RectEditor))]
    public class Editor_RectEditor : Editor_PointEditor2D
    {
        private const float MinSize = 0.01f;

        [AutoProperty]
        public SerializedProperty offset, size;
        private RectEditor rectEditor;
        private Vector3[] vertices;
        private Vector2[] mids;
        private Vector2[] directions;

        protected override void OnEnable()
        {
            base.OnEnable();
            vertices = new Vector3[5];
            mids = new Vector2[4];
            directions = new Vector2[] { Vector2.left, Vector2.up, Vector2.right, Vector2.down };
            rectEditor = target as RectEditor;
        }

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            EditorGUILayout.Space();
            offset.Vector2Field("offset");
            size.Vector2Field("size");
            size.vector2Value = new Vector2(Mathf.Max(MinSize, size.vector2Value.x), Mathf.Max(MinSize, size.vector2Value.y));
        }

        protected override void Paint()
        {
            base.Paint();
            Rect rect = rectEditor.WorldRect;
            vertices[0] = rect.min;
            vertices[1] = new Vector2(rect.xMin, rect.yMax);
            vertices[2] = rect.max;
            vertices[3] = new Vector2(rect.xMax, rect.yMin);
            vertices[4] = rect.min;
            Handles.color = settings.LineColor;
            HandleUI.DrawLines(vertices, settings.DefaultLineThickness);
            if (isEditting)
            {
                int index = GetIndex();
                for (int i = 0; i < vertices.Length - 1; i++)
                {
                    mids[i] = 0.5f * (vertices[i] + vertices[i + 1]);
                    Handles.color = index == i ? settings.SelectedPointColor : settings.PointColor;
                    HandleUI.DrawDot(mids[i], SceneViewUtility.ScreenToWorldSize(settings.DefaultDotSize));
                }
            }
        }

        protected override void OnLeftMouseDown()
        {
            selectedIndex = GetIndex();
        }

        protected override void OnLeftMouseDrag()
        {
            if (IsSelecting)
            {
                Vector2 newPoint = ExternalTool.GetPointOnRay(mouseRay, 0f);
                Vector2 direction = directions[selectedIndex];
                Vector2 opposite = mids[(selectedIndex + 2) % 4];
                float projection = Vector2.Dot(newPoint - opposite, direction);
                if (projection < MinSize)
                    projection = MinSize;
                mids[selectedIndex] = opposite + projection * direction;
                offset.vector2Value = CalculateOffset(selectedIndex);
                size.vector2Value = CalculateSize();
            }
        }

        protected override int GetIndex()
        {
            if (HandleUtility.DistanceToPolyLine(vertices) < settings.HitLineDistance)
            {
                Vector3 closestPoint = HandleUtility.ClosestPointToPolyLine(vertices);
                for (int i = 0; i < vertices.Length - 1; i++)
                {
                    if (ExternalTool.Parallel(closestPoint - vertices[i], vertices[i + 1] - closestPoint))
                        return i;
                }
            }
            return -1;
        }

        protected Vector2 CalculateSize()
        {
            return mids[2] - mids[0] + mids[1] - mids[3];
        }

        protected Vector2 CalculateOffset(int modifiedIndex)
        {
            return 0.5f * (mids[modifiedIndex] + mids[(modifiedIndex + 2) % 4]) - rectEditor.Position2D;
        }

        protected override void MatchSprite(Sprite sprite)
        {
            Rect rect = EditorExtendUtility.GetSpriteAABB(sprite);
            offset.vector2Value = rect.center;
            size.vector2Value = rect.size;
        }
    }
}