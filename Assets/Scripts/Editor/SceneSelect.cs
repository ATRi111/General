#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneSelect
{
    [MenuItem("Tools/Scene0 _`")]
	public static void OpenScene0()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/0.unity");
	}
}
#endif