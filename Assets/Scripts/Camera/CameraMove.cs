using System.Collections.Generic;

[System.Serializable]
public class CameraMove
{
    public List<SingleCameraMove> singleCameraMoves = new List<SingleCameraMove>();
    public CameraMove() { }
    public static CameraMove operator +(CameraMove c, SingleCameraMove s)
    {
        if (s != null)
            c.singleCameraMoves.Add(s);
        return c;
    }
}
