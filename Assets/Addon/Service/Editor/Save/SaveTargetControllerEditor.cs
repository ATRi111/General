using EditorExtend;
using UnityEditor;

namespace Services.Save
{
    [CustomEditor(typeof(SaveTargetController),true)]
    public class SaveTargetControllerEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty obj, groupId, eIdentifier, customizedIdentifier;

        protected override void MyOnInspectorGUI()
        {
            obj.PropertyField("游戏对象");
            groupId.IntPopUp("存档组", SaveGroupControllerEditor.GroupNames, SaveGroupControllerEditor.OptionValues);
            eIdentifier.EnumField<EIdentifier>("标识符类型");
            if (eIdentifier.intValue == (int)EIdentifier.Customized)
            {
                customizedIdentifier.TextField("标识符");
            }
        }
    }
}