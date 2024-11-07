using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace EditorExtend.GridEditor
{
    public static class IsometricGridUtility
    {
        public static Vector3 CellToWorld(Vector3 cellPosition, Vector3 cellSize)
        {
            float x = 0.5f * cellPosition.x * cellSize.x - 0.5f * cellPosition.y * cellSize.x;
            float y = 0.5f * cellPosition.x * cellSize.y + 0.5f * cellPosition.y * cellSize.y + 0.5f * cellPosition.z * cellSize.y * cellSize.z;
            float z = cellPosition.z * cellSize.z;
            return new(x, y, z);
        }

        public static int ManhattanDistance(Vector3Int a, Vector3Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
        }
        public static float ManhattanDistance(Vector3 a, Vector3 b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
        }
        public static float ProjectManhattanDistance(Vector2 a,Vector2 b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
        public static int ProjectManhattanDistance(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        private static readonly List<List<Vector2Int>> cache_withinProjectManhattanDistance = new();

        /// <summary>
        /// 获取到原点的曼哈顿距离小于等于某个值的所有点（不含原点）
        /// </summary>
        public static List<Vector2Int> WithinProjectManhattanDistance(int distance)
        {
            List<Vector2Int> ret = new();
            List<Vector2Int> temp = new();
            temp.AddRange(cache_withinProjectManhattanDistance[^1]);
            while (cache_withinProjectManhattanDistance.Count <= distance)
            {
                List<Vector2Int> layer = new();
                int layerNum = cache_withinProjectManhattanDistance.Count;
                for (int i = 0; i < layerNum; i++)
                {
                    temp.Add(new Vector2Int(-layerNum + i, i));
                    temp.Add(new Vector2Int(layerNum - i, i));
                    temp.Add(new Vector2Int(-i, layerNum - i));
                    temp.Add(new Vector2Int(-layerNum + i, -i));
                }
                layer.AddRange(temp);
                cache_withinProjectManhattanDistance.Add(layer);
            }
            ret.AddRange(cache_withinProjectManhattanDistance[distance]);
            return ret;
        }
    }
}