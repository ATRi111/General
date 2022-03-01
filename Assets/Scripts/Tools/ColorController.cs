using System;
using System.Collections;
using UnityEngine;

//脚本的enable值即表示是否在改变颜色
[RequireComponent(typeof(SpriteRenderer))]
public class ColorController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public ColorChange ColorChange { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (ColorChange.Completed)
        {
            spriteRenderer.color = ColorChange.Current;
            ColorChange.Timer += Time.fixedDeltaTime;
        }
        else
        {
            enabled = false;
        }
    }

    public void ChangeColor(Color targetColor, float duration)
    {
        ColorChange = new ColorChange(duration, spriteRenderer.color, targetColor);
    }

    public void Stop()
    {
        enabled = false;
    }
}
