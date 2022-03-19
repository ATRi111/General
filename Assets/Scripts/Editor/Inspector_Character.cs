using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Character))]
public class Inspector_Character : Editor
{
    private Character character;

    private void OnEnable()
    {
        character = (Character)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginVertical();
        GUILayout.Label("ÉúÃü");
    }
}
