using AStar;
using TMPro;
using UnityEngine;

public class DebugNode : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TextMeshProUGUI textbox;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(PathNode node)
    {
        spriteRenderer.color = node.Type switch
        {
            ENodeType.Open => Color.green,
            ENodeType.Close => Color.red,
            ENodeType.Block => Color.grey,
            ENodeType.Route => Color.blue,
            _ => Color.white,
        };
        textbox.text = $"G:{(int)node.GCost}\n" +
            $"H:{(int)node.HCost}\n" +
            $"F:{(int)node.FCost}\n";
    }
}
