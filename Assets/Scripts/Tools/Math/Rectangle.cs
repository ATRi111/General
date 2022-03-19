using UnityEngine;

public class Rectangle
{
    public Vector3 LeftDown { get; set; }
    public Vector3 RightDown { get; set; }
    public Vector3 LeftUp { get; set; }
    public Vector3 RightUp { get; set; }

    public float Left => LeftDown.x;
    public float Right => RightDown.x;
    public float Down => LeftDown.y;
    public float Up => LeftUp.y;

    public Vector3 Mid => (LeftDown + RightUp) / 2;
    public float Length => Right - Left;
    public float Width => Up - Down;

    public Rectangle()
    {
        LeftDown = RightDown = LeftUp = RightUp = Vector3.zero;
    }
    //输入对角线上两点
    public Rectangle(Vector3 point1, Vector3 point2)
    {
        float left = Mathf.Min(point1.x, point2.x);
        float right = Mathf.Max(point1.x, point2.x);
        float down = Mathf.Min(point1.y, point2.y);
        float up = Mathf.Max(point1.y, point2.y);

        LeftDown = new Vector3(left, down);
        RightDown = new Vector3(right, down);
        LeftUp = new Vector3(left, up);
        RightUp = new Vector3(right, up);
    }
    public Rectangle(float left, float right, float down, float up)
    {
        if (left > right || down > up)
            throw new System.ArgumentException();
        LeftDown = new Vector3(left, down);
        RightDown = new Vector3(right, down);
        LeftUp = new Vector3(left, up);
        RightUp = new Vector3(right, up);
    }
}
