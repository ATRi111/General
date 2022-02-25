using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//主要用在需要隐藏-显示的UI物体上
[RequireComponent(typeof(CanvasGroup))]
public class MyUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public CanvasGroup GetCanvasGroup()
        => canvasGroup;

    private float alpha_default;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        alpha_default = canvasGroup.alpha;
    }

    public void SetVisibleAndActive(bool visible)
    {
        canvasGroup.alpha = visible ? alpha_default : 0;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}
