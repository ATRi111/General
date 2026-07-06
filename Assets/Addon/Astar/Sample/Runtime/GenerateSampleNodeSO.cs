using UnityEngine;
using AStar.TwoD;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "生成SampleNode", menuName = "AStar/生成新节点的方法/生成SampleNode")]
    public class GenerateSampleNodeSO : GenerateNode2DSO
    {
        public override Node2D GenerateNode(PathFinding2DProcess process, Vector2Int position)
        {
            return new SampleNode(process, position);
        }
    }
}
