using System;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [SelectionBase]
    public class GridObject : MonoBehaviour
    {
        #region ���
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

        [NonSerialized]
        /// <summary>
        /// ����ֵͨ�������Ӧ��������1
        /// </summary>
        public int referenceCount;

        public void Register()
        {
            //��Manager��������ſ�ע��
            if (Manager != null)
                Manager.TryAddObject(this);
        }

        public virtual void Unregister()
        {
            //��Manager��������ſ�ȡ��ע��
            if (Manager != null)
                Manager.TryRemoveObject(CellPosition);
        }

        #endregion

        #region λ��
        [SerializeField]
        //���ֶβ������־û����ݣ�transform.position���ǳ־û�����
        internal Vector3Int cellPosition;
        public Vector3Int CellPosition
        {
            get => cellPosition;
            set
            {
                if (referenceCount == 0)
                {
                    cellPosition = value;
                    if (Manager != null)
                        Manager.TryAddObject(this);
                    Refresh();
                    GridSortingOrderControllerBase.RefreshChildren(this);
                }
                else if (referenceCount == 1)
                {
                    if (value != cellPosition)
                    {
                        Vector3Int prev = cellPosition;
                        cellPosition = value;
                        if (Manager != null)
                            Manager.RelocateObject(this, prev);
                    }
                    Refresh();
                    GridSortingOrderControllerBase.RefreshChildren(this);
                }
                else
                    throw new Exception($"{gameObject.name}��{referenceCount}ֵ������");
            }
        }

        public Vector3 Refresh()
        {
            transform.position = Manager.Grid.CellToWorld(cellPosition);
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
        public bool IsGround => groundHeight > 0;
        [SerializeField]
        protected int groundHeight = 1;
        /// <summary>
        /// ���ӵ�������ʱ��������ĸ߶�
        /// </summary>
        public virtual int GroundHeight => groundHeight;
        /// <summary>
        /// ���ĵ�����
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
        /// �ϱ������ĵ�
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
        /// �±������ĵ�
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
        /// ����ռ�ݵķ�Χ�Ƿ񸲸����������µ�ĳ��
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
        /// �ж������Ƿ����߶��ཻ�����ཻ����㽻��
        /// </summary>
        public virtual bool OverlapLineSegment(ref Vector3 from, ref Vector3 to)
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

        #region ��������

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
        #endregion 
    }
}