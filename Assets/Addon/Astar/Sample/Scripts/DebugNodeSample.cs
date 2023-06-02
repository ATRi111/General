using TMPro;
using UnityEngine;

namespace AStar
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
        private Color color_route;
        [SerializeField]
        private Color color_blank;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            textbox = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Initialize(PathNode node)
        {
            spriteRenderer.color = node.Type switch
            {
                ENodeType.Open => color_open,
                ENodeType.Close => color_close,
                ENodeType.Obstacle => color_obstacle,
                ENodeType.Route => color_route,
                _ => color_blank,
            };
            if (node.Type == ENodeType.Blank || node.Type == ENodeType.Obstacle)
                textbox.text = string.Empty;
            else
                textbox.text = $"G:{(int)node.GCost}\n" +
                    $"H:{(int)node.HCost}\n" +
                    $"F:{(int)node.FCost}\n";
        }
    }
}