using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MyTilemap : MonoBehaviour
{
    protected Tilemap m_tilemap;
    public TileBase this[Vector3Int cellPosition]
    {
        get => m_tilemap.GetTile<TileBase>(cellPosition);
        set => m_tilemap.SetTile(cellPosition, value);
    }
    public TileBase this[Vector3 worldPosition]
    {
        get => m_tilemap.GetTile<TileBase>(m_tilemap.WorldToCell(worldPosition));
        set => m_tilemap.SetTile(m_tilemap.WorldToCell(worldPosition), value);
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
    /// 获取所有tile的位置（可用于修改tile）
    /// </summary>
    /// <param name="ret">接收结果</param>
    /// <param name="includingNull">是否包含空tile的位置（若包含，将返回包围盒内的所有位置）</param>
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
    /// 获取所有tile（不能用于修改tile）
    /// </summary>
    /// <param name="ret">接收结果</param>
    public void GetAll(List<TileBase> ret)
    {
        if (ret == null)
            ret = new List<TileBase>();
        Vector3Int tilePosition;
        //Debug.Log($"边界为（{Bounds.xMin},{Bounds.yMin}） （{Bounds.xMax},{Bounds.yMax}）");
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
