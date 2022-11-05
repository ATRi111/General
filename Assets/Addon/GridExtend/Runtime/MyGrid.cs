using MyEditor.ShapeEditor;
using System.Collections.Generic;
using UnityEngine;

namespace GridExtend
{
    [RequireComponent(typeof(Grid))]
    public class MyGrid : MonoBehaviour
    {
        private readonly Dictionary<string, MyTilemap> myTilemaps = new Dictionary<string, MyTilemap>();

        private Grid grid;
        public Grid Grid
        {
            get
            {
                if (grid == null)
                    grid = GetComponent<Grid>();
                return grid;
            }
        }
        /// <summary>
        /// Grid��CellToWorldĬ�Ϸ��ظ������½ǵ�λ�ã����ϴ�ƫ������Ϊ��ȷ��λ��
        /// </summary>
        public Vector3 CenterOffset => 0.5f * grid.cellSize;

        public RectInt Rect { get; private set; }
        public int Count => (Rect.width + 1) * (Rect.height + 1);

        public MyTilemap GetMyTilemap(string name)
        {
            myTilemaps.TryGetValue(name, out MyTilemap myTilemap);
            return myTilemap;
        }

        private void Awake()
        {
            MyTilemap[] maps = GetComponentsInChildren<MyTilemap>();
            foreach (MyTilemap map in maps)
            {
                myTilemaps.Add(map.gameObject.name, map);
            }
            if (TryGetComponent(out RectEditor editor))
                Rect = GridTool.ToGrid(editor.WorldRect, this);
        }

        public Vector3 CellToWorld(Vector2Int position)
            => Grid.CellToWorld(new Vector3Int(position.x, position.y, 0)) + grid.cellSize / 2;

        public Vector2Int WorldToCell(Vector3 position)
            => (Vector2Int)Grid.WorldToCell(position);

        /// <summary>
        /// ��һ����Ϸ������뵽������������������
        /// </summary>
        public Vector2Int Align(Transform transform)
        {
            Vector2Int ret = WorldToCell(transform.position);
            transform.position = CellToWorld(ret);
            return ret;
        }
    }
}