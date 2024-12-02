using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace Character
{
    [CustomPropertyDrawer(typeof(PropertyModifier))]
    public class PropertyModifierDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty value, so, timing;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            static bool Percent(int timing)
            {
                return timing == (int)EModifyTiming.DirectMultiply || timing == (int)EModifyTiming.FinalMultiply;
            }

            int temp = timing.enumValueIndex;
            AutoPropertyField("��������", so);
            timing.EnumField<EModifyTiming>("�޸ķ�ʽ", NextRectRelative());

            if(Percent(temp) && !Percent(timing.enumValueIndex))
            {
                value.floatValue *= 100f;
            }
            else if(!Percent(temp) && Percent(timing.enumValueIndex))
            {
                value.floatValue /= 100f;
            }

            switch((EModifyTiming)timing.enumValueIndex)
            {
                case EModifyTiming.DirectAdd:
                case EModifyTiming.FinalAdd:
                    value.FloatField("�仯��", NextRectRelative());
                    break;
                case EModifyTiming.DirectMultiply:
                case EModifyTiming.FinalMultiply:
                    value.floatValue = EditorGUI.FloatField(NextRectRelative(), "�仯�ٷֱ�", value.floatValue * 100f) / 100f;
                    break;
            }
        }
    }
}