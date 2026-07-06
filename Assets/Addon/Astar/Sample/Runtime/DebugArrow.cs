using UnityEngine;

namespace AStar.Sample
{
    /// <summary>
    /// 调试用箭头：从起点指向终点，箭头尖端在终点一侧。
    /// 箭杆用 <see cref="LineRenderer"/> 画一条细线（不包含箭头部分），箭头用程序生成的实心三角形 Mesh 画出，
    /// 两者共用同一张支持顶点色的材质（Sprites/Default），避免像纯 LineRenderer 折线画箭头那样因为线宽在
    /// 折角处重叠、箭头是"空心V字"而显得粗糙不规整。
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class DebugArrow : MonoBehaviour
    {
        private const float ShaftWidth = 0.025f;
        private const float HeadLength = 0.15f;
        private const float HeadWidth = 0.09f;

        private static Material sharedMaterial;
        private static Material SharedMaterial => sharedMaterial ??= new Material(Shader.Find("Sprites/Default"));

        private LineRenderer lineRenderer;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Mesh headMesh;

        /// <summary>
        /// 创建一个箭头
        /// </summary>
        /// <param name="from">箭头起点（尾部）</param>
        /// <param name="to">箭头终点（箭头尖端指向这里）</param>
        /// <param name="color">箭头颜色</param>
        /// <param name="parent">挂载的父物体</param>
        /// <param name="sortingOrder">渲染排序，值越大越晚绘制（覆盖在其他内容之上）</param>
        public static DebugArrow Create(Vector3 from, Vector3 to, Color color, Transform parent, int sortingOrder)
        {
            GameObject obj = new("Arrow");
            obj.transform.SetParent(parent);
            DebugArrow arrow = obj.AddComponent<DebugArrow>();
            arrow.Initialize(from, to, color, sortingOrder);
            return arrow;
        }

        private void Setup()
        {
            if (lineRenderer != null)
                return;

            lineRenderer = GetComponent<LineRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();

            headMesh = new Mesh { name = "ArrowHead" };
            meshFilter.mesh = headMesh;

            lineRenderer.useWorldSpace = true;
            lineRenderer.material = SharedMaterial;
            lineRenderer.widthMultiplier = ShaftWidth;
            lineRenderer.numCapVertices = 0;
            lineRenderer.numCornerVertices = 0;
            lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lineRenderer.receiveShadows = false;
            lineRenderer.alignment = LineAlignment.View;

            meshRenderer.sharedMaterial = SharedMaterial;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
        }

        public void Initialize(Vector3 from, Vector3 to, Color color, int sortingOrder)
        {
            Setup();

            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.sortingOrder = sortingOrder;
            meshRenderer.sortingOrder = sortingOrder;

            Vector3 delta = to - from;
            float length = delta.magnitude;
            if (length < 0.0001f)
            {
                lineRenderer.positionCount = 0;
                headMesh.Clear();
                return;
            }

            Vector3 dir = delta / length;
            Vector3 perp = new(-dir.y, dir.x, 0f);

            float headLength = Mathf.Min(HeadLength, length * 0.6f);
            Vector3 headBase = to - dir * headLength;
            Vector3 headLeft = headBase + perp * (HeadWidth * 0.5f);
            Vector3 headRight = headBase - perp * (HeadWidth * 0.5f);

            // 箭杆：只画到箭头底边，不与箭头部分重叠，线宽保持均匀
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, from);
            lineRenderer.SetPosition(1, headBase);

            // 箭头：实心三角形，顶点色与箭杆颜色一致
            headMesh.Clear();
            headMesh.vertices = new[] { to, headLeft, headRight };
            headMesh.triangles = new[] { 0, 1, 2 };
            headMesh.colors = new[] { color, color, color };
            headMesh.RecalculateBounds();
        }
    }
}
