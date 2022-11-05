using UnityEngine;

namespace Tools
{
    public static class PhysicsTool2D
    {
        /// <summary>
        /// 检测某点沿指定方向到指定图层的距离
        /// </summary>
        /// <param name="origin">起点</param>
        /// <param name="direction">方向</param>
        /// <param name="maxDistance">最大检测距离（应足够大）</param>
        /// <param name="targetMask">目标图层</param>
        /// <returns>距离，探测不到返回maxDistance</returns>
        public static float RaycastRanging(Vector2 origin, Vector2 direction, float maxDistance, LayerMask targetMask)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, targetMask);
            return hit ? hit.distance : maxDistance;
        }
        /// <summary>
        /// 从多点出发，检测沿指定方向到指定图层的距离，取最小值
        /// </summary>
        /// <param name="origins">所有起点</param>
        /// <param name="direction">方向</param>
        /// <param name="maxDistance">最大检测距离（应足够大）</param>
        /// <param name="targetMask">目标图层</param>
        /// <returns>最小距离，探测不到返回maxDistance</returns>
        public static float RaycastRanging(Vector2[] origins, Vector2 direction, float maxDistance, LayerMask targetMask)
        {
            float ret = maxDistance;
            RaycastHit2D hit;
            foreach (Vector2 pos in origins)
            {
                hit = Physics2D.Raycast(pos, direction, maxDistance, targetMask);
                if (hit && hit.distance < ret)
                    ret = hit.distance;
            }
            return ret;
        }

        /// <summary>
        /// 从一点出发，均匀向四周发射等长射线，接触障碍物时停止，返回各射线到达的点组成的轮廓
        /// </summary>
        /// <param name="ret">接收结果，此数组的容量决定射线数目</param>
        /// <param name="center">射线中心</param>
        /// <param name="radius">射线长度</param>
        /// <param name="obstacleMask">障碍物图层</param>
        /// <param name="angle">第一条射线的角度</param>
        public static void CicularRaycast(Vector2[] ret, Vector2 center, float radius, LayerMask obstacleMask, float angle = 0f)
        {
            int n = ret.Length;
            float deltaAngle = 360f / n;
            Vector2 direction;
            float distance;
            for (int i = 0; i < n; i++)
            {
                direction = angle.ToDirection();
                distance = RaycastRanging(center, direction, radius, obstacleMask);
                ret[i] = center + distance * direction;
                angle += deltaAngle;
            }
        }
        /// <summary>
        /// 检测两点连线是否被指定图层遮挡
        /// </summary>
        public static bool ObstructRaycast(Vector2 a, Vector2 b, LayerMask obstacleMask)
        {
            Vector2 direction = (b - a).normalized;
            float length = (a - b).magnitude;
            return Physics2D.Raycast(a, direction, length, obstacleMask);
        }
    }
}

