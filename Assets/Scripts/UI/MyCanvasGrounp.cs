using UnityEngine;

//挂在需要隐藏-显示的UI物体上
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
