using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricGridManagerBase : GridManagerBase
    {
        //Isometric��ͼ��ͬһ(x,y)�������ж��������ͬ�����壬ʹ�������ֵ��¼ĳλ�õĲ�����Χ������߲�ѯЧ��
        private readonly Dictionary<Vector2Int, int> maxLayerDict = new();
        public Dictionary<Vector2Int, int> MaxLayerDict => maxLayerDict;
        public int maxLayer;

        public bool Contains(Vector2Int position)
            => maxLayerDict.ContainsKey(position);

        public override Vector3 CellToWorld(Vector3 cellPosition)
            => IsometricGridUtility.CellToWorld(cellPosition, Grid.cellSize);

        public override int CellToSortingOrder(Vector3 position)
        {
            Vector3 cell = IsometricGridUtility.WorldToCell(position, Grid.cellSize);
            return Mathf.RoundToInt(10f * (-cell.x - cell.y + cell.z));
        }

        public float LayerToWorldZ(float layer)
        {
            return layer * Grid.cellSize.z;
        }

        public override Vector3Int ClosestZ(Vector3Int xy, Vector3 worldPosition)
        {
            xy = xy.ResetZ();
            Vector3 worldBase = CellToWorld(xy);
            float deltaWorldY = worldPosition.y - worldBase.y;
            float cellZf = deltaWorldY / Grid.cellSize.y / Grid.cellSize.z * 2f;
            int cellZ = Mathf.Max(0, Mathf.FloorToInt(cellZf));  //0�����½�ֹ����
            return xy.ResetZ(cellZ);
        }

        public override void Clear()
        {
            base.Clear();
            maxLayer = 0;
            maxLayerDict.Clear();
        }

        public override bool TryAddObject(GridObject gridObject)
        {
            if (!base.TryAddObject(gridObject))
                return false;

            Vector2Int xy = (Vector2Int)gridObject.CellPosition;
            int layer = gridObject.CellPosition.z;
            if (!maxLayerDict.ContainsKey(xy))
            {
                maxLayerDict.Add(xy, layer);
            }
            else
            {
                maxLayerDict[xy] = Mathf.Max(maxLayerDict[xy], layer);
            }
            maxLayer = Mathf.Max(maxLayer, layer);
            return true;
        }

        public override GridObject TryRemoveObject(Vector3Int cellPosition)
        {
            GridObject gridObject = base.TryRemoveObject(cellPosition);
            if (gridObject != null)
                UpdateMaxLayerXY((Vector2Int)gridObject.CellPosition);
            return gridObject;
        }

        public void UpdateMaxLayerXY(Vector2Int xy)
        {
            if (!maxLayerDict.ContainsKey(xy))
                maxLayerDict.Add(xy, 0);
            else
            {
                GridObject gridObject = GetObjectXY(xy);
                if (gridObject != null)
                    maxLayerDict[xy] = gridObject.CellPosition.z;
                else
                    maxLayerDict[xy] = 0;
            }
        }

        /// <summary>
        /// ��ȡxy�����ϵ���������
        /// </summary>
        public void GetObjectsXY(Vector2Int xy, List<GridObject> objects, bool top_down = true)
        {
            objects.Clear();
            if (!maxLayerDict.ContainsKey(xy))
                return;
            if (top_down)
            {
                for (int layer = maxLayerDict[xy]; layer >= 0; layer--)
                {
                    Vector3Int temp = xy.AddZ(layer);
                    GridObject obj = GetObject(temp);
                    if (obj != null)
                        objects.Add(obj);
                }
            }
            else
            {
                for (int layer = 0; layer <= maxLayerDict[xy]; layer++)
                {
                    Vector3Int temp = xy.AddZ(layer);
                    GridObject obj = GetObject(temp);
                    if (obj != null)
                        objects.Add(obj);
                }
            }
        }
        /// <summary>
        /// ��ȡxy�����ϲ�����ߵ�����
        /// </summary>
        public GridObject GetObjectXY(Vector2Int xy)
        {
            if (!maxLayerDict.ContainsKey(xy))
                return null;
            for (int layer = maxLayerDict[xy]; layer >= 0; layer--)
            {
                Vector3Int temp = xy.AddZ(layer);
                GridObject obj = GetObject(temp);
                if (obj != null)
                    return obj;
            }
            return null;
        }
        /// <summary>
        /// xy�����ϣ������Ϸ��ĵ�һ���㼶
        /// </summary>
        public int AboveGroundLayer(Vector2Int xy)
        {
            GridObject gridObject = GetObjectXY(xy);
            if (gridObject != null)
                return gridObject.CellPosition.z + gridObject.GroundHeight;
            return 0;
        }
        /// <summary>
        /// xy�����ϣ������Ϸ��ĵ�һ��λ��
        /// </summary>
        public Vector3Int AboveGroundPosition(Vector2Int xy)
        {
            return xy.AddZ(AboveGroundLayer(xy));
        }

        /// <summary>
        /// �����������(������GridObject��Ϊ��������ϵ�±߳�Ϊ1��������)
        /// </summary>
        public virtual Vector3Int AbsorbSurfaceOfCube(Vector3 worldPosition)
        {
            Vector3 from = WorldToCell(worldPosition.ResetZ(LayerToWorldZ(maxLayer)));
            Vector3 to = WorldToCell(worldPosition.ResetZ(LayerToWorldZ(0)));
            List<Vector2Int> xys = maxLayerDict.Keys.ToList();
            xys.Sort((a, b) => (a.x + a.y).CompareTo(b.x + b.y));   //x+y��С����ǰ������ȷ����ȷ����
            List<Vector3Int> positions = new();
            for (int i = 0; i < xys.Count; i++)
            {
                GridObject gridObject = GetObjectXY(xys[i]);
                if (gridObject == null)
                    continue;
                if (GridPhysics.LineSegmentCastBox(gridObject.CellPosition - Vector3Int.forward, Vector3Int.one + Vector3Int.forward, ref from, ref to))
                    return gridObject.CellPosition;
            }
            return WorldToCell(worldPosition.ResetZ(LayerToWorldZ(0)));
        }

        public override bool CanPlaceAt(Vector3Int cellPosition)
        {
            if (!base.CanPlaceAt(cellPosition))
                return false;

            List<GridObject> objects = new();
            GetObjectsXY((Vector2Int)cellPosition, objects);
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Overlap(cellPosition))
                    return false;
            }
            return true;
        }
    }
}