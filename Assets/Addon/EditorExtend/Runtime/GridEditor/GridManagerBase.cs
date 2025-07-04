using System;
using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    //此脚本所在的游戏物体不可平移/缩放/旋转
    [SelectionBase]
    [RequireComponent(typeof(Grid))]
    public abstract class GridManagerBase : MonoBehaviour
    {
        public static Func<GridManagerBase> FindInstance;

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
            => CellToWorld(cellPosition.Round());
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
        /// 给定xy,确定一个z,使cellPositoin最接近worldPosition
        /// </summary>
        public virtual Vector3Int ClosestZ(Vector3Int xy, Vector3 worldPosition)
        {
            return xy;
        }

        /// <summary>
        /// 根据CellPosition自动计算SortingOrder
        /// </summary>
        public abstract int CellToSortingOrder(Vector3 position);

        public virtual void Clear()
        {
            foreach (GridObject obj in ObjectDict.Values)
            {
                obj.referenceCount = 0;
            }
            ObjectDict.Clear();
        }

        public virtual void AddAllObjects()
        {
            Clear();
            GridObject[] objects = GetComponentsInChildren<GridObject>();
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].cellPosition = ClosestCell(objects[i].transform.position);
                TryAddObject(objects[i]);
            }
        }

        public virtual GridObject GetObject(Vector3Int cellPosition)
        {
            if (ObjectDict.TryGetValue(cellPosition, out GridObject ret))
                return ret;
            return null;
        }

        public virtual bool TryAddObject(GridObject gridObject)
        {
            if (gridObject.referenceCount != 0)
            {
                Debug.LogWarning($"试图添加{gridObject.gameObject.name},但referenceCount={gridObject.referenceCount}");
                return false;
            }

            if (!ObjectDict.ContainsKey(gridObject.CellPosition))
            {
                ObjectDict.Add(gridObject.CellPosition, gridObject);
                gridObject.referenceCount++;
                return true;
            }
            else
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(gridObject.gameObject);
#endif
                return false;
            }
        }

        public virtual GridObject TryRemoveObject(Vector3Int cellPosition)
        {
            GridObject gridObject = ObjectDict[cellPosition];

            if (cellPosition == null)
                return null;

            if (gridObject.referenceCount != 1)
            {
                Debug.LogWarning($"试图移除{gridObject.gameObject.name},但referenceCount={gridObject.referenceCount}");
                return null;
            }

            ObjectDict.Remove(cellPosition);
            gridObject.referenceCount--;
            return gridObject;
        }

        public virtual void RelocateObject(GridObject gridObject, Vector3Int prevPosition)
        {
            if (gridObject.referenceCount != 1)
                throw new InvalidOperationException();

            TryRemoveObject(prevPosition);
            TryAddObject(gridObject);
        }

        public virtual bool CanPlaceAt(Vector3Int cellPosition)
        {
            return !ObjectDict.ContainsKey(cellPosition);
        }
    }
}