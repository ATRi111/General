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
                sb.AppendLine($"�����Ϸ����:{nameof(ServiceLocator)}");
            }
            else
            {
                sb.AppendLine($"��Ϸ�����Ѵ���:{nameof(ServiceLocator)}");
            }

            obj = GameObject.Find(nameof(GameLauncher));
            if (obj == null)
            {
                obj = new GameObject(nameof(GameLauncher));
                sb.AppendLine($"�����Ϸ����:{nameof(GameLauncher)}");
            }
            else
            {
                sb.AppendLine($"��Ϸ�����Ѵ���:{nameof(GameLauncher)}");
            }

            if (obj.GetComponent<GameLauncher>() == null)
            {
                obj.AddComponent<GameLauncher>();
                sb.AppendLine($"������:{nameof(GameLauncher)}");
            }
            else
            {
                sb.AppendLine($"����Ѵ���:{nameof(GameLauncher)}");
            }
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        public static void InitAllService()
        {
            HashSet<Type> targets = new HashSet<Type>();
            string[] guids = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Addon/Service" });

            Debugger.settings.Copy();
            Debugger.settings.SetAllowLog(EMessageType.System, false);

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

            Debugger.settings.Paste();

            sb.AppendLine("���ֲ�����ӵ�Service����:");
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