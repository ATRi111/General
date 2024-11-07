using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.ShapeEditor
{
    public enum EPolygonStyle
    {
        ClosedPolyLine,
        PolyLine,
        Point
    }

    public class PolygonMono : Shape2DMono
    {
        public EPolygonStyle style;
        public List<Vector3> localPoints;

        public override Rect GetAABB()
        {
            Rect local = ExternalTool.GetAABB(localPoints);
            return new Rect(local.position + Position2D, local.size);
        }

        public void GetLocalPoints(List<Vector3> ret)
        {
            ret.Clear();
            ret.AddRange(localPoints);
        }
        public void GetLocalPoints(List<Vector2> ret)
        {
            ret.Clear();
            for (int i = 0; i < localPoints.Count; i++)
            {
                ret.Add(localPoints[i]);
            }
        }
        public void GetWorldPoints(List<Vector3> ret)
        {
            ret.Clear();
            for (int i = 0; i < localPoints.Count; i++)
            {
                ret.Add(localPoints[i] + transform.position);
            }
        }

        protected override void Reset()
        {
            base.Reset();
            localPoints = new List<Vector3>() { new(-1, -1, 0), new(-1, +1, 0), new(+1, +1, 0), new(+1, -1, 0), };
        }
    }
}
