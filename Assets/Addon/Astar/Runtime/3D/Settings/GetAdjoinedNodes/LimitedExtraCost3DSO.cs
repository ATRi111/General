using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "限制额外Cost以限制深度", menuName = "AStar3D/确定扫描深度的方法/限制额外Cost以限制深度")]
    public class LimitedExtraCost3DSO : ScanCheck3DSO
    {
        [Range(0, 1)]
        public float tolerance = 0.1f;

        public override bool ScanCheck(PathFinding3DProcess process, Node3D start, Node3D current)
        {
            //此时Node的各个Cost尚未更新（JPS嵌套扫描里start往往是尚未加入movables的临时节点，
            //HCost还是初始值-1；如果直接读取start.HCost，minCost会是负数，导致tolerance*minCost < 0，
            //而extraCost按三角不等式恒>=0，判断永远为false——等效把嵌套扫描的深度截断成1步）。
            //改成直接用CalculateDistance现算start到终点的距离作为参考基准，不依赖HCost是否已被填充
            float Calc(Node3D from, Node3D to)
                => process.settings.CalculateDistance(from.Position, to.Position);

            float minCost = Calc(start, process.To as Node3D);
            float extraCost = Calc(start, current)
                + Calc(current, process.To as Node3D)
                - minCost;
            return extraCost < tolerance * minCost;
        }
    }
}
