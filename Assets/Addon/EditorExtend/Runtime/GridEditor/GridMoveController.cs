using EditorExtend.GridEditor;
using MyTimer;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridObject))]
public class GridMoveController : MonoBehaviour
{
    protected GridObject gridObject;
    protected GridManagerBase gridManager;

    public float defaultSpeed = 2f;
    [SerializeField]
    protected Vector3[] currentRoute;

    public UniformFoldLineMotion ufm;
    public Action AfterMove;

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public void SetRoute(List<Vector3> route, float speed = -1)
    {
        if (route.Count < 2)
            return;
        if (speed <= 0)
            speed = defaultSpeed;

        currentRoute = new Vector3[route.Count];
        currentRoute[0] = gridManager.CellToWorld(route[0]);
        float length = 0;
        for (int i = 1; i < route.Count; i++)
        {
            currentRoute[i] = gridManager.CellToWorld(route[i]);
            length += (currentRoute[i] - currentRoute[i - 1]).magnitude;
        }
        ufm.Initialize(currentRoute, length, length / speed);
    }

    public void ForceComplete()
    {
        ufm.ForceComplete();
    }

    protected virtual void OnTick(Vector3 v)
    {
        Position = v;
    }

    protected virtual void AfterComplete(Vector3 v)
    {
        Position = v;
        gridObject.AlignXY();
        AfterMove?.Invoke();
    }

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        gridManager = GetComponentInParent<GridManagerBase>();

        ufm = new UniformFoldLineMotion();
        ufm.OnTick += OnTick;
        ufm.AfterComplete += AfterComplete;
    }
}
