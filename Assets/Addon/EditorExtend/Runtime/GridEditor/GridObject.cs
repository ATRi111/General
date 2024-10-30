using System;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridObject : MonoBehaviour
    {
        #region 组件
        private GridManagerBase manager;
        public GridManagerBase Manager
        {
            get
            {
                if (manager == null)
                    manager = GetComponentInParent<GridManagerBase>();
                return manager;
            }
        }

        private SpriteRenderer spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (spriteRenderer == null)
                    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                return spriteRenderer;
            }
        }

        [SerializeField]
        private string shortName;
        public string ShortName
        {
            get
            {
                if (string.IsNullOrEmpty(shortName))
                    shortName = ExternalTool.GetShortName(gameObject);
                return shortName;
            }
        }

        public virtual int ExtraSortingOrder => 0;
        [NonSerialized]
        /// <summary>
        /// 此数值通常情况下应当保持在1
        /// </summary>
        public int referenceCount;

        protected void Register()
        {
            Manager.AddObject(this);
        }

        protected virtual void Unregister()
        {
            Manager.RemoveObject(CellPosition);
        }

        #endregion

        #region 位置
        [SerializeField]
        protected Vector3Int cellPosition;
        public Vector3Int CellPosition
        {
            get => cellPosition;
            set
            {
                if (referenceCount == 0)
                {
                    cellPosition = value;
                    Manager.AddObject(this);
                    Refresh();
                }
                else if (referenceCount == 1)
                {
                    if (value != cellPosition)
                    {
                        Vector3Int prev = cellPosition;
                        cellPosition = value;
                        Manager.RelocateObject(this, prev);
                    }
                    Refresh();
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public Vector3 Refresh()
        {
            transform.position = Manager.Grid.CellToWorld(cellPosition);
            SpriteRenderer.sortingOrder = Manager.CellToSortingOrder(this);
            return transform.position;
        }
        /// <summary>
        /// cellPosition的Z不变,确定XY，使对应的世界坐标最接近当前世界坐标
        /// </summary>
        public Vector3Int AlignXY()
        {
            CellPosition = Manager.ClosestCell(transform.position);
            return CellPosition;
        }
        /// <summary>
        /// cellPosition的XY不变,确定一个Z，使对应的世界坐标最接近当前世界坐标
        /// </summary>
        public Vector3Int AlignZ()
        {
            CellPosition = Manager.ClosestZ(cellPosition, transform.position);
            return CellPosition;
        }
        #endregion

        #region 游戏逻辑
        protected internal Func<int> GroundHeightFunc;
        /// <summary>
        /// 发挥地面作用时，此物体的高度
        /// </summary>
        public int GroundHeight
        {
            get
            {
                if (GroundHeightFunc != null)
                    return GroundHeightFunc.Invoke();
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    GridGround ground = GetComponentInChildren<GridGround>();
                    if (ground != null)
                        return ground.GroundHeight();
                }
#endif
                return 1;
            }
        }

        protected internal Func<Vector3, bool> OverlapFunc;
        /// <summary>
        /// 物体占据的范围是否覆盖网格坐标下的某点
        /// </summary>
        public virtual bool Overlap(Vector3 p)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GridCollider collider = GetComponentInChildren<GridCollider>();
                if (collider != null)
                    return collider.Overlap(p);
            }
#endif
            if (OverlapFunc != null)
                return OverlapFunc.Invoke(p);
            return GridUtility.BoxOverlap(cellPosition, Vector3Int.one, p);
        }

        #endregion

        #region 生命周期
        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();
        }
        #endregion 
    }
}