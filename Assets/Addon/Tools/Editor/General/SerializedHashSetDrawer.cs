using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace MyTool
{
    [CustomPropertyDrawer(typeof(SerializedHashSetBase), true)]
    public class SerializedHashSetDrawer : AutoPropertyDrawer
    {
        public override bool NoLabel => true;

        [AutoProperty]
        public SerializedProperty list;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AutoPropertyField(label.text, list);
        }
    }
}