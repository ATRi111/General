using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorExtend.PointEditor
{
    public abstract class Editor_PointEditor : InteractiveEditor
    {
        protected PointEditorSettings settings;

        protected int selectedIndex;    //当前选中点的索引
        protected virtual bool IsSelecting => selectedIndex != -1;

        protected string helpInfo;

        protected override void OnEnable()
        {
            base.OnEnable();
            selectedIndex = -1;
            settings = Resources.Load<PointEditorSettings>(nameof(PointEditorSettings));
            editorModeOnly = true;
        }

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            if (!string.IsNullOrEmpty(helpInfo))
                EditorGUILayout.HelpBox(helpInfo, MessageType.Info);
        }

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

        /// <summary>
        /// 获取当前鼠标位置对应的点，如果不对应任何点，返回-1
        /// </summary>
        protected virtual int GetIndex()
        {
            return -1;
        }

        protected virtual void Select()
        {
            selectedIndex = GetIndex();
        }
    }
}