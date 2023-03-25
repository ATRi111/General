using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MeshExtend
{
    /// <summary>
    /// 用于绘制具有特殊形状/uv/颜色的UI图像
    /// </summary>
    public class MyImage : Image
    {
        //注意：不要在Awake中调用myImage.rectTransform.rect
        //MeshHelper2D中的坐标应使用此脚本所在的游戏物体的RectTransform坐标系下的localPosition，与Anchor无关
        public MeshHelper2D helper;

        [SerializeField]
        private bool custom;

        /// <summary>
        /// 是否应用自定义mesh
        /// </summary>
        public bool Custom
        {
            get => custom;
            set
            {
                if (custom != value)
                {
                    custom = value;
                    helper.dirty = true;
                    Repaint();
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            helper = new MeshHelper2D
            {
                GetRect = GetRect
            };
        }

        //注意：RectTransform.position是UI的pivot的屏幕坐标
        /// <summary>
        /// 将世界空间内的物体的世界坐标转换为MyImage顶点的坐标
        /// </summary>
        public Vector2 WorldToLocalPoint(Camera camera, Vector3 worldPoint)
        {
            Vector2 screenPoint = camera.WorldToScreenPoint(worldPoint);
            Vector2 localPoint = rectTransform.InverseTransformPoint(screenPoint);
            return localPoint;
        }

        public void Repaint()
        {
            if (helper.dirty)
            {
                SetAllDirty();
                helper.dirty = false;
            }
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (custom && helper != null && !helper.IsEmpty)
            {
                FillMesh(toFill);
            }
            else
                base.OnPopulateMesh(toFill);
        }

        protected virtual void FillMesh(VertexHelper toFill)
        {
            toFill.Clear();
            List<Vertex> vertices = helper.GetVertices();
            List<int> triangles = helper.GetTriangles();
            for (int i = 0; i < vertices.Count; i++)
            {
                toFill.AddVert(vertices[i].ToUIVertex());
            }
            for (int i = 0; i < triangles.Count; i += 3)
            {
                toFill.AddTriangle(triangles[i], triangles[i + 1], triangles[i + 2]);
            }
        }

        public Rect GetRect()
            => rectTransform.rect;
        /// <summary>
        /// 默认的计算颜色的方法
        /// </summary>
        public Color DefaultGetColor(Vector3 _)
            => color;

        /// <summary>
        /// 默认的计算uv的方法
        /// </summary>
        public Vector2 DefaultGetUV(Vector3 v)
            => helper.DefaultGetUV(v);
    }
}
