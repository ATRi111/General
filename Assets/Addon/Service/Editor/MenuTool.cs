using Services.Audio;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Services
{
    public static class MenuTool
    {
        private readonly static AudioAssetSettings settings;

        static MenuTool()
        {
            settings = Resources.Load<AudioAssetSettings>(nameof(AudioAssetSettings));
        }

        private static readonly string Scene0Path = "Assets/Scenes/0.unity";
        [MenuItem("Tools/Scene/Scene0 _`")]
        //用于便捷地返回0场景
        public static void OpenScene0()
        {
            EditorSceneManager.OpenScene(Scene0Path);
            SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(Scene0Path);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }

        [MenuItem("Tools/Service/Audio/CreateAudioSource #A")]
        //用于批量创建带有AudioSource的Prefab
        public static void CreateAudioPrefab()
        {
            string outputPath = settings.AudioAssetPath;
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            int count = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                AudioClip clip = objects[i] as AudioClip;
                if (clip != null)
                {
                    GameObject obj = new GameObject(clip.name);

                    obj.AddComponent<AudioSource>();
                    AudioSource audioSource = obj.GetComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.playOnAwake = false;
                    try
                    {
                        PrefabUtility.SaveAsPrefabAsset(obj, outputPath + clip.name + ".prefab");
                        count++;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        Object.DestroyImmediate(obj);
                    }
                }
            }
            AssetDatabase.Refresh();
            Debug.Log($"已通过{objects.Length}个对象中的{count}个创建音频预制体");
        }

        [MenuItem("Tools/File/OpenPersistantDataPath")]
        public static void OpenPersistantDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}