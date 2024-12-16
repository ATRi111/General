using EditorExtend;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Services.Save
{
    [CustomEditor(typeof(SaveTargetController),true)]
    public class SaveTargetControllerEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty obj, saveDataType, groupId, eIdentifier, customIdentifier;
        private List<Type> searchResult;

        protected override void OnEnable()
        {
            base.OnEnable();
            searchResult = new List<Type>();
            EditorExtendUtility.FindAllScriptInherit(typeof(SaveData), searchResult);
        }

        protected override void MyOnInspectorGUI()
        {
            saveDataType.TextField("存档数据的类名");
            for (int i = 0; i < searchResult.Count; i++)
            {
                Type type = searchResult[i];
                if (type.ToString().Contains(saveDataType.stringValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    string name = type.ToString();
                    if (GUILayout.Button(name))
                    {
                        saveDataType.stringValue = searchResult[i].Name;
                        break;
                    }
                }
            }
            obj.PropertyField("游戏对象");
            groupId.IntPopUp("存档组", SaveGroupControllerEditor.GroupNames, SaveGroupControllerEditor.OptionValues);
            eIdentifier.EnumField<EIdentifier>("标识符类型");
            if (eIdentifier.intValue == (int)EIdentifier.Custom)
            {
                customIdentifier.TextField("标识符");
            }
        }
    }
}