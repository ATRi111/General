using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.TwoD
{
    [CustomPropertyDrawer(typeof(PathFinding2DProcess))]
    public class PathFinding2DProcessDrawer : PathFindingProcessDrawer
    {
        [AutoProperty]
        public SerializedProperty settings, boundaryMin, boundaryMax;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AutoPropertyField("设置", settings);
            base.MyOnGUI(position, property, label);
            if(useBoundary.boolValue)
            {
                boundaryMin.Vector2IntField("边界最小值", NextRectRelative());
                boundaryMax.Vector2IntField("边界最大值", NextRectRelative());
            }
        }
    }
}
