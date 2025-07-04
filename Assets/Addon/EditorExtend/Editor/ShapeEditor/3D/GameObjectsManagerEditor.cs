using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EditorExtend.ShapeEditor
{
    [CustomEditor(typeof(GameObjectManager))]
    public class GameObjectsManagerEditor : ShapeEditor
    {
        [AutoProperty]
        protected SerializedProperty gameObjects, prefab;
        protected GameObjectManager manager;
        protected GameObject[] GameObjects => manager.gameObjects;
        protected bool isEditing;

        protected GameObject SelectedGameObject
        {
            get
            {
                if (IsSelecting)
                    return GameObjects[selectedIndex];
                return null;
            }
        }
        protected Vector3 SelectedPosition
        {
            get
            {
                if (IsSelecting)
                    return SelectedGameObject.transform.position;
                return default;
            }
            set
            {
                if (IsSelecting)
                    SelectedGameObject.transform.position = value;
            }
        }
        protected override bool IsSelecting => selectedIndex > -1 && selectedIndex < GameObjects.Length && GameObjects[selectedIndex] != null;

        protected bool isDragging;

        protected override void OnEnable()
        {
            base.OnEnable();
            manager = target as GameObjectManager;
            helpInfo = "右击删除一个子物体";
            isEditing = true;
            Tools.current = Tool.None;
        }

        protected override void MyOnInspectorGUI()
        {
            manager.RefreshGameObjects();
            string s = isEditing ? "整体移动" : "结束整体移动";
            if (GUILayout.Button(s))
            {
                isEditing = !isEditing;
                Tools.current = isEditing ? Tool.None : Tool.Move;
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("在开头添加子物体"))
            {
                CreateObject(true);
            }
            if (GUILayout.Button("在末尾添加子物体"))
            {
                CreateObject(false);
            }
            if (GUILayout.Button("自动重命名子物体"))
            {
                RenameAllObjects();
            }
            if (isEditing && !string.IsNullOrEmpty(helpInfo))
                EditorGUILayout.HelpBox(helpInfo, MessageType.Info);
            prefab.PropertyField("Prefab");
            gameObjects.ListField("Children");
        }

        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            manager.RefreshGameObjects();
            if (isEditing)
            {
                if (!isDragging)
                    selectedIndex = GetIndex(); //避免拖动时经过其他点时选中点改变
                MoveObject();
            }
            else
            {
                isDragging = false;
                selectedIndex = -1;
            }
        }

        protected override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            switch (button)
            {
                case 0:
                    isDragging = true;
                    break;
                case 1:
                    if (IsSelecting)
                        DeleteObject(SelectedGameObject);
                    break;
            }
        }

        protected override void OnMouseUp(int button)
        {
            base.OnMouseUp(button);
            switch (button)
            {
                case 0:
                    isDragging = false;
                    break;
            }
        }

        protected virtual bool MoveObject()
        {
            if (IsSelecting)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.PositionHandle(SelectedPosition, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    SelectedGameObject.transform.position = newPosition;
                    EditorSceneManager.MarkSceneDirty(SelectedGameObject.scene);
                    return true;
                }
            }
            return false;
        }

        protected virtual GameObject CreateObject(bool first)
        {
            GameObject obj = PrefabUtility.InstantiatePrefab(manager.prefab) as GameObject;
            Undo.RegisterCreatedObjectUndo(obj, "CreateGameobject");
            obj.name = CreateName();
            obj.transform.SetParent(manager.transform);
            obj.transform.localPosition = Vector3.zero;
            if (first)
                obj.transform.SetAsFirstSibling();
            manager.RefreshGameObjects();
            return obj;
        }

        protected virtual void DeleteObject(GameObject gameObject)
        {
            Undo.DestroyObjectImmediate(gameObject);
            manager.RefreshGameObjects();
        }

        protected virtual string CreateName()
        {
            string origin = manager.prefab.name;
            int maxIndex = -1;
            for (int i = 0; i < GameObjects.Length; i++)
            {
                string[] temp = GameObjects[i].name.Split('-');
                if (temp.Length == 2)
                {
                    if (int.TryParse(temp[1], out int index))
                    {
                        maxIndex = Mathf.Max(maxIndex, index);
                    }
                }
            }
            return $"{origin}-{maxIndex + 1}";
        }

        public virtual void RenameAllObjects()
        {
            Undo.RecordObjects(GameObjects, "RenameAllObjects");
            for (int i = 0; i < GameObjects.Length; i++)
            {
                GameObjects[i].name = $"{manager.prefab.name}-{i}";
            }
        }

        protected override int GetIndex()
        {
            List<Vector3> worldPoints = new List<Vector3>();
            manager.GetWorldPoints(worldPoints);

            int ret = ClosestPointToMousePosition(worldPoints, settings.HitObjectDistance);

            if (ret != -1)
                return ret;

            if (IsSelecting && DistanceToMousePosition(SelectedPosition) < settings.ContainHitObjectDistance)
                ret = selectedIndex;
            return ret;
        }
    }
}