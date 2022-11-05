using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GridExtend
{
    public class MyTilemap : MonoBehaviour
    {
        protected Tilemap m_tilemap;
        public TileBase this[Vector3Int cellPosition]
        {
            get => m_tilemap.GetTile(cellPosition);
            set
            {
                m_tilemap.SetTile(cellPosition, value);
                m_tilemap.RefreshTile(cellPosition);
            }
        }
        public TileBase this[Vector3 worldPosition]
        {
            get => this[m_tilemap.WorldToCell(worldPosition)];
            set => this[m_tilemap.WorldToCell(worldPosition)] = value;
        }

        public BoundsInt Bounds => m_tilemap.cellBounds;
        public Vector2 Center => Bounds.center;

        protected virtual void Awake()
        {
            m_tilemap = GetComponent<Tilemap>();
        }

        public void SetColor(Color color)
        {
            m_tilemap.color = color;
        }

        /// <summary>
        /// ��ȡ����tile��λ�ã��������޸�tile��
        /// </summary>
        /// <param name="ret">���ս��</param>
        /// <param name="includingNull">�Ƿ������tile��λ�ã��������������ذ�Χ���ڵ�����λ�ã�</param>
        /// <returns></returns>
        public void GetAllPosition(List<Vector3Int> ret, bool includingNull = false)
        {
            if (ret == null)
                ret = new List<Vector3Int>();
            Vector3Int tilePosition;

            if (includingNull)
            {
                for (int i = Bounds.xMin; i < Bounds.xMax; i++)
                {
                    for (int j = Bounds.yMin; j < Bounds.yMax; j++)
                    {
                        tilePosition = new Vector3Int(i, j, 0);
                        ret.Add(tilePosition);
                    }
                }
            }
            else
            {
                for (int i = Bounds.xMin; i < Bounds.xMax; i++)
                {
                    for (int j = Bounds.yMin; j < Bounds.yMax; j++)
                    {
                        tilePosition = new Vector3Int(i, j, 0);
                        if (this[tilePosition] != null)
                            ret.Add(tilePosition);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ����tile�����������޸�tile��
        /// </summary>
        /// <param name="ret">���ս��</param>
        public void GetAll(List<TileBase> ret)
        {
            if (ret == null)
                ret = new List<TileBase>();
            Vector3Int tilePosition; ;
            for (int i = Bounds.xMin; i < Bounds.xMax; i++)
            {
                for (int j = Bounds.yMin; j < Bounds.yMax; j++)
                {
                    tilePosition = new Vector3Int(i, j, 0);
                    ret.Add(this[tilePosition]);
                }
            }
        }
    }
}