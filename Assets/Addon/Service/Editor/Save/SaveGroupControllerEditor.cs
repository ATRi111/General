using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace Services.Save
{
    [CustomEditor(typeof(SaveGroupController))]
    public class SaveGroupControllerEditor : AutoEditor
    {
        private static string[] _GroupNames;
        public static string[] GroupNames
        {
            get
            {
                if( _GroupNames == null)
                {
                    SaveGroupSetting setting = Resources.Load<SaveGroupSetting>("SaveGroupSetting");
                    _GroupNames = setting.groupNames;
                }
                return _GroupNames;
            }
        }

        private static int[] _OptionValues;
        public static int[] OptionValues
        {
            get
            {
                if( _OptionValues == null )
                {
                    _OptionValues = new int[GroupNames.Length];
                    for (int i = 0; i < OptionValues.Length; i++)
                    {
                        OptionValues[i] = i;
                    }
                }
                return _OptionValues;
            }
        }

        [AutoProperty]
        public SerializedProperty readOnAwake, fileName, groupId;

        protected override void MyOnInspectorGUI()
        {
            readOnAwake.BoolField("Awake时便读存档文件");
            fileName.TextField("存档文件名");
            groupId.IntPopUp("存档组", GroupNames, OptionValues);
        }
    }
}