using TMPro;
using UnityEngine;

namespace AStar.Sample
{
    public class DebugNodeSample : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private TextMeshProUGUI textbox;

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
            spriteRenderer = GetComponent<SpriteRenderer>();
            textbox = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Initialize(AStarNode node)
        {
            if (node.IsObstacle)
            {
                spriteRenderer.color = color_obstacle;
            }
            else if (node.process.output.Contains(node))
            {
                spriteRenderer.color = color_output;
            }
            else if (node.process.available.Contains(node))
            {
                spriteRenderer.color = color_available;
            }
            else
            {
                spriteRenderer.color = node.state switch
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