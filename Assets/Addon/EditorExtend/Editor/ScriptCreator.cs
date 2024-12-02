using Services;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace EditorExtend
{
    public static class ScriptCreator
    {
        [MenuItem("Tools/EditorExtend/CreateAutoEditorScript")]
        public static void CreateAutoEditorScript()
            => CreateEditorScript('e');
        [MenuItem("Tools/EditorExtend/CreateAutoPropertyDrawerScript")]
        public static void CreateAutoPropertyDrawerScript()
            => CreateEditorScript('d');

        public static void CreateEditorScript(char type)
        {
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

            int count = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is MonoScript)
                {
                    EditorExtendUtility.DivideAssetPath(AssetDatabase.GetAssetPath(objects[i]), out string directory, out string file, out string extend);
                    string suffix = type switch
                    {
                        'd' => "Drawer",
                        _ => "Editor",
                    };
                    string path = directory.Replace("Runtime", "Editor") + file + suffix + extend;
                    string code = type switch
                    {
                        'd' => GenerateAutoPropertyDrawerCode(file),
                        _ => GenerateAutoEditorCode(file),
                    };
                    try
                    {
                        FileInfo info = FileTool.GetFileInfo(path, true);
                        using StreamWriter writer = info.AppendText();
                        writer.Write(code);
                        count++;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            AssetDatabase.Refresh();
            Debug.Log($"已通过{objects.Length}个对象中的{count}个创建Editor脚本");
        }

        internal static string GenerateAutoEditorCode(string className)
        {
            StringBuilder sb = new();
            sb.Append("using EditorExtend;\n");
            sb.Append("using UnityEditor;\n");
            sb.Append("using UnityEngine;\n\n");

            sb.Append($"[CustomEditor(typeof({className}))]\n");
            sb.Append($"public class {className}Editor : AutoEditor\n");
            sb.Append("{\r\n");
            sb.Append("    [AutoProperty]\r\n");
            sb.Append("    public SerializedProperty data;\r\n\r\n");
            sb.Append("    protected override void MyOnInspectorGUI()\r\n");
            sb.Append("    {\r\n");
            sb.Append("        \r\n");
            sb.Append("    }\r\n");
            sb.Append("}");
            return sb.ToString();
        }

        internal static string GenerateAutoPropertyDrawerCode(string className)
        {
            StringBuilder sb = new();
            sb.Append("using EditorExtend;\n");
            sb.Append("using UnityEditor;\n");
            sb.Append("using UnityEngine;\n\n");

            sb.Append($"[CustomPropertyDrawer(typeof({className}))]\n");
            sb.Append($"public class {className}Drawer : AutoPropertyDrawer\n");
            sb.Append("{\r\n");
            sb.Append("    [AutoProperty]\r\n");
            sb.Append("    public SerializedProperty data;\r\n\r\n");
            sb.Append("    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)\r\n");
            sb.Append("    {\r\n");
            sb.Append("        \r\n");
            sb.Append("    }\r\n");
            sb.Append("}");
            return sb.ToString();
        }
    }
}