using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "����SampleNode", menuName = "AStar/�����½ڵ�ķ���/����SampleNode")]
    public class GenerateSampleNodeSO : GenerateNodeSO
    {
        public override Node GenerateNode(PathFindingProcess process, Vector2Int position)
        {
            return new SampleNode(process, position);
        }
    }
}