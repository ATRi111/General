using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DraggableGameObject2D : MonoBehaviour
{
    private InputManager inputManager;
    private Vector3 mousePosition;

    public bool b_dragged;

    protected virtual void Awake()
    {
        inputManager = ServiceLocator.GetService<InputManager>();
    }

    protected virtual void OnMouseDown()
    {
        mousePosition = inputManager.MouseWorldPosition2D();
        b_dragged = true;
    }

    protected virtual void OnMouseUp()
    {
        b_dragged = false;
    }

    protected virtual void OnMouseDrag()
    {
        transform.position += inputManager.MouseWorldPosition2D() - mousePosition;
        mousePosition = inputManager.MouseWorldPosition2D();
    }
}
