using System;
using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "限制额外Cost以限制深度", menuName = "AStar2D/确定扫描深度的方法/限制额外Cost以限制深度")]
    public class LimitedExtraCost2DSO : ScanCheck2DSO
    {
        [Range(0, 1)]
        public float tolerance = 0.2f;

        public override bool ScanCheck(PathFinding2DProcess process, Node2D start, Node2D current)
        {
            //此时Node的各个Cost尚未更新（JPS嵌套扫描里start往往是尚未加入movables的临时节点，
            //HCost还是初始值-1；如果直接读取start.HCost，minCost会是负数，导致tolerance*minCost < 0，
            //而extraCost按三角不等式恒>=0，判断永远为false——等效把嵌套扫描的深度截断成1步）。
            //改成直接用CalculateDistance现算start到终点的距离作为参考基准，不依赖HCost是否已被填充
            float Calc(Node2D from, Node2D to)
                => process.settings.CalculateDistance(from.Position, to.Position);

            float minCost = Calc(start, (Node2D)process.To);
            float extraCost = Calc(start, current)
                + Calc(current, (Node2D)process.To)
                - minCost;
            return extraCost < tolerance * minCost;
        }
    }
}
