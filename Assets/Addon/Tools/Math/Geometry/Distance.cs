using UnityEngine;

namespace Tools
{
    public static partial class GeometryTool
    {
        public static float ManhattanDistance(Vector2 a, Vector2 b)
            => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        public static int ManhattanDistanceInt(Vector2Int a, Vector2Int b)
            => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        public static float ChebyshevDistance(Vector2 a, Vector2 b)
        {
            float deltaX = Mathf.Abs(a.x - b.x);
            float deltaY = Mathf.Abs(a.y - b.y);
            float max = Mathf.Max(deltaX, deltaY);
            float min = Mathf.Min(deltaX, deltaY);
            return min * MathTool.DIAGNOL + max - min;
        }
        public static int ChebyshevDistanceInt(Vector2Int a, Vector2Int b, int side = 100, int diagnol = 141)
        {
            int deltaX = Mathf.Abs(a.x - b.x);
            int deltaY = Mathf.Abs(a.y - b.y);
            int max = Mathf.Max(deltaX, deltaY);
            int min = Mathf.Min(deltaX, deltaY);
            return min * diagnol + (max - min) * side;
        }
    }
}

