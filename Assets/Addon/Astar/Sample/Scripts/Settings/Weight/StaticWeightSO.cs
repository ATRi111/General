namespace AStar.Sample
{
    public class StaticWeightSO : CalculateWeightSO
    {
        public float weight;

        public override float CalculateWeight(PathFindingProcess process)
        {
            return weight;
        }
    }
}