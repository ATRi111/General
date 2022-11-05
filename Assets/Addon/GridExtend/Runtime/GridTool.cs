using System.Collections.Generic;
using UnityEngine;

namespace GridExtend
{
    public static class GridTool
    {
        /// <summary>
        /// 获取矩形范围内的坐标为整数的点
        /// </summary>
        public static List<Vector2Int> GetIntPositions(RectInt rect)
        {
            List<Vector2Int> ret = new List<Vector2Int>();
            for (int i = rect.xMin; i <= rect.xMax; i++)
            {
                for (int j = rect.yMin; j < rect.yMax; j++)
                {
                    ret.Add(new Vector2Int(i, j));
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取包围一个区域的网格范围(不足一格的视为一格)
        /// </summary>
        /// <param name="rect">区域(世界坐标)</param>
        /// <returns>网格范围(网格坐标)</returns>
        public static RectInt ToGrid(Rect rect, MyGrid myGrid)
        {
            Vector2Int leftDown = myGrid.WorldToCell(rect.min);
            Vector2Int rightUp = myGrid.WorldToCell(rect.max) + Vector2Int.one;
            return new RectInt(leftDown, rightUp - leftDown);
        }
        /// <summary>
        /// 获取圆形范围内所有坐标为整数的点
        /// </summary>
        public static void GetIntPositions(Vector2Int center, float radius, List<Vector2Int> ret, MyGrid myGrid)
        {
            ret.Clear();
            float sqrRadius = radius * radius;
            int x = Mathf.FloorToInt(radius / myGrid.Grid.cellSize.x);
            int y = Mathf.CeilToInt(radius / myGrid.Grid.cellSize.y);
            Vector2Int v;
            for (int i = -x; i <= x; i++)
            {
                for (int j = -y; j <= y; j++)
                {
                    v = new Vector2Int(i, j);
                    if (v.sqrMagnitude <= sqrRadius)
                        ret.Add(v + center);
                }
            }
        }

        /// <summary>
        /// 使区域内所有不足一格的网格变为一格，并返回结果
        /// </summary>
        /// <param name="rect">区域(世界坐标)</param>
        /// <returns>新区域(世界坐标)</returns>
        public static Rect GetGridAlignedRect(Rect rect, MyGrid myGrid)
        {
            Vector2Int leftDownInt = myGrid.WorldToCell(rect.min);
            Vector2Int rightUpInt = myGrid.WorldToCell(rect.max);
            Vector2 leftDown = myGrid.CellToWorld(leftDownInt) - myGrid.CenterOffset;
            Vector2 rightUp = myGrid.CellToWorld(rightUpInt) + myGrid.CenterOffset;
            return new Rect(leftDown, rightUp - leftDown);
        }
    }
}