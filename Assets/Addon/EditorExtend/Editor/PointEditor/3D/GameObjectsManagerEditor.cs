using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EditorExtend.PointEditor
{
    [CustomEditor(typeof(GameObjectsManager))]
    public class GameObjectsManagerEditor : Editor_PointEditor
    {
        [AutoProperty]
        protected SerializedProperty gameObjects, prefab;
        protected GameObjectsManager manager;
        protected GameObject[] GameObjects => manager.gameObjects;

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
            manager = target as GameObjectsManager;
            helpInfo = "右击删除一个子物体";
            isEditting = true;
            UnityEditor.Tools.current = Tool.None;
        }

        protected override void MyOnInspectorGUI()
        {
            string s = isEditting ? "整体移动" : "结束整体移动";
            if (GUILayout.Button(s))
            {
                isEditting = !isEditting;
                UnityEditor.Tools.current = isEditting ? Tool.None : Tool.Move;
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
            if (isEditting && !string.IsNullOrEmpty(helpInfo))
                EditorGUILayout.HelpBox(helpInfo, MessageType.Info);
            prefab.PropertyField("Prefab");
            gameObjects.ListField("Children");
        }

        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            manager.RefreshGameObjects();
            if (isEditting)
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

        protected override void OnLeftMouseDown()
        {
            isDragging = true;
        }

        protected override void OnLeftMouseUp()
        {
            isDragging = false;
        }

        protected override void OnRightMouseDown()
        {
            if (IsSelecting)
                DeleteObject(SelectedGameObject);
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