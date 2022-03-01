using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public Vector3 A { get; set; }
    public Vector3 B { get; set; }
    public Vector3 C { get; set; }

    public Rectangle BoundingBox
    {
        get
        {
            float left = Mathf.FloorToInt(Mathf.Min(A.x, B.x, C.x));
            float right = Mathf.CeilToInt(Mathf.Max(A.x, B.x, C.x));
            float down = Mathf.FloorToInt(Mathf.Min(A.y, B.y, C.y));
            float up = Mathf.CeilToInt(Mathf.Max(A.y, B.y, C.y));
            return new Rectangle(left,right,down,up);
        }
    }
    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        A = a; B = b; C = c;
    }

    public bool Contain(Vector3 point)
    {
        Vector3 v = MathTool.BarycentricCoordinates(A, B, C, point); 
        return v.x >= 0 && v.y >= 0 && v.z >= 0;
    }
}