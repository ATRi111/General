using UnityEditor;

public class MeshHelperIndirectEditor
{
    public string label;
    public bool foldout;

    protected SerializedProperty dirty, vertices, triangles;

    public virtual void Initialize(SerializedProperty serializedProperty, string label)
    {
        this.label = label;
        foldout = false;

        dirty = serializedProperty.FindPropertyRelative("dirty");
        vertices = serializedProperty.FindPropertyRelative("vertices");
        triangles = serializedProperty.FindPropertyRelative("triangles");
    }

    public virtual void OnInspectorGUI()
    {
        foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, label);
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (foldout)
            MyInspectorGUI();
    }

    public virtual void MyInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.indentLevel++;
        dirty.boolValue = EditorGUILayout.Toggle("dirty", dirty.boolValue);
        EditorGUILayout.PropertyField(vertices, true);
        EditorGUILayout.PropertyField(triangles, true);
        EditorGUI.indentLevel--;

        EditorGUI.EndDisabledGroup();
    }
}
