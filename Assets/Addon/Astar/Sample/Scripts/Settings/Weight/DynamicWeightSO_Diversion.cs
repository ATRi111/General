namespace AStar
{
    public class DynamicWeightSO_Diversion : CalculateWeightSO
    {
        public float multiplier;

        public override float CalculateWeight(PathFindingProcess process)
        {
            return multiplier * process.currentNode.FCost / process.From.FCost;
        }
    }
}