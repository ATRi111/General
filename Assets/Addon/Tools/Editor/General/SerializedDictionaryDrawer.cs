using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace MyTool
{
    [CustomPropertyDrawer(typeof(SerializedDictionaryBase))]
    public class SerializedDictionaryDrawer : AutoPropertyDrawer
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