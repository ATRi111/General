using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    //�˽ű����ڵ���Ϸ���岻��ƽ��/����/��ת
    [SelectionBase]
    [RequireComponent(typeof(Grid))]
    public abstract class GridManagerBase : MonoBehaviour
    {
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

        private Dictionary<Vector3Int, GridObject> objectDict;
        public Dictionary<Vector3Int, GridObject> ObjectDict
        {
            get
            {
                if (objectDict == null)
                {
                    objectDict = new Dictionary<Vector3Int, GridObject>();
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        AddAllObjects();
#endif
                }
                return objectDict;
            }
        }

        public Vector2 centerOffset = new(0.33f, 0.33f);

        public virtual Vector3 CellToWorld(Vector3Int cellPosition)
            => Grid.CellToWorld(cellPosition);
        public virtual Vector3 CellToWorld(Vector3 cellPosition)
            => CellToWorld(cellPosition.Integerized());
        public virtual Vector3Int WorldToCell(Vector3 worldPosition)
           => Grid.WorldToCell(worldPosition);
        public virtual Vector3Int ClosestCell(Vector3 worldPosition)
        {
            Vector3 offset = CellToWorld(new Vector3Int(1, 1, 0));
            offset = new Vector3(offset.x * centerOffset.x, offset.y * centerOffset.y, 0f);
            worldPosition += offset;
            return WorldToCell(worldPosition);
        }
        /// <summary>
        /// ����xy,ȷ��һ��z,ʹcellPositoin��ӽ�worldPosition
        /// </summary>
        public virtual Vector3Int ClosestZ(Vector3Int xy, Vector3 worldPosition)
        {
            return xy;
        }

        /// <summary>
        /// ����CellPosition�Զ�����SortingOrder
        /// </summary>
        public abstract int CellToSortingOrder(GridObject obj);

        public virtual void Clear()
        {
            foreach (GridObject obj in objectDict.Values)
            {
                obj.referenceCount = 0;
            }
            objectDict.Clear();
        }

        public virtual void AddAllObjects()
        {
            Clear();
            GridObject[] objects = GetComponentsInChildren<GridObject>();
            for (int i = 0; i < objects.Length; i++)
            {
                AddObject(objects[i]);
            }
        }

        public virtual GridObject GetObject(Vector3Int cellPosition)
        {
            if (ObjectDict.TryGetValue(cellPosition, out GridObject ret))
                return ret;
            return null;
        }

        public virtual void AddObject(GridObject gridObject)
        {
            if (gridObject.referenceCount != 0)
            {
                Debug.LogWarning($"{gridObject.gameObject.name}��ͼ����,��referenceCount={gridObject.referenceCount}");
                throw new System.InvalidOperationException();
            }

            ObjectDict.Add(gridObject.CellPosition, gridObject);
            gridObject.referenceCount++;
        }

        public virtual GridObject RemoveObject(Vector3Int cellPosition)
        {
            GridObject gridObject = ObjectDict[cellPosition];
            ObjectDict.Remove(cellPosition);
            gridObject.referenceCount--;
            return gridObject;
        }

        public virtual void RelocateObject(GridObject gridObject,Vector3Int prevPosition)
        {
            if (gridObject.referenceCount != 1)
                throw new System.InvalidOperationException();

            RemoveObject(prevPosition); 
            AddObject(gridObject);
        }

        public virtual bool CanPlaceAt(Vector3Int cellPosition)
        {
            return !ObjectDict.ContainsKey(cellPosition);
        }
    }
}