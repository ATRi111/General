using EditorExtend;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Services.Save
{
    [CustomEditor(typeof(SaveTargetController), true)]
    public class SaveTargetControllerEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty obj, saveDataType, groupId, eIdentifier, customIdentifier;
        private List<string> classNames;

        protected override void OnEnable()
        {
            base.OnEnable();
            classNames = new();
            List<Type> searchResult = new();
            EditorExtendUtility.FindAllScriptInherit(typeof(SaveData), searchResult);
            for (int i = 0; i < searchResult.Count; i++)
            {
                classNames.Add(searchResult[i].ToString());
            }
        }

        protected override void MyOnInspectorGUI()
        {
            bool modified = saveDataType.TextFieldWithOptionButton("存档数据的类名", classNames);
            if (modified)
                Repaint();
            obj.PropertyField("游戏对象");
            groupId.IntPopField("存档组", SaveGroupControllerEditor.GroupNames, SaveGroupControllerEditor.OptionValues);
            eIdentifier.EnumField<EIdentifier>("标识符类型");
            if (eIdentifier.intValue == (int)EIdentifier.Custom)
            {
                customIdentifier.TextField("标识符");
            }
        }
    }
}