using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.ThreeD
{
    [CustomPropertyDrawer(typeof(PathFinding3DProcess))]
    public class PathFinding3DProcessDrawer : PathFindingProcessDrawer
    {
        [AutoProperty]
        public SerializedProperty settings, boundaryMin, boundaryMax;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AutoPropertyField("设置", settings);
            base.MyOnGUI(position, property, label);
            if(useBoundary.boolValue)
            {
                boundaryMin.Vector3IntField("边界最小值", NextRectRelative());
                boundaryMax.Vector3IntField("边界最大值", NextRectRelative());
            }
        }
    }
}
