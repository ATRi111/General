using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    internal static class GridPhysics
    {
        public readonly static GridPhysicsSettings settings;

        static GridPhysics()
        {
            settings = Resources.Load<GridPhysicsSettings>(nameof(GridPhysicsSettings));
        }

        #region 点求交
        public static bool BoxOverlap(Vector3 min, Vector3 extend, Vector3 p)
        {
            return p.x >= min.x && p.x < min.x + extend.x
                && p.y >= min.y && p.y < min.y + extend.y
                && p.z >= min.z && p.z < min.z + extend.z;
        }
        public static bool CylinderOverlap(Vector3 bottomCenter, float height, float radius, Vector3 p)
        {
            if (p.z < bottomCenter.z || p.z >= bottomCenter.z + height)
                return false;
            float projSqrDistance = (p - bottomCenter).ResetZ().sqrMagnitude;
            return projSqrDistance < radius * radius;
        }

        public static bool PointsCastBox(Vector3 min, Vector3 extend, List<Vector3> points,out Vector3 enter,out Vector3 exit)
        {
            enter = exit = Vector3.zero;
            int i = 0;
            for (;i < points.Count;i++)
            {
                if(BoxOverlap(min,extend,points[i]))
                {
                    enter = points[i];
                    i++;
                    break;
                }
            }
            for (;i < points.Count;i++)
            {
                if(!BoxOverlap(min,extend,points[i]))
                {
                    exit = points[i];
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 线段求交
        public static bool LineSegmentCastBox(Vector3 min, Vector3 extend, ref Vector3 from, ref Vector3 to)
        {
            float uIn = 0, uOut = 1;
            float u1, u2;
            Vector3 v = to - from;

            bool IntersectAndCheck(float p1, float p2, float p0, float q)
            {
                if (q == 0)
                    return p0 >= p1 && p0 <= p2;

                u1 = (p1 - p0) / q;
                u2 = (p2 - p0) / q;
                if (u1 > u2)
                    (u1, u2) = (u2, u1);
                uIn = Mathf.Max(uIn, u1);
                uOut = Mathf.Min(uOut, u2);
                return uIn < uOut;
            }

            if (IntersectAndCheck(min.x, min.x + extend.x, from.x, v.x)
                && IntersectAndCheck(min.y, min.y + extend.y, from.y, v.y)
                && IntersectAndCheck(min.z, min.z + extend.z, from.z, v.z))
            {
                to = from + uOut * v;
                from += uIn * v;
                return true;
            }
            return false;
        }

        public static bool LineSegmentCastCylinder(Vector3 bottomCenter, float height, float radius, ref Vector3 from, ref Vector3 to)
        {
            from -= bottomCenter;
            to -= bottomCenter;
            bool ret = LineSegmentCastCylinder(height, radius, ref from, ref to);  //转换为圆心在原点的问题
            from += bottomCenter;
            to += bottomCenter;
            return ret;
        }
        private static bool LineSegmentCastCylinder(float height, float radius, ref Vector3 from, ref Vector3 to)
        {
            float uIn = 0, uOut = 1;
            float u1 , u2;
            Vector3 v = to - from;
            //先投影到xy平面，求线段与圆的交点
            
            if(v.x == 0)
            {
                float d = radius * radius - from.x * from.x;
                if(d < 0)
                    return false;
                float y1 = -Mathf.Sqrt(d);
                float y2 = -y1;
                u1 = (y1 - from.y) / v.y;
                u2 = (y2 - from.y) / v.y;
            }
            else
            {
                //y=kx+m
                float k = v.y / v.x;
                float m = from.y - k * from.x;
                //ax^2+bx+c=0
                float a = k * k + 1;
                float b = 2 * m * k;
                float c = m * m - radius * radius;
                float d = b * b - 4 * a * c;
                if(d < 0) 
                    return false;
                float x1 = (-b - Mathf.Sqrt(d)) / 2 / a;
                float x2 = (-b + Mathf.Sqrt(d)) / 2 / a;
                u1 = (x1 - from.x) / v.x;
                u2 = (x2 - from.x) / v.x;
            }

            if (u1 > u2)
                (u1, u2) = (u2, u1);
            uIn = Mathf.Max(uIn, u1);
            uOut = Mathf.Min(uOut, u2);
            if (uIn >= uOut)
                return false;

            float z1 = from.z + uIn * v.z;
            float z2 = from.z + uOut * v.z;
            if( z1 >= 0 && z1 < height && z2 >= 0 && z2 < height)
            {
                to = from + u2 * v;
                from += u1 * v;
                return true;
            }
            return false;
        }
        #endregion

        #region 抛物线

        /// <summary>
        /// 求抛物线初速度
        /// </summary>
        /// <param name="from">起点</param>
        /// <param name="to">终点</param>
        /// <param name="includedAngle">初速度与地面的夹角（弧度制）</param>
        /// <param name="gravity">重力加速度（沿Z轴负方向为正）</param>
        public static bool InitialVelocityOfParabola(Vector3 from, Vector3 to, float includedAngle, float gravity, out Vector3 velocity, out float time)
        {
            float h = to.z - from.z;
            Vector3 project = to - from;
            project = new Vector3(project.x, project.y, 0);
            float p = project.magnitude;
            bool feasible = InitialSpeedOfParabola(h, p, includedAngle, gravity, out float speed, out time);
            if (!feasible)
            {
                velocity = Vector3.zero;
                return false;
            }
            velocity = speed * (project + p * Mathf.Tan(includedAngle) * Vector3.forward).normalized;
            return true;
        }

        /// <summary>
        /// 求抛物线初速率
        /// </summary>
        /// <param name="h">高度改变量</param>
        /// <param name="p">xy平面内投影距离</param>
        /// <param name="includedAngle">初速度与地面的夹角（弧度制）</param>
        /// <param name="gravity">重力加速度（沿Z轴负方向为正）</param>
        public static bool InitialSpeedOfParabola(float h, float p, float includedAngle, float gravity, out float speed, out float time)
        {
            float t_square = 2 / gravity * (p * Mathf.Tan(includedAngle) - h);
            if (t_square < 0)
            {
                time = -1;
                speed = -1;
                return false;
            }
            time = Mathf.Sqrt(t_square);
            speed = p / time / Mathf.Cos(includedAngle);
            return speed >= 0;
        }

        /// <summary>
        /// 将抛物线离散化
        /// </summary>
        /// <param name="from">初始位置</param>
        /// <param name="velocity">初速度</param>
        /// <param name="gravity">重力加速度</param>
        /// <param name="time">运动时间</param>
        /// <param name="precision">精度</param>
        public static void DiscretizeParabola(Vector3 from, Vector3 velocity, float gravity, float time, float precision, List<Vector3> ret)
        {
            ret.Clear();
            int count = Mathf.CeilToInt(precision * ((Vector2)velocity).magnitude) + 1;
            float t = 0;
            for (int i = 0; i <= count; i++)
            {
                Vector3 s = from + t * velocity + t * t / 2 * gravity * Vector3.back;
                t += time / count;
                ret.Add(s);
            }
        }

        #endregion
    }
}