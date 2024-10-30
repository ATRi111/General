using System;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridObject : MonoBehaviour
    {
        #region ���
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
        /// ����ֵͨ�������Ӧ��������1
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

        #region λ��
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
        /// cellPosition��Z����,ȷ��XY��ʹ��Ӧ������������ӽ���ǰ��������
        /// </summary>
        public Vector3Int AlignXY()
        {
            CellPosition = Manager.ClosestCell(transform.position);
            return CellPosition;
        }
        /// <summary>
        /// cellPosition��XY����,ȷ��һ��Z��ʹ��Ӧ������������ӽ���ǰ��������
        /// </summary>
        public Vector3Int AlignZ()
        {
            CellPosition = Manager.ClosestZ(cellPosition, transform.position);
            return CellPosition;
        }
        #endregion

        #region ��Ϸ�߼�
        protected internal Func<int> GroundHeightFunc;
        /// <summary>
        /// ���ӵ�������ʱ��������ĸ߶�
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
        /// ����ռ�ݵķ�Χ�Ƿ񸲸����������µ�ĳ��
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

        #region ��������
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