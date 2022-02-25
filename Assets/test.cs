using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    private float width;
    [SerializeField]
    private float height;
    [SerializeField]
    private SpriteScaleMode scaleMode;

    private void Start()
    {
        transform.localScale = SpriteTool.ScaleWithScreen(GetComponent<SpriteRenderer>(), width, height, scaleMode);
    }
}
