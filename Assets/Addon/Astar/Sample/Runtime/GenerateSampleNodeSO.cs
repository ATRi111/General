using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "生成SampleNode",menuName = "AStar/生成新节点的方法/生成SampleNode")]
    public class GenerateSampleNodeSO : GenerateNodeSO
    {
        public override Node GenerateNode(PathFindingProcess process, Vector2Int position)
        {
            return new SampleNode(process, position);
        }
    }
}