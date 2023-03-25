using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MeshExtend
{
    [CustomEditor(typeof(MyImage))]
    public class MyImageEditor : ImageEditor
    {
        protected SerializedProperty custom;
        protected SerializedProperty helper;
        protected MeshHelperIndirectEditor indirectEditor;

        protected SerializedProperty FillMethod;
        protected SerializedProperty FillOrigin;
        protected SerializedProperty FillAmount;
        protected SerializedProperty FillClockwise;
        protected SerializedProperty Type;
        protected SerializedProperty FillCenter;
        protected SerializedProperty Sprite;
        protected SerializedProperty PreserveAspect;
        protected SerializedProperty UseSpriteMesh;
        protected SerializedProperty PixelsPerUnitMultiplier;

        protected GUIContent SpriteContent;
        protected GUIContent SpriteTypeContent;
        protected GUIContent ClockwiseContent;

        protected override void OnEnable()
        {
            base.OnEnable();
            SpriteContent = EditorGUIUtility.TrTextContent("Source Image");
            SpriteTypeContent = EditorGUIUtility.TrTextContent("Image Type");
            ClockwiseContent = EditorGUIUtility.TrTextContent("Clockwise");
            Sprite = serializedObject.FindProperty("m_Sprite");
            Type = serializedObject.FindProperty("m_Type");
            FillCenter = serializedObject.FindProperty("m_FillCenter");
            FillMethod = serializedObject.FindProperty("m_FillMethod");
            FillOrigin = serializedObject.FindProperty("m_FillOrigin");
            FillClockwise = serializedObject.FindProperty("m_FillClockwise");
            FillAmount = serializedObject.FindProperty("m_FillAmount");
            PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");
            UseSpriteMesh = serializedObject.FindProperty("m_UseSpriteMesh");
            PixelsPerUnitMultiplier = serializedObject.FindProperty("m_PixelsPerUnitMultiplier");
            custom = serializedObject.FindProperty("custom");
            helper = serializedObject.FindProperty("helper");
            indirectEditor = new MeshHelperIndirectEditor();
            indirectEditor.Initialize(helper, "Mesh");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SpriteGUI();
            AppearanceControlsGUI();
            RaycastControlsGUI();
            MaskableControlsGUI();

            if (!Application.isPlaying)
                custom.boolValue = EditorGUILayout.Toggle("Custom", custom.boolValue);

            if (!custom.boolValue)
                TypeGUI();

            SetShowNativeSize(false);

            if (!custom.boolValue)
            {
                EditorGUI.indentLevel++;
                if (Type.enumValueIndex == 0)
                {
                    EditorGUILayout.PropertyField(UseSpriteMesh);
                }

                EditorGUILayout.PropertyField(PreserveAspect);
                EditorGUI.indentLevel--;
            }

            NativeSizeButtonGUI();
            serializedObject.ApplyModifiedProperties();
            MeshHelper2D temp = (serializedObject.targetObject as MyImage).helper;
            if (Application.isPlaying && temp.VertexCount < 100 && temp.TriangleCount < 100)
                indirectEditor.OnInspectorGUI();
        }

        protected void SetShowNativeSize(bool instant)
        {
            Image.Type enumValueIndex = (Image.Type)Type.enumValueIndex;
            bool show = (enumValueIndex == Image.Type.Simple || enumValueIndex == Image.Type.Filled) && Sprite.objectReferenceValue != null;
            SetShowNativeSize(show, instant);
        }
    }
}