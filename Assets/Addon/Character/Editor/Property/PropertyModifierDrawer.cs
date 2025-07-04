using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace Character
{
    [CustomPropertyDrawer(typeof(PropertyModifier))]
    public class PropertyModifierDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty value, so, method;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            static bool Percent(int timing)
            {
                return timing == (int)EModifyMethod.DirectMultiply || timing == (int)EModifyMethod.FinalMultiply;
            }

            int temp = method.enumValueIndex;
            AutoPropertyField("��������", so);
            method.EnumField<EModifyMethod>("�޸ķ�ʽ", NextRectRelative());

            if (Percent(temp) && !Percent(method.enumValueIndex))
            {
                value.floatValue *= 100f;
            }
            else if (!Percent(temp) && Percent(method.enumValueIndex))
            {
                value.floatValue /= 100f;
            }

            switch ((EModifyMethod)method.enumValueIndex)
            {
                case EModifyMethod.DirectAdd:
                case EModifyMethod.FinalAdd:
                    value.FloatField("�仯��", NextRectRelative());
                    break;
                case EModifyMethod.DirectMultiply:
                case EModifyMethod.FinalMultiply:
                    value.floatValue = EditorGUI.FloatField(NextRectRelative(), "�仯�ٷֱ�", value.floatValue * 100f) / 100f;
                    break;
            }
        }
    }
}