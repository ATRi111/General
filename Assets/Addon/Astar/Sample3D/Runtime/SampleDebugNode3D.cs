using AStar.ThreeD;
using TMPro;
using UnityEngine;

namespace AStar.Sample
{
    public class SampleDebugNode3D : MonoBehaviour
    {
        private Renderer cubeRenderer;
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

        private void Awake()
        {
            cubeRenderer = GetComponent<Renderer>();
            textbox = GetComponentInChildren<TextMeshPro>();
        }

        /// <summary>
        /// 3D场景中相机视角是任意的，文字需要每帧转向主摄像机才能一直保持可读
        /// </summary>
        private void LateUpdate()
        {
            if (textbox == null || Camera.main == null)
                return;

            Transform textTransform = textbox.transform;
            textTransform.rotation = Quaternion.LookRotation(textTransform.position - Camera.main.transform.position);
        }

        public void Initialize(Node3D node)
        {
            if (node.IsObstacle)
            {
                cubeRenderer.material.color = color_obstacle;
            }
            else if (node.process.output.Contains(node))
            {
                cubeRenderer.material.color = color_output;
            }
            else if (node.process.available.Contains(node))
            {
                cubeRenderer.material.color = color_available;
            }
            else
            {
                cubeRenderer.material.color = node.state switch
                {
                    ENodeState.Open => color_open,
                    ENodeState.Close => color_close,
                    _ => color_blank,
                };
            }

            if (node.state == ENodeState.Blank || node.IsObstacle)
                textbox.text = string.Empty;
            else
                textbox.text = $"G:{Mathf.RoundToInt(10 * node.GCost)}\n" +
                    $"H:{Mathf.RoundToInt(10 * node.HCost)}\n" +
                    $"F:{Mathf.RoundToInt(10 * node.WeightedFCost)}\n";
        }
    }
}
