using UnityEngine;

//������Ҫ����-��ʾ��UI������
[RequireComponent(typeof(CanvasGroup))]
public class MyCanvasGrounp : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    public CanvasGroup GetCanvasGroup()
        => canvasGroup;

    protected float alpha_default;

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
