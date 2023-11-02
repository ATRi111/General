using Services;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EditorExtend
{
    public static class ScriptCreator
    {
        [MenuItem("Tools/EditorExtend/CreateEditorScript ^E")]
        public static void CreateEditorScript()
        {
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

            int count = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is MonoScript)
                {
                    EditorExtendUtility.DevideAssetPath(AssetDatabase.GetAssetPath(objects[0]), out string folder, out string file, out string extend);
                    string path = folder + file + "Editor" + extend;
                    string code = GenerateCode(file);
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

        public static string GenerateCode(string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using EditorExtend;\n" +
                "using UnityEditor;\n" +
                "using UnityEngine;\n\n");
            sb.Append($"[CustomEditor(typeof({className}))]\n" +
                $"public class {className}Editor : AutoEditor\n" +
                "{\n" +
                "   \n" +
                "}\n");
            return sb.ToString();
        }
    }
}