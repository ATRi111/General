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
        //���ڱ�ݵط���0����
        public static void OpenScene0()
        {
            EditorSceneManager.OpenScene(Scene0Path);
            SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(Scene0Path);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }

        [MenuItem("Tools/Service/Audio/CreateAudioSource #A")]
        //����������������AudioSource��Prefab
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
            Debug.Log($"��ͨ��{objects.Length}�������е�{count}��������ƵԤ����");
        }

        [MenuItem("Tools/File/OpenPersistantDataPath")]
        public static void OpenPersistantDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}