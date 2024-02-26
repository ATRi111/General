using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Services
{
    internal static class EditorModeServiceInitializer
    {
        [MenuItem("Tools/Service/InitializeService")]
        public static void ComfirmInit()
        {
            InitServiceConfirmWindow window = EditorWindow.GetWindow<InitServiceConfirmWindow>("Confirm");
            window.callBack += Init;
            window.Show();
        }

        private static StringBuilder sb;

        public static void Init()
        {
            sb = new StringBuilder();
            InitBaseObject();
            InitAllService();
            Debugger.Log(sb, EMessageType.System);
        }

        public static void InitBaseObject()
        {
            GameObject obj = GameObject.Find(nameof(ServiceLocator));
            if (obj == null)
            {
                obj = new GameObject(nameof(ServiceLocator));
                obj.AddComponent<DontDestroy>();
                sb.AppendLine($"添加游戏物体:{nameof(ServiceLocator)}");
            }
            else
            {
                sb.AppendLine($"游戏物体已存在:{nameof(ServiceLocator)}");
            }

            obj = GameObject.Find(nameof(GameLauncher));
            if (obj == null)
            {
                obj = new GameObject(nameof(GameLauncher));
                sb.AppendLine($"添加游戏物体:{nameof(GameLauncher)}");
            }
            else
            {
                sb.AppendLine($"游戏物体已存在:{nameof(GameLauncher)}");
            }

            if (obj.GetComponent<GameLauncher>() == null)
            {
                obj.AddComponent<GameLauncher>();
                sb.AppendLine($"添加组件:{nameof(GameLauncher)}");
            }
            else
            {
                sb.AppendLine($"组件已存在:{nameof(GameLauncher)}");
            }
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        public static void InitAllService()
        {
            HashSet<Type> targets = new HashSet<Type>();
            string[] guids = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Addon/Service" });

            Debugger.Settings.Copy();
            Debugger.Settings.SetAllowLog(EMessageType.System, false);

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                MonoScript mono = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                Type type = mono.GetClass();
                if (type != null)
                {
                    Type target = IService.GetSubInterfaceOfIService(type);
                    if (target != null)
                        targets.Add(target);
                }
            }
            Transform parent = GameObject.Find(nameof(ServiceLocator)).transform;

            Debugger.Settings.Paste();

            sb.AppendLine("发现并新添加的Service类型:");
            foreach (Type type in targets)
            {
                InitTargetdService(type, parent);
            }
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        public static void InitTargetdService(Type target, Transform parent)
        {
            string original = target.ToString();
            string name = original[(original.LastIndexOf('.') + 2)..];
            GameObject obj = GameObject.Find(name);
            if (obj == null)
            {
                obj = new GameObject(name);
                InitService init = obj.AddComponent<InitService>();
                init.search = name;
                obj.transform.SetParent(parent);
                sb.AppendLine(name);
            }
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
}