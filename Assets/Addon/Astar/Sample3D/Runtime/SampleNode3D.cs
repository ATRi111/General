using AStar.ThreeD;
using UnityEngine;

namespace AStar.Sample
{
    /// <summary>
    /// 场景里没有Tilemap可用，改用 <see cref="PathFindingSample3D"/> 预先建好的位置字典判定障碍：
    /// 命中的物体名字以"Block"开头即视为障碍物（约定与2D Sample里 Tile 名为"Block"时视为障碍一致）
    /// </summary>
    public class SampleNode3D : Node3D
    {
        internal SampleNode3D(PathFinding3DProcess process, Vector3Int position)
            : base(process, position)
        {
        }

        private PathFindingSample3D sample;
        private bool? isObstacle;

        public override bool IsObstacle
        {
            get
            {
                sample ??= process.mountPoint.GetComponentInChildren<PathFindingSample3D>();
                isObstacle ??= sample.IsBlockAt(Position);
                return isObstacle.Value;
            }
        }
    }
}
