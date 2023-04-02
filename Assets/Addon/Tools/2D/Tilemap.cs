using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tools
{
    public static partial class Tool2D
    {
        /// <summary>
        /// ��ָ��tilemap�в����Ƿ�����ָ��gameObjectƥ���ruletile��ͨ���������жϣ�
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