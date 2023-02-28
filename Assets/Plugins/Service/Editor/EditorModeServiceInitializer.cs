using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Services
{
    internal static class EditorModeServiceInitializer
    {
        [MenuItem("Tools/Service/InitializeService")]
        public static void CompleteInit()
        {
            InitBaseObject();
            InitAllService();
        }

        public static void InitBaseObject()
        {
            GameObject obj = GameObject.Find(nameof(ServiceLocator));
            if(obj == null)
                obj = new GameObject(nameof(ServiceLocator));

            obj = GameObject.Find(nameof(GameLauncher));
            if (obj == null)
                obj = new GameObject(nameof(GameLauncher));

            if(obj.GetComponent<GameLauncher>() == null)
                obj.AddComponent<GameLauncher>();
        }

        public static void InitAllService()
        {
            HashSet<Type> targets = new HashSet<Type>();
            string[] guids = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Plugins/Service" });

            Debugger.settings.Copy();
            Debugger.settings.SetAllowLog(EMessageType.System, false);

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                MonoScript mono = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                Type type = mono.GetClass();
                if(type != null)
                {
                    Type target = IService.GetSubInterfaceOfIService(type);
                    if (target != null)
                        targets.Add(target);
                }
            }
            Transform parent = GameObject.Find(nameof(ServiceLocator)).transform;

            Debugger.settings.Paste();

            foreach (Type type in targets)
            {
                InitTargetdService(type, parent);
            }
        }

        public static void InitTargetdService(Type target, Transform parent)
        {
            string original = target.ToString();
            string search = original[(original.LastIndexOf('.') + 2)..];
            GameObject obj = new GameObject(search);
            InitService init = obj.AddComponent<InitService>();
            init.search = search;

            obj.transform.SetParent(parent);
        }
    }
}