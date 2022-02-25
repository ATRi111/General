using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ҫ������Ҫ����-��ʾ��UI������
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
