using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableGameObject : MonoBehaviour
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
        mousePosition = inputManager.MouseWorldPosition(transform.position);
        b_dragged = true;
    }

    protected virtual void OnMouseUp()
    {
        b_dragged = false;
    }

    protected virtual void OnMouseDrag()
    {
        transform.position += inputManager.MouseWorldPosition(transform.position) - mousePosition;
        mousePosition = inputManager.MouseWorldPosition(transform.position);
    }
}
