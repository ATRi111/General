using Services;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EditorExtend
{
    public static class ScriptCreator
    {
        [MenuItem("Tools/EditorExtend/CreateAutoEditorScript")]
        public static void CreateAutoEditorScript()
            => CreateEditorScript('a');
        [MenuItem("Tools/EditorExtend/CreateIndirectEditorScript")]
        public static void CreateIndirectEditorScript()
            => CreateEditorScript('i');

        public static void CreateEditorScript(char type)
        {
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

            int count = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is MonoScript)
                {
                    EditorExtendUtility.DevideAssetPath(AssetDatabase.GetAssetPath(objects[i]), out string folder, out string file, out string extend);
                    string path = type switch
                    {
                        'i' => "IndirectEditor",
                        _ => "Editor",
                    };
                    path = folder + file + path + extend;
                    string code = type switch
                    {
                        'i' => GenerateIndirectEditorCode(file),
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
            StringBuilder sb = new StringBuilder();
            sb.Append("using EditorExtend;\n" +
                "using UnityEditor;\n" +
                "using UnityEngine;\n\n");
            sb.Append($"[CustomEditor(typeof({className}))]\n" +
                $"public class {className}Editor : AutoEditor\n" +
                "{\r\n    " +
                "[AutoProperty]\r\n" +
                "    public SerializedProperty data;\r\n\r\n" +
                "    protected override void MyOnInspectorGUI()\r\n" +
                "    {\r\n" +
                "        \r\n" +
                "    }\r\n" +
                "}");
            return sb.ToString();
        }

        internal static string GenerateIndirectEditorCode(string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using EditorExtend;\n" +
                "using UnityEditor;\n" +
                "using UnityEngine;\n\n");
            sb.Append($"[CustomEditor(typeof({className}))]\n" +
                $"public class {className}IndirectEditor : IndirectEditor\n" +
                "{\r\n" +
                "    protected override string DefaultLabel => base.DefaultLabel;\r\n" +
                "    [AutoProperty]\r\n" +
                "    public SerializedProperty data;\r\n\r\n" +
                "    protected override void MyOnInspectorGUI()\r\n" +
                "    {\r\n" +
                "        \r\n" +
                "    }\r\n" +
                "}");
            return sb.ToString();
        }
    }
}