using UnityEngine;

public static class PhysicsTool
{
    /// <summary>
    /// 检测某点沿指定方向到指定图层的距离
    /// </summary>
    /// <param name="origin">起点</param>
    /// <param name="direction">方向</param>
    /// <param name="maxDistance">最大检测距离（应足够大）</param>
    /// <param name="targetMask">目标图层</param>
    /// <returns>距离，探测不到返回maxDistance</returns>
    public static float RaycastRanging(Vector3 origin, Vector3 direction, float maxDistance, LayerMask targetMask)
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
    public static float RaycastRanging(Vector3[] origins, Vector3 direction, float maxDistance, LayerMask targetMask)
    {
        float ret = maxDistance;
        RaycastHit2D hit;
        foreach (Vector3 pos in origins)
        {
            hit = Physics2D.Raycast(pos, direction, maxDistance, targetMask);
            if (hit && hit.distance < ret)
                ret = hit.distance;
        }
        return ret;
    }
}
