using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace Character
{
    [CustomPropertyDrawer(typeof(PropertyModifier))]
    public class PropertyModifierDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty value, so, bucket;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AutoPropertyField("作用属性", so);
            EModifierType type = ModifierBucket.BucketToType((EModifierBucket)bucket.enumValueIndex);

            bucket.EnumField<EModifierBucket>("作用区", NextRectRelative());
            switch (type)
            {
                case EModifierType.Add:
                    value.FloatField("增量", NextRectRelative());
                    break;
                case EModifierType.Multiply:
                    value.FloatField("增幅(小数)", NextRectRelative());
                    break;
            }
        }
    }
}