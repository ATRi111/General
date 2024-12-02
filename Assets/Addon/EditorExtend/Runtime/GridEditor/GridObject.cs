using System;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridObject : MonoBehaviour
    {
        #region 组件
        protected GridManagerBase manager;
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
                    throw new Exception($"{gameObject.name}的{referenceCount}值不合理");
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
        [SerializeField]
        protected int groundHeight = 1;
        /// <summary>
        /// 发挥地面作用时，此物体的高度
        /// </summary>
        public virtual int GroundHeight => groundHeight;
        /// <summary>
        /// 中心点坐标
        /// </summary>
        public virtual Vector3 Center
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    gridCollider = GetComponentInChildren<GridCollider>();
#endif
                if (gridCollider != null)
                    return gridCollider.Center;

                return cellPosition + 0.5f * Vector3.one;
            }
        }
        /// <summary>
        /// 上表面中心点
        /// </summary>
        public virtual Vector3 TopCenter
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    gridCollider = GetComponentInChildren<GridCollider>();
#endif
                if (gridCollider != null)
                    return gridCollider.TopCenter;

                return cellPosition + new Vector3(0.5f, 0.5f, 1f);
            }
        }
        /// <summary>
        /// 下表面中心点
        /// </summary>
        public virtual Vector3 BottomCenter
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    gridCollider = GetComponentInChildren<GridCollider>();
#endif
                if (gridCollider != null)
                    return gridCollider.BottomCenter;

                return cellPosition + new Vector3(0.5f, 0.5f, 0f);
            }
        }

        protected GridCollider gridCollider;
        /// <summary>
        /// 物体占据的范围是否覆盖网格坐标下的某点
        /// </summary>
        public virtual bool Overlap(Vector3 p)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                gridCollider = GetComponentInChildren<GridCollider>();
#endif
            if (gridCollider != null)
                return gridCollider.Overlap(p);

            return GridPhysics.BoxOverlap(cellPosition, Vector3Int.one, p);
        }

        /// <summary>
        /// 判断物体是否与线段相交，若相交则计算交点
        /// </summary>
        public virtual bool OverlapLineSegment(ref Vector3 from,ref Vector3 to)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                gridCollider = GetComponentInChildren<GridCollider>();
#endif
            if (gridCollider != null)
                return gridCollider.OverlapLineSegment(ref from, ref to);

            return GridPhysics.LineSegmentCastBox(cellPosition, Vector3Int.one, ref from, ref to);
        }


        #endregion

        public bool autoSortingOrder = true;

        #region 生命周期

        protected virtual void Awake()
        {
            gridCollider = GetComponentInChildren<GridCollider>();
        }

        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();
        }

        protected virtual void Update()
        {
            if (autoSortingOrder)
                SpriteRenderer.sortingOrder = Manager.CellToSortingOrder(this);
        }

        private void OnDrawGizmos()
        {
            if(autoSortingOrder)
                SpriteRenderer.sortingOrder = Manager.CellToSortingOrder(this);
        }
        #endregion 
    }
}