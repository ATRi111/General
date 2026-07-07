using AStar.ThreeD;
using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "生成SampleNode3D", menuName = "AStar/生成新节点的方法/生成SampleNode3D")]
    public class GenerateSampleNode3DSO : GenerateNode3DSO
    {
        public override Node3D GenerateNode(PathFinding3DProcess process, Vector3Int position)
        {
            return new SampleNode3D(process, position);
        }
    }
}
