using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace Services.Save
{
    [CustomEditor(typeof(SaveGroupController))]
    public class SaveGroupControllerEditor : AutoEditor
    {
        private static SaveGroupSetting setting;
        public static SaveGroupSetting Setting
        {
            get
            {
                if (setting == null)
                    setting = Resources.Load<SaveGroupSetting>("SaveGroupSetting");
                return setting;
            }
        }

        public static string[] GroupNames => Setting.groupNames;

        public static int[] OptionValues
        {
            get
            {
                int[] ret = new int[Setting.groupNames.Length];
                for (int i = 0; i < ret.Length; i++)
                {
                    ret[i] = i;
                }
                return ret;
            }
        }

        [AutoProperty]
        public SerializedProperty readOnAwake, fileName, index, groupId;

        protected override void MyOnInspectorGUI()
        {
            readOnAwake.BoolField("Awake时便读存档文件");
            fileName.TextField("默认存档文件名");
            index.TextField("存档槽位");
            groupId.IntPopUp("存档组", GroupNames, OptionValues);
        }
    }
}