using UnityEngine;

namespace AStar.Sample
{
    /// <summary>
    /// 独立于寻路本身的相机漫游组件：以越肩视角沿 <see cref="PathFindingSample3D"/> 的寻路结果自动移动主摄像机，
    /// 并对跟随过程做平滑+避障处理。只通过 <see cref="sample"/> 这一个引用读取路径数据/查询障碍物，
    /// 不直接依赖寻路内部实现，方便单独增删这个组件而不影响寻路逻辑本身
    /// </summary>
    public class PathCameraRoamer : MonoBehaviour
    {
        [SerializeField]
        private PathFindingSample3D sample;

        /// <summary>
        /// 供 <see cref="PathFindingSample3D"/> 在按钮被按下时调用，把自己设为当前跟随的目标——
        /// 支持同一个物体上挂多个 <see cref="PathFindingSample3D"/>（比如同一张地图分别用不同的
        /// 获取相邻节点策略对比效果），点哪一个的按钮，摄像机接下来就跟随哪一个的寻路结果，
        /// 不需要每次都手动去这个组件的Inspector里重新拖引用
        /// </summary>
        internal void SetSample(PathFindingSample3D newSample) => sample = newSample;



        /// <summary>沿路径飞行时的移动速度（单位/秒），决定 <see cref="FlyCameraAlongPath"/> 里飞完全程要多久</summary>
        public float flightSpeed = 5f;

        /// <summary>越肩视角：摄像机跟在路径当前点身后多远（沿移动方向反方向偏移）</summary>
        public float shoulderBackDistance = 3f;
        /// <summary>越肩视角：摄像机比路径当前点高多少</summary>
        public float shoulderUpDistance = 1.5f;
        /// <summary>越肩视角：摄像机偏向路径当前点右侧多少（负值则偏向左肩）</summary>
        public float shoulderSideDistance = 1f;

        /// <summary>
        /// 摄像机跟随目标位置的平滑速度：值越大跟得越紧（越"跟手"），值越小越有延迟的跟随感。
        /// 用指数平滑 <c>1-e^(-speed*dt)</c> 而不是固定插值系数，效果不受帧率影响
        /// </summary>
        public float positionSmoothSpeed = 6f;
        /// <summary>
        /// 摄像机朝向跟随移动方向的平滑速度，同上。路径在拐角处的方向是瞬间反转的（折线，不是曲线），
        /// 没有这层平滑的话镜头会跟着瞬间转向，非常生硬——这个值就是用来把"瞬间转向"拉成"有一点跟随延迟的转向"
        /// </summary>
        public float rotationSmoothSpeed = 6f;

        /// <summary>沿摄像机期望位置方向做碰撞检测时的采样间隔（单位：格），越小越精确但开销越高</summary>
        private const float cameraCollisionCheckStep = 0.2f;
        /// <summary>摄像机与检测到的障碍物之间保留的间隙，避免摄像机贴脸导致近裁剪面穿模</summary>
        private const float cameraCollisionClearance = 0.3f;

        /// <summary>
        /// 持有当前正在播放的相机飞行动画，仅用于避免被GC提前回收
        /// （<see cref="MyTimer.Timer{TValue,TLerp}"/> 内部通过 <see cref="MyTimer.GameCycle"/> 驱动，
        /// 不需要在这里手动Update，但对象本身要有地方持有引用）
        /// </summary>
        private MyTimer.UniformFoldLineMotion cameraFlight;

        /// <summary>
        /// 让主摄像机以越肩视角沿 <see cref="sample"/> 的寻路结果自动移动——
        /// 用 <see cref="MyTimer.UniformFoldLineMotion"/> 驱动一个虚拟"角色"沿折线匀速前进
        /// （按每段实际距离比例分配时间，不是在节点间简单等分时间）；摄像机本身不直接摆在路径点上，
        /// 而是通过 <see cref="UpdateShoulderCamera"/> 跟在这个"角色"身后偏一侧、偏上一点的位置，
        /// 朝向角色前方（移动方向），并对这个跟随过程做平滑+避障处理
        /// </summary>
        public void FlyCameraAlongPath()
        {
            if (sample == null)
            {
                Debug.LogWarning("PathCameraRoamer没有指定sample（PathFindingSample3D），无法沿路径飞行");
                return;
            }

            Vector3[] points = sample.GetOutputPathWorldPoints();
            if (points.Length < 2)
            {
                Debug.LogWarning("寻路结果的节点数不足2个，无法沿路径飞行（请先完成寻路）");
                return;
            }

            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogWarning("场景里没有找到MainCamera，无法沿路径飞行");
                return;
            }

