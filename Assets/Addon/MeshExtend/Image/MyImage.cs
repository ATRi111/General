using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MeshExtend
{
    /// <summary>
    /// ���ڻ��ƾ���������״/uv/��ɫ��UIͼ��
    /// </summary>
    public class MyImage : Image
    {
        //ע�⣺��Ҫ��Awake�е���myImage.rectTransform.rect
        //MeshHelper2D�е�����Ӧʹ�ô˽ű����ڵ���Ϸ�����RectTransform����ϵ�µ�localPosition����Anchor�޹�
        public MeshHelper2D helper;

        [SerializeField]
        private bool custom;

        /// <summary>
        /// �Ƿ�Ӧ���Զ���mesh
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

        //ע�⣺RectTransform.position��UI��pivot����Ļ����
        /// <summary>
        /// ������ռ��ڵ��������������ת��ΪMyImage���������
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
        /// Ĭ�ϵļ�����ɫ�ķ���
        /// </summary>
        public Color DefaultGetColor(Vector3 _)
            => color;

        /// <summary>
        /// Ĭ�ϵļ���uv�ķ���
        /// </summary>
        public Vector2 DefaultGetUV(Vector3 v)
            => helper.DefaultGetUV(v);
    }
}
