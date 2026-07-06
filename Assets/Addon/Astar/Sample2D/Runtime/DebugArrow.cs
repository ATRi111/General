using UnityEngine;

namespace AStar.Sample
{
    /// <summary>
    /// 调试用箭头：从起点指向终点，箭头尖端在终点一侧，支持任意3D方向（不局限于XY平面）。
    /// 箭杆用 <see cref="LineRenderer"/> 画一条细线（不包含箭头部分），箭头用程序生成的实心四棱锥 Mesh 画出——
    /// 用四棱锥而不是单片三角形，是为了在3D场景中无论从哪个角度观察都能看到实心箭头，不会因为视线正好与三角形共面而"消失"。
    /// 两者共用同一张支持顶点色的材质（Sprites/Default），该材质默认双面渲染（Cull Off），无需关心三角形绕序。
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class DebugArrow : MonoBehaviour
    {
        private const float ShaftWidth = 0.05f;
        private const float HeadLength = 0.25f;
        private const float HeadWidth = 0.15f;

        private static Material sharedMaterial;
        private static Material SharedMaterial
        {
            get
            {
                if(sharedMaterial == null)
                    sharedMaterial = new Material(Shader.Find("Sprites/Default"));
                return sharedMaterial;
            }
        }

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
            // 用叉乘求与dir垂直的一对基向量，兼容任意3D方向（2D场景下dir.z恒为0，同样适用）；
            // 当dir几乎与参考向量平行时（例如竖直方向的箭头）切换参考向量，避免叉乘退化为零向量
            Vector3 reference = Mathf.Abs(Vector3.Dot(dir, Vector3.up)) > 0.99f ? Vector3.forward : Vector3.up;
            Vector3 right = Vector3.Cross(dir, reference).normalized;
            Vector3 up = Vector3.Cross(right, dir).normalized;

            float headLength = Mathf.Min(HeadLength, length * 0.6f);
            Vector3 headBase = to - dir * headLength;
            float radius = HeadWidth * 0.5f;
            Vector3 p0 = headBase + right * radius;
            Vector3 p1 = headBase + up * radius;
            Vector3 p2 = headBase - right * radius;
            Vector3 p3 = headBase - up * radius;

            // 箭杆：只画到箭头底边，不与箭头部分重叠，线宽保持均匀
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, from);
            lineRenderer.SetPosition(1, headBase);

            // 箭头：以 to 为尖端、底面为正方形的四棱锥（而非单片三角形），
            // 保证在3D场景中无论从哪个角度看都能看到实心的箭头，不会因为视角与三角形共面而"消失"
            headMesh.Clear();
            headMesh.vertices = new[] { to, p0, p1, p2, p3 };
            headMesh.triangles = new[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 1,
                1, 3, 2,
                1, 4, 3,
            };
            headMesh.colors = new[] { color, color, color, color, color };
            headMesh.RecalculateBounds();
        }
    }
}
