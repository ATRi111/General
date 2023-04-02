using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tools
{
    public static partial class Tool2D
    {
        /// <summary>
        /// 在指定tilemap中查找是否有与指定gameObject匹配的ruletile（通过名称来判断）
        /// </summary>
        public static bool MatchRuleTileByName(Tilemap tilemap, GameObject gameObject, out Vector3Int cellPosition)
        {
            cellPosition = tilemap.WorldToCell(gameObject.transform.position);
            RuleTile ruleTile = tilemap.GetTile(cellPosition) as RuleTile;
            if (ruleTile == null)
                return false;
            if (gameObject.name.Contains(ruleTile.m_DefaultGameObject.name))
                return true;
            return false;
        }
    }
}