using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyEditor.ShapeEditor
{
    [CustomEditor(typeof(PolygonEditor))]
    public class Editor_PolygonEditor : Editor_ShapeEditor
    {
        private ShapeEditorSettings settings;

        [Auto]
        public SerializedProperty localPoints, style;
        private PolygonEditor polygonEditor;
        private readonly List<Vector3> worldPoints = new List<Vector3>();

        protected override void OnEnable()
        {
            base.OnEnable();
            polygonEditor = target as PolygonEditor;
            settings = Resources.Load<ShapeEditorSettings>("ShapeEditorSettings");
        }

        protected override void MyOnInspectorGUI()
        {
            polygonEditor.GetWorldPoints(worldPoints);
            style.EnumField<EPolygonStyle>("style");
            localPoints.ListField("points");
            if (b_edit)
                EditorGUILayout.HelpBox("Right click to delete a point", MessageType.None);
            base.MyOnInspectorGUI();
        }

        protected override void Paint()
        {
            base.Paint();
            polygonEditor.GetWorldPoints(worldPoints);
            Handles.color = settings.LineColor;
            switch (polygonEditor.style)
            {
                case EPolygonStyle.PolyLine:
                    HandleUI.DrawLines(worldPoints, 2f, false);
                    break;
                case EPolygonStyle.ClosedPolyLine:
                    HandleUI.DrawLines(worldPoints, 2f, true);
                    break;
            }

            if (b_edit)
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
            if (selectedIndex == -1)
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
            if(localPoints.arraySize < 3)
            {
                Debug.LogWarning($"至少应当有两个点");
                return;
            }

            int delete = GetIndex();
            if (delete != -1)
                localPoints.DeleteArrayElementAtIndex(delete);
        }
        protected override void OnLeftMouseDrag()
        {
            if (selectedIndex >= 0)
            {
                localPoints.GetArrayElementAtIndex(selectedIndex).vector3Value =
                    ExternalTool.GetPointOnRay(mouseRay, worldPoints[selectedIndex]) - polygonEditor.transform.position;
            }
        }

        private int GetIndex()
        {
            Vector3 temp;
            for (int i = 0; i < worldPoints.Count; i++)
            {
                temp = worldPoints[i];
                if ((ExternalTool.GetPointOnRay(mouseRay, temp) - temp).sqrMagnitude < settings.HitPointSqrDistance)
                    return i;
            }
            return -1;
        }
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
            List<Vector2> outline = MyEditorUtility.GetSpritePhysicsShape(sprite);
            localPoints.ClearArray();
            for (int i = 0; i < outline.Count; i++)
            {
                localPoints.arraySize++;
                localPoints.GetArrayElementAtIndex(i).vector3Value = outline[i];
            }
        }
    }
}