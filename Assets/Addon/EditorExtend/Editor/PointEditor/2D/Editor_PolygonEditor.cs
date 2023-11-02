using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorExtend.PointEditor
{
    [CustomEditor(typeof(PolygonEditor))]
    public class Editor_PolygonEditor : Editor_PointEditor2D
    {
        [AutoProperty]
        public SerializedProperty localPoints, style;
        private PolygonEditor polygonEditor;
        private readonly List<Vector3> worldPoints = new List<Vector3>();

        protected override bool IsSelecting => selectedIndex > -1 && selectedIndex < worldPoints.Count;

        protected override void OnEnable()
        {
            base.OnEnable();
            polygonEditor = target as PolygonEditor;
            helpInfo = "左击添加一个点; \n右击删除一个点";
        }

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            EditorGUILayout.Space();
            style.EnumField<EPolygonStyle>("style");
            polygonEditor.GetWorldPoints(worldPoints);
            localPoints.ListField("points");
        }

        protected override void Paint()
        {
            base.Paint();
            polygonEditor.GetWorldPoints(worldPoints);
            Handles.color = settings.LineColor;
            switch (polygonEditor.style)
            {
                case EPolygonStyle.PolyLine:
                    HandleUI.DrawLines(worldPoints, settings.DefaultLineThickness, false);
                    break;
                case EPolygonStyle.ClosedPolyLine:
                    HandleUI.DrawLines(worldPoints, settings.DefaultLineThickness, true);
                    break;
            }

            if (isEditting)
            {
                Handles.color = settings.PointColor;
                int index = GetIndex();
                for (int i = 0; i < worldPoints.Count; i++)
                {
                    Handles.color = index == i ? settings.SelectedPointColor : settings.PointColor;
                    HandleUI.DrawDot(worldPoints[i], SceneViewUtility.ScreenToWorldSize(settings.DefaultDotSize));
                }

                if (index == -1)
                {
                    Handles.color = settings.NewPointColor;
                    int insert = GetPointIndexOnLine(out Vector3 closest);
                    if (insert != -1)
                        HandleUI.DrawDot(closest, SceneViewUtility.ScreenToWorldSize(settings.DefaultDotSize));
                }
            }
        }

        protected override void OnLeftMouseDown()
        {
            selectedIndex = GetIndex();

            if (!IsSelecting)
            {
                int insert = GetPointIndexOnLine(out Vector3 closest);
                if (insert != -1)
                {
                    localPoints.InsertArrayElementAtIndex(insert);
                    localPoints.GetArrayElementAtIndex(insert).vector3Value = closest - polygonEditor.transform.position;
                    selectedIndex = insert;
                }
            }
        }

        protected override void OnRightMouseDown()
        {
            if (localPoints.arraySize < 3)
            {
                Debug.LogWarning($"至少应当有两个点");
                return;
            }

            int index = GetIndex();
            if (index != -1)
                localPoints.DeleteArrayElementAtIndex(index);
        }

        protected override void OnLeftMouseDrag()
        {
            if (IsSelecting)
            {
                localPoints.GetArrayElementAtIndex(selectedIndex).vector3Value =
                    ExternalTool.GetPointOnRay(mouseRay, 0f) - polygonEditor.transform.position;
            }
        }

        protected override int GetIndex()
        {
            return ClosestPointToMousePosition(worldPoints, settings.HitPointDistance);
        }

        /// <summary>
        /// 获取鼠标位置到当前形状最近的点，以及为插入该点提供的索引
        /// </summary>
        private int GetPointIndexOnLine(out Vector3 closestPoint)
        {
            closestPoint = default;
            EPolygonStyle s = (EPolygonStyle)style.enumValueIndex;
            switch (s)
            {
                case EPolygonStyle.PolyLine:
                case EPolygonStyle.ClosedPolyLine:
                    List<Vector3> vertices = new List<Vector3>();
                    polygonEditor.GetWorldPoints(vertices);
                    if (vertices.Count > 0)
                    {
                        if (s == EPolygonStyle.ClosedPolyLine)
                            vertices.Add(vertices[0]);
                        if (HandleUtility.DistanceToPolyLine(vertices.ToArray()) < settings.HitLineDistance)
                        {
                            closestPoint = HandleUtility.ClosestPointToPolyLine(vertices.ToArray());
                            for (int i = 0; i < vertices.Count - 1; i++)
                            {
                                if (ExternalTool.Parallel(closestPoint - vertices[i], vertices[i + 1] - closestPoint))
                                    return (i + 1) % vertices.Count;
                            }
                        }
                    }
                    break;
            }
            return -1;
        }

        protected override void MatchSprite(Sprite sprite)
        {
            List<Vector2> outline = EditorExtendUtility.GetSpritePhysicsShape(sprite);
            localPoints.ClearArray();
            for (int i = 0; i < outline.Count; i++)
            {
                localPoints.arraySize++;
                localPoints.GetArrayElementAtIndex(i).vector3Value = outline[i];
            }
        }
    }
}