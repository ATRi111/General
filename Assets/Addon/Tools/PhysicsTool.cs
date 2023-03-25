using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class PhysicsTool
    {
        private readonly static Comparer_Hit comparer_hit;

        static PhysicsTool()
        {
            comparer_hit = new Comparer_Hit(Vector3.zero);
        }

        /// <summary>
        /// ��һ�������߶��Ͻ������߼�⣬ֻ���⵽��"�������߶η���ļн�С��90�����"�Ľ���
        /// </summary>
        public static RaycastHit[] InverseRaycastAll(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
        {
            return Physics.RaycastAll(origin + maxDistance * direction, -direction, maxDistance, layerMask);
        }

        /// <summary>
        /// ��һ�������߶��Ͻ������߼�⣬ֻ���⵽��"�������߶η���ļн�С��90�����"�Ľ��㣬�������������ĵ�
        /// </summary>
        public static bool InverseRaycast(Vector3 origin, Vector3 direction, out RaycastHit result, float maxDistance, int layerMask)
        {
            RaycastHit[] hits = InverseRaycastAll(origin, direction, maxDistance, layerMask);
            if (hits.Length > 0)
            {
                comparer_hit.origin = origin;
                Array.Sort(hits, comparer_hit);
                result = hits[0];
                return true;
            }
            else
            {
                result = new RaycastHit();
                return false;
            }
        }

        /// <summary>
        /// ��һ���߶��Ͻ���˫�����߼��
        /// </summary>
        public static RaycastHit[] TowWayRaycastAll(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
        {
            RaycastHit[] forward = Physics.RaycastAll(origin, direction, maxDistance, layerMask);
            RaycastHit[] backward = InverseRaycastAll(origin, direction, maxDistance, layerMask);
            RaycastHit[] ret = new RaycastHit[forward.Length + backward.Length];
            Array.Copy(forward, ret, forward.Length);
            Array.Copy(backward, 0, ret, forward.Length, backward.Length);
            return ret;
        }

        /// <summary>
        /// ��һ���߶��Ͻ���˫�����߼�⣬�������������ĵ�
        /// </summary>
        public static bool TwoWayRaycast(Vector3 origin, Vector3 direction, out RaycastHit result, float maxDistance, int layerMask)
        {
            RaycastHit NearerOf(RaycastHit hit1, RaycastHit hit2)
            {
                return (hit1.point - origin).sqrMagnitude < (hit2.point - origin).sqrMagnitude ? hit1 : hit2;
            }

            bool flag_forward = Physics.Raycast(origin, direction, out RaycastHit hit_forward, maxDistance, layerMask);
            bool flag_backward = InverseRaycast(origin, direction, out RaycastHit hit_backward, maxDistance, layerMask);

            result = (flag_forward, flag_backward) switch
            {
                (false, false) => new RaycastHit(),
                (false, true) => hit_backward,
                (true, false) => hit_forward,
                (true, true) => NearerOf(hit_backward, hit_forward),
            };

            return flag_forward || flag_backward;
        }
    }

    public class Comparer_Hit : Comparer<RaycastHit>
    {
        public Vector3 origin;

        public Comparer_Hit(Vector3 origin)
        {
            this.origin = origin;
        }

        public override int Compare(RaycastHit x, RaycastHit y)
        {
            return (int)Mathf.Sign((x.point - origin).sqrMagnitude - (y.point - origin).sqrMagnitude);
        }
    }
}