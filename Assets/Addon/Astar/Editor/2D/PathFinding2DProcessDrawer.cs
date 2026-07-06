using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.TwoD
{
    [CustomPropertyDrawer(typeof(PathFinding2DProcess))]
    public class PathFinding2DProcessDrawer : PathFindingProcessDrawer
    {
        [AutoProperty]
        public SerializedProperty settings;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AutoPropertyField("设置", settings);
            base.MyOnGUI(position, property, label);
        }
    }
}
