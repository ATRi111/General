using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricGridManagerBase : GridManagerBase
    {
        //Isometric��ͼ��ͬһ(x,y)�������ж��������ͬ�����壬ʹ�������ֵ��¼ĳλ�õĲ�����Χ������߲�ѯЧ��
        private readonly Dictionary<Vector2Int, int> maxLayerDict = new();
        public Dictionary<Vector2Int, int> MaxLayerDict => maxLayerDict;
        public int maxLayer;

        public override Vector3 CellToWorld(Vector3 cellPosition)
            => IsometricGridUtility.CellToWorld(cellPosition, Grid.cellSize);

        public override int CellToSortingOrder(GridObject obj)
        {
            return 10 * (-obj.CellPosition.x - obj.CellPosition.y + obj.CellPosition.z) + obj.ExtraSortingOrder;
        }

        public float LayerToWorldZ(int layer)
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

        /// <summary>
        /// �����������꣨����z��ȷ��һЩ���������꣬�ж���Щ�����������Ƿ������壬�����򷵻����е���߲���
        /// </summary>
        public bool MatchMaxLayer(Vector3 worldPosition, out int layer)
        {
            for (int currentLayer = maxLayer; currentLayer >= 0; currentLayer--)
            {
                float z = LayerToWorldZ(currentLayer);
                worldPosition = worldPosition.ResetZ(z);
                Vector3Int temp = WorldToCell(worldPosition);
                if (GetObject(temp) != null)
                {
                    layer = currentLayer;
                    return true;
                }
            }
            layer = 0;
            return false;
        }

        public override void Clear()
        {
            base.Clear();
            maxLayer = 0;
            maxLayerDict.Clear();
        }

        public override void AddObject(GridObject gridObject)
        {
            base.AddObject(gridObject);
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
        }

        /// <summary>
        /// ��ȡxy�����ϵ���������
        /// </summary>
        public void GetObejectsXY(Vector2Int xy, List<GridObject> objects)
        {
            objects.Clear();
            if (!maxLayerDict.ContainsKey(xy))
                return;
            for (int layer = maxLayerDict[xy]; layer >= 0; layer--) 
            {
                Vector3Int temp = xy.AddZ(layer);
                GridObject obj = GetObject(temp);
                if (obj != null)
                    objects.Add(obj);
            }
        }
        /// <summary>
        /// ��ȡxy�����ϲ�����ߵ�����
        /// </summary>
        public GridObject GetObejectXY(Vector2Int xy)
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
            int ret = 0;
            GridObject gridObject = GetObejectXY(xy);
            if (gridObject != null)
                ret = gridObject.CellPosition.z + gridObject.GroundHeight;
            return ret;
        }

        public override bool CanPlaceAt(Vector3Int cellPosition)
        {
            if (!base.CanPlaceAt(cellPosition))
                return false;
            if (!maxLayerDict.ContainsKey((Vector2Int)cellPosition))
                return true;
            if (cellPosition.z > maxLayerDict[(Vector2Int)cellPosition])
                return cellPosition.z >= AboveGroundLayer((Vector2Int)cellPosition);
            
            for (int layer = cellPosition.z; layer >= 0; layer--)
            {
                Vector3Int temp = cellPosition.ResetZ(layer);
                GridObject obj = GetObject(temp);
                if (obj != null)
                    return cellPosition.z >= obj.GroundHeight + obj.CellPosition.z;
            }
            return true;
        }
    }
}