            float length = 0f;
            for (int i = 1; i < points.Length; i++)
                length += Vector3.Distance(points[i], points[i - 1]);

            Vector3 lastPos = points[0];
            Vector3 lastDirection = (points[1] - points[0]).normalized;
            UpdateShoulderCamera(cam, lastPos, lastDirection, snap: true);

            cameraFlight = new MyTimer.UniformFoldLineMotion();
            cameraFlight.OnTick += pos =>
            {
                // FoldLineLerp只提供位置，没有现成的方向可用，改用"和上一帧位置的差"反推当前帧的移动方向；
                // 只有方向发生实际变化时才更新lastDirection，避免同一段直线内因浮点误差抖动
                Vector3 direction = pos - lastPos;
                if (direction.sqrMagnitude > 0.0001f)
                    lastDirection = direction.normalized;
                UpdateShoulderCamera(cam, pos, lastDirection, snap: false);
                lastPos = pos;
            };
            cameraFlight.Initialize(points, length, length / Mathf.Max(flightSpeed, 0.01f));
        }

        /// <summary>
        /// 越肩视角：摄像机不摆在"角色"(<paramref name="subjectPos"/>)本身位置上，而是摆在其身后偏一侧、
        /// 偏上的位置（先经过 <see cref="ResolveCameraObstruction"/> 避障处理），朝向角色前方。
        /// 侧向轴用世界上方向和移动方向的叉乘得到，移动方向接近正上/正下时叉乘会退化为零向量，
        /// 这种情况下沿用摄像机当前的right轴，避免瞬间转向。
        /// <paramref name="snap"/>为true时直接跳到目标位置/朝向（用于飞行开始的第一帧），
        /// 否则用<see cref="positionSmoothSpeed"/>/<see cref="rotationSmoothSpeed"/>做平滑跟随，
        /// 避免路径拐角处方向瞬间反转导致镜头瞬间转向的生硬感
        /// </summary>
        private void UpdateShoulderCamera(Camera cam, Vector3 subjectPos, Vector3 direction, bool snap)
        {
            Vector3 right = Vector3.Cross(Vector3.up, direction);
            right = right.sqrMagnitude > 0.0001f ? right.normalized : cam.transform.right;

            Vector3 desiredPos = subjectPos - direction * shoulderBackDistance
                + Vector3.up * shoulderUpDistance
                + right * shoulderSideDistance;
            Vector3 safePos = ResolveCameraObstruction(subjectPos, desiredPos);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            if (snap)
            {
                cam.transform.SetPositionAndRotation(safePos, targetRotation);
                return;
            }

            float dt = Time.deltaTime;
            float posT = 1f - Mathf.Exp(-positionSmoothSpeed * dt);
            float rotT = 1f - Mathf.Exp(-rotationSmoothSpeed * dt);
            cam.transform.SetPositionAndRotation(
                Vector3.Lerp(cam.transform.position, safePos, posT),
                Quaternion.Slerp(cam.transform.rotation, targetRotation, rotT));
        }

        /// <summary>
        /// 越肩视角的避障：场景里的障碍物（<see cref="AStarSample3DBuilder"/> 生成的Block）创建时就故意去掉了
        /// Collider（避免物理查询的时序/精度问题），所以不能用Physics.Raycast，改成沿"角色"到期望摄像机位置
        /// 这条线段，按固定步长采样，复用 <see cref="sample"/> 已有的 <see cref="PathFindingSample3D.IsBlockAt"/>
        /// 格子查表判断有没有撞进障碍物——一旦命中，就把摄像机沿这条线段拉回命中点前面
        /// <see cref="cameraCollisionClearance"/> 的地方，而不是让它穿进墙体/建筑内部
        /// </summary>
        private Vector3 ResolveCameraObstruction(Vector3 subjectPos, Vector3 desiredCamPos)
        {
            Vector3 offset = desiredCamPos - subjectPos;
            float totalDistance = offset.magnitude;
            if (totalDistance < 0.0001f)
                return desiredCamPos;

            Vector3 direction = offset / totalDistance;
            int stepCount = Mathf.Max(1, Mathf.CeilToInt(totalDistance / cameraCollisionCheckStep));

            float safeDistance = totalDistance;
            for (int i = 1; i <= stepCount; i++)
            {
                float d = totalDistance * i / stepCount;
                if (sample.IsBlockAt(sample.WorldToNode(subjectPos + direction * d)))
                {
                    safeDistance = Mathf.Max(0f, d - cameraCollisionClearance);
                    break;
                }
            }
            return subjectPos + direction * safeDistance;
        }
    }
}
