using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorExtend.PointEditor
{
    public abstract class Editor_PointEditor : AutoEditor
    {
        protected PointEditorSettings settings;

        /// <summary>
        /// 是否处于编辑状态；编辑状态下，才会响应各种鼠标事件
        /// </summary>
        protected bool isEditting;

        protected int selectedIndex;    //当前选中点的索引
        protected virtual bool IsSelecting => selectedIndex != -1;

        protected string helpInfo;

        /// <summary>
        /// 获取所给点中，到鼠标最近且小于给定距离的点的索引号；如果没有满足条件的点，返回-1
        /// </summary>
        protected int ClosestPointToMousePosition(List<Vector3> worldPoints, float maxDistance = float.MaxValue)
        {
            int ret = -1;
            float minDistance = maxDistance;
            float distance;
            for (int i = 0; i < worldPoints.Count; i++)
            {
                distance = DistanceToMousePosition(worldPoints[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    ret = i;
                }
            }
            return ret;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            isEditting = false;
            selectedIndex = -1;
            focusMode = EFocusMode.Default;
            settings = Resources.Load<PointEditorSettings>(nameof(PointEditorSettings));
        }

        protected override void MyOnInspectorGUI()
        {
            string s = isEditting ? "结束编辑" : "开始编辑";
            if (GUILayout.Button(s))
            {
                isEditting = !isEditting;
                if (isEditting)
                {
                    focusMode = EFocusMode.Lock;
                    UnityEditor.Tools.current = Tool.None;
                }
                else
                {
                    focusMode = EFocusMode.Default;
                    UnityEditor.Tools.current = Tool.Move;
                    selectedIndex = -1;
                }
                SceneView.RepaintAll();
            }
            if (isEditting && !string.IsNullOrEmpty(helpInfo))
                EditorGUILayout.HelpBox(helpInfo, MessageType.Info);
        }

        protected override void MyOnSceneGUI()
        {
            if (currentEvent.type == EventType.Repaint)
                Paint();
            if (isEditting)
            {
                switch (currentEvent.type)
                {
                    case EventType.MouseDown:
                        if (currentEvent.button == 0)
                            OnLeftMouseDown();
                        else if (currentEvent.button == 1)
                            OnRightMouseDown();
                        break;
                    case EventType.MouseDrag:
                        if (currentEvent.button == 0)
                            OnLeftMouseDrag();
                        break;
                    case EventType.MouseUp:
                        if (currentEvent.button == 0)
                            OnLeftMouseUp();
                        break;
                }
            }
        }

        protected virtual void OnLeftMouseDown() { }
        protected virtual void OnLeftMouseDrag() { }
        protected virtual void OnLeftMouseUp() { }
        protected virtual void OnRightMouseDown() { }

        /// <summary>
        /// 获取当前鼠标位置对应的点，如果不对应任何点，返回-1
        /// </summary>
        protected virtual int GetIndex()
        {
            return -1;
        }

        protected virtual void Paint() { }
    }
}