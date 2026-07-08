using AStar.ThreeD;
using TMPro;
using UnityEngine;

namespace AStar.Sample
{
    public class SampleDebugNode3D : MonoBehaviour
    {
        private Renderer sphereRenderer;
        private TextMeshPro textbox;

        [SerializeField]
        private Color color_open;
        [SerializeField]
        private Color color_close;
        [SerializeField]
        private Color color_obstacle;
        [SerializeField]
        private Color color_output;
        [SerializeField]
        private Color color_available;
        [SerializeField]
        private Color color_blank;

        /// <summary>
        /// 球体的整体透明度（0~1），统一应用在上面6种状态颜色之上——半透明便于观察被遮挡在球体背后/内部的其它节点，
        /// 或者球体本身正好嵌在障碍物方块内部时不会完全挡住方块。要真正显示出透明效果，球体材质本身必须支持alpha混合
        /// （见 <see cref="AStarSample3DBuilder.GetOrCreateDebugPrefab"/> 里用的是"Sprites/Default"而不是默认不透明材质）
        /// </summary>
        [SerializeField]
        private float alpha = 0.55f;

        private void Awake()
        {
            sphereRenderer = GetComponent<Renderer>();
            textbox = GetComponentInChildren<TextMeshPro>();
        }

        /// <summary>
        /// 3D场景中相机视角是任意的，文字需要每帧转向主摄像机才能一直保持可读；
        /// 文字原本挂在球心位置，容易被球体本身遮挡/与球面深度打架导致看不清，
        /// 这里额外把文字沿"球心→摄像机"方向挪到球体朝向摄像机一侧的表面外沿（略微外扩避免z-fighting），
        /// 效果类似一个贴着球面、随摄像机移动而绕球体表面移动的公告板
        /// </summary>
        private void LateUpdate()
        {
            if (textbox == null || Camera.main == null)
                return;

            Transform textTransform = textbox.transform;
            Vector3 camPos = Camera.main.transform.position;
            Vector3 center = transform.position;
            Vector3 direction = camPos - center;
            if (direction.sqrMagnitude > 0.0001f)
            {
                direction.Normalize();
                float radius = sphereRenderer.bounds.extents.x;
                textTransform.position = center + direction * (radius * 1.05f);
            }

            textTransform.rotation = Quaternion.LookRotation(textTransform.position - camPos);
        }

        public void Initialize(Node3D node)
        {
            Color color;
            if (node.IsObstacle)
            {
                color = color_obstacle;
            }
            else if (node.process.output.Contains(node))
            {
                color = color_output;
            }
            else if (node.process.available.Contains(node))
            {
                color = color_available;
            }
            else
            {
                color = node.state switch
                {
                    ENodeState.Open => color_open,
                    ENodeState.Close => color_close,
                    _ => color_blank,
                };
            }
            color.a = alpha;
            sphereRenderer.material.color = color;

            // UI拥挤看不清，只保留FCost（寻路排序实际依据的值），不再同时显示G/H
            if (node.state == ENodeState.Blank || node.IsObstacle)
                textbox.text = string.Empty;
            else
                textbox.text = $"{Mathf.RoundToInt(10 * node.WeightedFCost)}";
        }
    }
}
