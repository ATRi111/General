using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AStar.Sample
{
    public static class AStarSample3DBuilder
    {
        private const string ResFolder = "Assets/Addon/Astar/Sample3D/Res";

        private const float ObstacleScale = 1f;
        /// <summary>地形/建筑之上再留出多少格的飞行/通行空间，供摄像机取景时预留</summary>
        private const int AirBuffer = 3;

        /// <summary>
        /// 等距视角：绕X轴俯视45°，再绕Y轴转45°。
        /// 在这个角度下，画面"越靠上"对应世界坐标 x+z 越大。
        /// </summary>
        private static readonly Quaternion IsometricRotation = Quaternion.Euler(45f, 45f, 0f);

        #region 山地(Hill)方案：从起点一侧低、往终点一侧高的连续坡地

        private const int HillSizeX = 10;
        private const int HillSizeZ = 10;
        /// <summary>地形最低高度（起点附近）</summary>
        private const int HillMinHeight = 1;
        /// <summary>地形最高高度（终点附近，还会叠加随机小山丘，实际可能更高）</summary>
        private const int HillMaxHeight = 6;

        /// <summary>
        /// 障碍物调色板：仿照参考图里石头/泥土/草地的配色，按绝对高度(y)取色——同一高度的地块无论在哪一列都用同种颜色
        /// </summary>
        private static readonly Color[] HillBlockPalette =
        {
            new(0.55f, 0.55f, 0.50f), // 浅灰石
            new(0.45f, 0.38f, 0.28f), // 泥土棕
            new(0.35f, 0.55f, 0.30f), // 草绿
            new(0.62f, 0.52f, 0.35f), // 沙黄
            new(0.42f, 0.44f, 0.48f), // 青灰石
            new(0.58f, 0.32f, 0.28f), // 红陶
        };

        [MenuItem("Tools/AStar/搭建3D寻路示例场景（山地Hill）")]
        public static void BuildHill()
        {
            EnsureFolder(ResFolder);

            System.Random rng = new(12345);
            int[,] heights = BuildHeightMap(rng);

            GameObject root = new("Hill");

            // y 取该列地形高度之上半格，确保起点/终点落在地面以上，而不是被埋进地形里
            CreateMarker("From", new Vector3(0.5f, heights[0, 0] + 0.5f, 0.5f), Color.green, root.transform);
            CreateMarker("To", new Vector3(HillSizeX - 0.5f, heights[HillSizeX - 1, HillSizeZ - 1] + 0.5f, HillSizeZ - 0.5f), Color.cyan, root.transform);

            int verticalExtent = HillMaxHeight + 2 + AirBuffer;
            Vector3Int gridSize = new(HillSizeX, verticalExtent, HillSizeZ);

            BuildTerrain(root.transform, heights);
            SetupIsometricCamera(gridSize);

            Selection.activeGameObject = root;
        }

        /// <summary>
        /// 生成整片地形的高度图：沿 x+z 方向从 <see cref="HillMinHeight"/> 线性升高到 <see cref="HillMaxHeight"/>，
        /// 再叠加少量随机抖动和偶尔出现的小山丘，让坡地不至于完全平滑
        /// </summary>
        private static int[,] BuildHeightMap(System.Random rng)
        {
            int[,] heights = new int[HillSizeX, HillSizeZ];
            float maxT = (HillSizeX - 1) + (HillSizeZ - 1);

            for (int x = 0; x < HillSizeX; x++)
            {
                for (int z = 0; z < HillSizeZ; z++)
                {
                    float t = (x + z) / maxT;
                    int baseHeight = Mathf.RoundToInt(Mathf.Lerp(HillMinHeight, HillMaxHeight, t));
                    int jitter = rng.Next(-1, 2);
                    int hill = rng.Next(0, 100) < 20 ? rng.Next(1, 3) : 0;
                    heights[x, z] = Mathf.Clamp(baseHeight + jitter + hill, HillMinHeight, HillMaxHeight + 2);
                }
            }
            return heights;
        }

        /// <summary>按绝对高度(y)取调色板颜色，保证同一高度的地块无论在哪一列都是同种颜色</summary>
        private static Color GetHillLayerColor(int y) => HillBlockPalette[y % HillBlockPalette.Length];

        /// <summary>
        /// 按高度图把整片地形从地面(y=0)填到各自的高度，每一列内部连续没有空洞，
        /// 也不会出现悬空的孤立方块——因此整块地形没有任何从外部看不到的中空区域
        /// </summary>
        private static void BuildTerrain(Transform mount, int[,] heights)
        {
            Dictionary<Color, Material> materialCache = new();
            Material GetMaterial(Color color) => GetOrCreateMaterial(materialCache, color);

            for (int x = 0; x < HillSizeX; x++)
            {
                for (int z = 0; z < HillSizeZ; z++)
                {
                    int height = heights[x, z];

                    for (int y = 0; y < height; y++)
                    {
                        // 不再对每列的顶层单独提亮——那样会导致"同一高度"在不同列上出现两种颜色
                        // （某列的顶层被提亮，另一列在这个高度只是側面/非顶层，仍是原色），
                        // 统一只按绝对高度取色，保证同一高度的地块颜色完全一致
                        CreateObstacleCube("Block", new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), GetMaterial(GetHillLayerColor(y)), mount);
                    }
                }
            }
        }

        #endregion

        #region 城市(City)方案：大片平整地面 + 若干大小不一的建筑

        private const int CitySizeX = 100;
        private const int CitySizeZ = 100;
        /// <summary>
        /// 固定的竖直网格范围（不再像 <see cref="BuildHill"/> 那样由"最高高度+缓冲"反推）——
        /// 下面 CityGroundHeight(1) + CityMaxFloors(6)*CityFloorHeight(6) + 屋顶(1) = 38，
        /// 剩下2格纯空气留作摄像机取景缓冲；如果以后调大 CityMaxFloors，要记得同步调大这里，否则最高的楼会被边界削平
        /// </summary>
        private const int CitySizeY = 40;
        /// <summary>地面厚度（起点/终点站在地面正上方）</summary>
        private const int CityGroundHeight = 1;

        /// <summary>每层的实心地板/天花板厚度</summary>
        private const int CityFloorSlabHeight = 1;
        /// <summary>每层内部中空部分的高度（只有四周墙体，内部是空气，可以走进去）——
        /// 层数减少的同时把这个值调大，让每层本身更高</summary>
        private const int CityFloorInteriorHeight = 5;
        private const int CityFloorHeight = CityFloorSlabHeight + CityFloorInteriorHeight;
        private const int CityMinFloors = 3;
        private const int CityMaxFloors = 6;

        /// <summary>
        /// 每层每条边上，窗户开口占该边可用长度（去掉两端角落后）的比例范围——用一整段连续区间
        /// （见 <see cref="GetWindowSpan"/>），而不是逐格独立掷概率，让开口看起来像一整块落地窗，
        /// 且平均开口面积明显更大
        /// </summary>
        private const float CityWindowSpanMinRatio = 0.35f;
        private const float CityWindowSpanMaxRatio = 0.65f;

        /// <summary>至少3格见方才有"内部中空"的意义——四周墙体各占1格，中间还要留至少1格空气；
        /// 继续调大足迹以提高建筑占比</summary>
        private const int CityMinFootprint = 14;
        private const int CityMaxFootprint = 28;
        /// <summary>
        /// 建筑数量按地图面积算比例，而不是固定值；除数按"期望建筑覆盖率约40%、平均单栋建筑足迹约
        /// (14~28边长)²≈441格"倒推：面积*40%/441 ≈ 面积/1100，100×100时约剩9栋（含2栋锚点建筑）——
        /// 比上一版(约17%覆盖率、12栋)占比更高，数量因为单栋更大反而没有再增加
        /// </summary>
        private const int CityCellsPerBuilding = 1100;

        /// <summary>地面色：统一的人行道浅灰，和建筑拉开区分</summary>
        private static readonly Color CityGroundColor = new(0.60f, 0.60f, 0.58f);

        /// <summary>建筑外墙改成浅蓝色玻璃幕墙（类似落地窗）；屋顶不用这个颜色，和地板一样按建筑调色板上色</summary>
        private static readonly Color CityGlassColor = new(0.62f, 0.80f, 0.88f);

        /// <summary>
        /// 楼层地板与屋顶共用的调色板：按建筑编号轮换，让每栋楼的地板/屋顶（外墙统一变成玻璃后，
        /// 这两处是唯一还能看出"这是不同的建筑"的部位）保留一点彼此的区分
        /// </summary>
        private static readonly Color[] CityBuildingPalette =
        {
            new(0.32f, 0.48f, 0.62f), // 玻璃幕墙蓝
            new(0.72f, 0.68f, 0.60f), // 混凝土米黄
            new(0.58f, 0.30f, 0.28f), // 砖红
            new(0.40f, 0.40f, 0.44f), // 现代灰
            new(0.55f, 0.50f, 0.42f), // 石材棕
        };

        private struct CityBuilding
        {
            public int x0, z0, width, depth, floorCount;
        }

        [MenuItem("Tools/AStar/搭建3D寻路示例场景（城市City）")]
        public static void BuildCity()
        {
            EnsureFolder(ResFolder);

            System.Random rng = new(54321);
            List<CityBuilding> buildings = BuildCityLayout(rng, out CityBuilding fromBuilding, out CityBuilding toBuilding);

            GameObject root = new("City");

            // 起点/终点放在各自那栋"锚点建筑"内部（首层中空部分的中心），而不是露天地面上；
            // 这两栋建筑在 BuildCityLayout 里已经保证一定存在、且大小固定为 CityMaxFootprint，
            // 中心格必定落在中空内部（不会正好卡在墙体格上）
            CreateMarker("From", GetBuildingInteriorMarkerPosition(fromBuilding), Color.green, root.transform);
            CreateMarker("To", GetBuildingInteriorMarkerPosition(toBuilding), Color.cyan, root.transform);

            Vector3Int gridSize = new(CitySizeX, CitySizeY, CitySizeZ);

            BuildCityBlocks(root.transform, buildings, rng);
            SetupIsometricCamera(gridSize);

            Selection.activeGameObject = root;
        }

        /// <summary>
        /// 随机生成若干栋不重叠的建筑（矩形足迹+随机楼层数）。
        /// 起点/终点需要落在建筑内部，先在两个角落各放一栋固定大小(<see cref="CityMaxFootprint"/>)的
        /// "锚点建筑"（一定生成，通过 <paramref name="fromBuilding"/>/<paramref name="toBuilding"/> 传出去），
        /// 再用简单的拒绝采样在剩余空间里随机填充其余建筑：随机出一个矩形，若与已占用格子重叠就重试
        /// </summary>
        private static List<CityBuilding> BuildCityLayout(System.Random rng, out CityBuilding fromBuilding, out CityBuilding toBuilding)
        {
            bool[,] occupied = new bool[CitySizeX, CitySizeZ];
            List<CityBuilding> buildings = new();

            fromBuilding = CreateAnchorBuilding(rng, 0, 0, occupied);
            toBuilding = CreateAnchorBuilding(rng, CitySizeX - CityMaxFootprint, CitySizeZ - CityMaxFootprint, occupied);
            buildings.Add(fromBuilding);
            buildings.Add(toBuilding);

            int buildingCount = Mathf.Max(buildings.Count, CitySizeX * CitySizeZ / CityCellsPerBuilding);
            int attempts = 0;
            int maxAttempts = buildingCount * 20;
            while (buildings.Count < buildingCount && attempts < maxAttempts)
            {
                attempts++;
                int width = rng.Next(CityMinFootprint, CityMaxFootprint + 1);
                int depth = rng.Next(CityMinFootprint, CityMaxFootprint + 1);
                int x0 = rng.Next(0, CitySizeX - width + 1);
                int z0 = rng.Next(0, CitySizeZ - depth + 1);

                bool overlap = false;
                for (int x = x0; x < x0 + width && !overlap; x++)
                {
                    for (int z = z0; z < z0 + depth; z++)
                    {
                        if (occupied[x, z])
                        {
                            overlap = true;
                            break;
                        }
                    }
                }
                if (overlap)
                    continue;

                for (int x = x0; x < x0 + width; x++)
                    for (int z = z0; z < z0 + depth; z++)
                        occupied[x, z] = true;

                int floorCount = rng.Next(CityMinFloors, CityMaxFloors + 1);
                buildings.Add(new CityBuilding { x0 = x0, z0 = z0, width = width, depth = depth, floorCount = floorCount });
            }
            return buildings;
        }

        /// <summary>
        /// 在指定角落放置一栋固定 <see cref="CityMaxFootprint"/> 大小的建筑，并把它的足迹标记为已占用
        /// （供 <see cref="BuildCityLayout"/> 里后续的随机布局避开）。固定用最大足迹，保证内部中空区域
        /// 足够大、中心格离墙体够远，不会因为随机到最小足迹(3格)而让"中心"意外落在墙上
        /// </summary>
        private static CityBuilding CreateAnchorBuilding(System.Random rng, int x0, int z0, bool[,] occupied)
        {
            int width = CityMaxFootprint;
            int depth = CityMaxFootprint;
            x0 = Mathf.Clamp(x0, 0, CitySizeX - width);
            z0 = Mathf.Clamp(z0, 0, CitySizeZ - depth);

            for (int x = x0; x < x0 + width; x++)
                for (int z = z0; z < z0 + depth; z++)
                    occupied[x, z] = true;

            int floorCount = rng.Next(CityMinFloors, CityMaxFloors + 1);
            return new CityBuilding { x0 = x0, z0 = z0, width = width, depth = depth, floorCount = floorCount };
        }

        /// <summary>
        /// 建筑首层中空部分的中心位置——足迹中心格 + 首层地板正上方半格（<see cref="CityFloorInteriorHeight"/>
        /// 中空区间的第一格），保证一定落在内部空气里，不会卡在墙体或地板上
        /// </summary>
        private static Vector3 GetBuildingInteriorMarkerPosition(CityBuilding building)
        {
            int centerX = building.x0 + building.width / 2;
            int centerZ = building.z0 + building.depth / 2;
            float y = CityGroundHeight + CityFloorSlabHeight + 0.5f;
            return new Vector3(centerX + 0.5f, y, centerZ + 0.5f);
        }

        /// <summary>
        /// 在 [0, length) 范围内随机截出一段窗户开口区间（起点+长度），长度占整段的
        /// <see cref="CityWindowSpanMinRatio"/>~<see cref="CityWindowSpanMaxRatio"/>，
        /// 保证开口是连续一整块（类似落地窗），而不是像逐格独立掷概率那样开口零散、平均面积小
        /// </summary>
        private static void GetWindowSpan(System.Random rng, int length, out int start, out int len)
        {
            float ratio = Mathf.Lerp(CityWindowSpanMinRatio, CityWindowSpanMaxRatio, (float)rng.NextDouble());
            len = Mathf.Clamp(Mathf.RoundToInt(length * ratio), 1, length);
            start = rng.Next(0, length - len + 1);
        }

        private static bool InSpan(int index, int start, int len) => index >= start && index < start + len;

        /// <summary>
        /// 先铺满一层地面，再把每栋建筑逐层往上叠：每层由"实心地板"(同时也是下一层的天花板，用建筑各自
        /// 的调色板着色)+"浅蓝色玻璃墙体"两部分组成——墙体只沿建筑四周生成，内部整层留空（可以从缺口走进去），
        /// 四条边各自独立截出一段连续的窗户开口（见 <see cref="GetWindowSpan"/>），保证每层至少4处、
        /// 且面积明显更大的落地窗缺口；最后再盖一层实心屋顶，颜色和地板一样（同用建筑调色板，不用玻璃色）
        /// </summary>
        private static void BuildCityBlocks(Transform mount, List<CityBuilding> buildings, System.Random rng)
        {
            Dictionary<Color, Material> materialCache = new();
            Material GetMaterial(Color color) => GetOrCreateMaterial(materialCache, color);

            for (int x = 0; x < CitySizeX; x++)
            {
                for (int z = 0; z < CitySizeZ; z++)
                {
                    for (int y = 0; y < CityGroundHeight; y++)
                    {
                        CreateObstacleCube("Block", new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), GetMaterial(CityGroundColor), mount);
                    }
                }
            }

            Material glassMaterial = GetMaterial(CityGlassColor);

            for (int i = 0; i < buildings.Count; i++)
            {
                CityBuilding building = buildings[i];
                Color floorColor = CityBuildingPalette[i % CityBuildingPalette.Length];

                int x1 = building.x0 + building.width - 1;
                int z1 = building.z0 + building.depth - 1;
                // 四条边各自非角落格子的数量：z0/z1边沿x方向排列，x0/x1边沿z方向排列
                int sideLenX = building.width - 2;
                int sideLenZ = building.depth - 2;

                for (int floor = 0; floor < building.floorCount; floor++)
                {
                    int floorBaseY = CityGroundHeight + floor * CityFloorHeight;

                    // 楼层地板：整片实心，同时也是下一层（或屋顶）的地基
                    for (int x = building.x0; x <= x1; x++)
                        for (int z = building.z0; z <= z1; z++)
                            CreateObstacleCube("Block", new Vector3(x + 0.5f, floorBaseY + 0.5f, z + 0.5f), GetMaterial(floorColor), mount);

                    // 四条边各自独立截出一段随机长度、随机位置的落地窗开口
                    GetWindowSpan(rng, sideLenX, out int spanZ0Start, out int spanZ0Len);
                    GetWindowSpan(rng, sideLenX, out int spanZ1Start, out int spanZ1Len);
                    GetWindowSpan(rng, sideLenZ, out int spanX0Start, out int spanX0Len);
                    GetWindowSpan(rng, sideLenZ, out int spanX1Start, out int spanX1Len);

                    // 墙体：只沿四周生成，内部留空；四个角落始终保留（撑住结构），
                    // 其余墙面格落在上面截出的开口区间内就跳过，形成落地窗缺口
                    for (int wallY = floorBaseY + 1; wallY <= floorBaseY + CityFloorInteriorHeight; wallY++)
                    {
                        for (int x = building.x0; x <= x1; x++)
                        {
                            for (int z = building.z0; z <= z1; z++)
                            {
                                bool onPerimeter = x == building.x0 || x == x1 || z == building.z0 || z == z1;
                                if (!onPerimeter)
                                    continue;    //内部留空，可以从窗户缺口走进去

                                bool isCorner = (x == building.x0 || x == x1) && (z == building.z0 || z == z1);
                                if (!isCorner)
                                {
                                    bool isWindow =
                                        (z == building.z0 && InSpan(x - building.x0 - 1, spanZ0Start, spanZ0Len)) ||
                                        (z == z1 && InSpan(x - building.x0 - 1, spanZ1Start, spanZ1Len)) ||
                                        (x == building.x0 && InSpan(z - building.z0 - 1, spanX0Start, spanX0Len)) ||
                                        (x == x1 && InSpan(z - building.z0 - 1, spanX1Start, spanX1Len));
                                    if (isWindow)
                                        continue;    //落地窗开口
                                }

                                CreateObstacleCube("Block", new Vector3(x + 0.5f, wallY + 0.5f, z + 0.5f), glassMaterial, mount);
                            }
                        }
                    }
                }

                // 屋顶：整片实心，颜色和地板一样（同用建筑调色板），不用玻璃色
                int roofY = CityGroundHeight + building.floorCount * CityFloorHeight;
                for (int x = building.x0; x <= x1; x++)
                    for (int z = building.z0; z <= z1; z++)
                        CreateObstacleCube("Block", new Vector3(x + 0.5f, roofY + 0.5f, z + 0.5f), GetMaterial(floorColor), mount);
            }
        }

        #endregion

        #region 高墙迷宫(Wall)方案：50×50平地上散布高墙+若干矮于墙高的平台

        private const int WallSizeX = 50;
        private const int WallSizeZ = 50;
        /// <summary>地面厚度（起点站在地面正上方）</summary>
        private const int WallGroundHeight = 1;
        /// <summary>高墙的高度（从地面往上算），比任何平台都高——逼迫寻路只能绕开墙的两端，飞不过去/爬不上去</summary>
        private const int WallHeight = 10;
        /// <summary>墙体厚度固定为1格，长度在下面范围内随机，方向随机取"沿x"或"沿z"</summary>
        private const int WallMinLength = 6;
        private const int WallMaxLength = 16;
        /// <summary>墙的数量按地图面积/该值估算，值越小墙越密</summary>
        private const int WallCellsPerWall = 55;
        /// <summary>普通平台的正方形足迹边长范围</summary>
        private const int WallPlatformMinFootprint = 4;
        private const int WallPlatformMaxFootprint = 8;
        /// <summary>普通平台的高度范围，必须严格低于 <see cref="WallHeight"/>，否则会失去"平台矮墙高"的对比</summary>
        private const int WallPlatformMinHeight = 3;
        private const int WallPlatformMaxHeight = 6;
        private const int WallPlatformCellsPerPlatform = 220;
        /// <summary>终点所在的"锚点平台"固定足迹，比普通平台更大，保证中心格离边缘足够远</summary>
        private const int WallGoalPlatformFootprint = 8;
        /// <summary>终点锚点平台的高度，取普通平台的最高值，明确低于墙高</summary>
        private const int WallGoalPlatformHeight = WallPlatformMaxHeight;
        /// <summary>起点/终点各自预留的空地边长，生成墙/平台时会避开这片区域，保证起终点不会被卡在障碍物里</summary>
        private const int WallClearZoneSize = 3;

        private static readonly Color WallGroundColor = new(0.42f, 0.46f, 0.40f); // 灰绿地面
        private static readonly Color WallStoneColor = new(0.30f, 0.30f, 0.32f); // 深灰石墙
        private static readonly Color WallGoalPlatformColor = new(0.85f, 0.65f, 0.20f); // 金黄，突出终点平台
        private static readonly Color[] WallPlatformPalette =
        {
            new(0.55f, 0.42f, 0.30f), // 木棕
            new(0.50f, 0.50f, 0.55f), // 石灰
            new(0.35f, 0.50f, 0.42f), // 苔绿
        };

        private struct WallSegment { public int x0, z0, length; public bool horizontal; }
        private struct WallPlatform { public int x0, z0, footprint, height; public bool isGoal; }

        [MenuItem("Tools/AStar/搭建3D寻路示例场景（高墙迷宫Wall）")]
        public static void BuildWall()
        {
            EnsureFolder(ResFolder);

            System.Random rng = new(20240609);
            (List<WallSegment> walls, List<WallPlatform> platforms, WallPlatform goalPlatform) = BuildWallLayout(rng);

            GameObject root = new("Wall");

            // From站在地面上（左下角预留空地中心），To站在终点锚点平台顶面中心
            CreateMarker("From", new Vector3(WallClearZoneSize / 2f, WallGroundHeight + 0.5f, WallClearZoneSize / 2f), Color.green, root.transform);
            CreateMarker("To", GetWallPlatformTopMarkerPosition(goalPlatform), Color.cyan, root.transform);

            Vector3Int gridSize = new(WallSizeX, WallGroundHeight + WallHeight + AirBuffer, WallSizeZ);

            BuildWallBlocks(root.transform, walls, platforms);
            SetupIsometricCamera(gridSize);

            Selection.activeGameObject = root;
        }

        /// <summary>
        /// 随机生成墙体与平台的布局：先各自预留起点角落（左下）与终点锚点平台（右上，固定足迹，一定生成）
        /// 两片区域，标记进占用网格；再用拒绝采样先随机撒墙、再随机撒普通平台——顺序很重要：
        /// 墙先撒，保证墙的分布不受平台挤占；平台后撒，允许平台紧贴墙边（更像"墙内藏平台"的地形），
        /// 但两者都不会互相重叠或落进起终点的预留空地
        /// </summary>
        private static (List<WallSegment> walls, List<WallPlatform> platforms, WallPlatform goalPlatform) BuildWallLayout(System.Random rng)
        {
            bool[,] occupied = new bool[WallSizeX, WallSizeZ];
            MarkOccupied(occupied, 0, 0, WallClearZoneSize, WallClearZoneSize);

            int goalX0 = WallSizeX - WallGoalPlatformFootprint - 1;
            int goalZ0 = WallSizeZ - WallGoalPlatformFootprint - 1;
            WallPlatform goalPlatform = new()
            {
                x0 = goalX0,
                z0 = goalZ0,
                footprint = WallGoalPlatformFootprint,
                height = WallGoalPlatformHeight,
                isGoal = true,
            };
            // 预留比足迹本身再宽1格的空地，避免墙紧贴平台边缘导致终点平台被完全包围、绕不进去
            MarkOccupied(occupied, goalX0 - 1, goalZ0 - 1, WallGoalPlatformFootprint + 2, WallGoalPlatformFootprint + 2);

            List<WallPlatform> platforms = new() { goalPlatform };

            List<WallSegment> walls = new();
            int wallCount = WallSizeX * WallSizeZ / WallCellsPerWall;
            int wallAttempts = 0;
            int wallMaxAttempts = wallCount * 30;
            while (walls.Count < wallCount && wallAttempts < wallMaxAttempts)
            {
                wallAttempts++;
                bool horizontal = rng.Next(0, 2) == 0;
                int length = rng.Next(WallMinLength, WallMaxLength + 1);
                int width = horizontal ? length : 1;
                int depth = horizontal ? 1 : length;
                int x0 = rng.Next(0, WallSizeX - width + 1);
                int z0 = rng.Next(0, WallSizeZ - depth + 1);

                if (IsOccupied(occupied, x0, z0, width, depth))
                    continue;

                MarkOccupied(occupied, x0, z0, width, depth);
                walls.Add(new WallSegment { x0 = x0, z0 = z0, length = length, horizontal = horizontal });
            }

            int platformCount = WallSizeX * WallSizeZ / WallPlatformCellsPerPlatform;
            int platformAttempts = 0;
            int platformMaxAttempts = platformCount * 30;
            while (platforms.Count < platformCount && platformAttempts < platformMaxAttempts)
            {
                platformAttempts++;
                int footprint = rng.Next(WallPlatformMinFootprint, WallPlatformMaxFootprint + 1);
                int x0 = rng.Next(0, WallSizeX - footprint + 1);
                int z0 = rng.Next(0, WallSizeZ - footprint + 1);

                if (IsOccupied(occupied, x0, z0, footprint, footprint))
                    continue;

                MarkOccupied(occupied, x0, z0, footprint, footprint);
                int height = rng.Next(WallPlatformMinHeight, WallPlatformMaxHeight + 1);
                platforms.Add(new WallPlatform { x0 = x0, z0 = z0, footprint = footprint, height = height, isGoal = false });
            }

            return (walls, platforms, goalPlatform);
        }

        private static bool IsOccupied(bool[,] occupied, int x0, int z0, int width, int depth)
        {
            for (int x = x0; x < x0 + width; x++)
                for (int z = z0; z < z0 + depth; z++)
                    if (occupied[x, z])
                        return true;
            return false;
        }

        private static void MarkOccupied(bool[,] occupied, int x0, int z0, int width, int depth)
        {
            x0 = Mathf.Max(x0, 0);
            z0 = Mathf.Max(z0, 0);
            int x1 = Mathf.Min(x0 + width, occupied.GetLength(0));
            int z1 = Mathf.Min(z0 + depth, occupied.GetLength(1));
            for (int x = x0; x < x1; x++)
                for (int z = z0; z < z1; z++)
                    occupied[x, z] = true;
        }

        /// <summary>终点锚点平台顶面中心的世界坐标——平台是实心填满的，顶面正上方半格即可站稳，不会卡进平台内部</summary>
        private static Vector3 GetWallPlatformTopMarkerPosition(WallPlatform platform)
        {
            int centerX = platform.x0 + platform.footprint / 2;
            int centerZ = platform.z0 + platform.footprint / 2;
            float y = WallGroundHeight + platform.height + 0.5f;
            return new Vector3(centerX + 0.5f, y, centerZ + 0.5f);
        }

        /// <summary>
        /// 先铺满整片地面，再把每段墙体从地面往上实心堆到 <see cref="WallHeight"/>，
        /// 最后把每个平台（含终点锚点平台）从地面往上实心堆到各自的高度——平台高度严格低于墙高，
        /// 这样"绕开墙的两端"和"直接走上/飞过平台"才会呈现出真正的高度差异，而不是所有障碍物一样高
        /// </summary>
        private static void BuildWallBlocks(Transform mount, List<WallSegment> walls, List<WallPlatform> platforms)
        {
            Dictionary<Color, Material> materialCache = new();
            Material GetMaterial(Color color) => GetOrCreateMaterial(materialCache, color);

            Material groundMaterial = GetMaterial(WallGroundColor);
            for (int x = 0; x < WallSizeX; x++)
                for (int z = 0; z < WallSizeZ; z++)
                    for (int y = 0; y < WallGroundHeight; y++)
                        CreateObstacleCube("Block", new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), groundMaterial, mount);

            Material wallMaterial = GetMaterial(WallStoneColor);
            foreach (WallSegment wall in walls)
            {
                int width = wall.horizontal ? wall.length : 1;
                int depth = wall.horizontal ? 1 : wall.length;
                for (int x = wall.x0; x < wall.x0 + width; x++)
                {
                    for (int z = wall.z0; z < wall.z0 + depth; z++)
                    {
                        for (int y = WallGroundHeight; y < WallGroundHeight + WallHeight; y++)
                        {
                            CreateObstacleCube("Block", new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), wallMaterial, mount);
                        }
                    }
                }
            }

            // 终点锚点平台用专属金黄色，方便在画面里一眼认出终点所在的平台；其余按调色板轮换取色
            int normalIndex = 0;
            foreach (WallPlatform platform in platforms)
            {
                Material material = platform.isGoal
                    ? GetMaterial(WallGoalPlatformColor)
                    : GetMaterial(WallPlatformPalette[normalIndex++ % WallPlatformPalette.Length]);
                BuildOnePlatform(mount, platform, material);
            }
        }

        private static void BuildOnePlatform(Transform mount, WallPlatform platform, Material material)
        {
            for (int x = platform.x0; x < platform.x0 + platform.footprint; x++)
                for (int z = platform.z0; z < platform.z0 + platform.footprint; z++)
                    for (int y = WallGroundHeight; y < WallGroundHeight + platform.height; y++)
                        CreateObstacleCube("Block", new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), material, mount);
        }

        #endregion

        #region 三套方案共用：策略SO资源、Debug预制体、PathFindingSample3D组件挂载、等距摄像机

        private static Material GetOrCreateMaterial(Dictionary<Color, Material> cache, Color color)
        {
            if (!cache.TryGetValue(color, out Material material))
            {
                material = CreateColorMaterial(color);
                cache[color] = material;
            }
            return material;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            string parent = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
            string folderName = System.IO.Path.GetFileName(path);
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, folderName);
        }

        private static Material CreateColorMaterial(Color color)
        {
            return new Material(Shader.Find("Sprites/Default")) { color = color };
        }

        private static GameObject CreateMarker(string name, Vector3 worldPos, Color color, Transform parent)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            marker.name = name;
            Object.DestroyImmediate(marker.GetComponent<Collider>());
            marker.transform.SetParent(parent, false);
            marker.transform.position = worldPos;
            marker.transform.localScale = Vector3.one * 0.3f;
            marker.GetComponent<Renderer>().sharedMaterial = CreateColorMaterial(color);
            return marker;
        }

        /// <summary>摄像机视野角度，透视投影下用它反推出能框住整个gridSize所需的取景距离</summary>
        private const float CameraFieldOfView = 50f;

        /// <summary>
        /// 把场景里Tag为MainCamera的摄像机（没有则新建一个）摆到等距视角、透视投影，取景范围刚好框住整个gridSize——
        /// 透视下没有orthographicSize可以直接指定取景范围，改成按FieldOfView反推：
        /// 距离 = (半个取景范围) / tan(半FOV)，这样不管gridSize多大，整个场景都能刚好落入画面
        /// </summary>
        private static void SetupIsometricCamera(Vector3Int gridSize)
        {
            GameObject camObj = GameObject.FindWithTag("MainCamera");
            if (camObj == null)
            {
                camObj = new GameObject("Main Camera");
                camObj.tag = "MainCamera";
            }
            Camera cam = camObj.GetComponent<Camera>();
            if (cam == null)
                cam = camObj.AddComponent<Camera>();

            Vector3 size = gridSize;
            Vector3 center = size * 0.5f;
            float diagonal = size.magnitude;

            cam.orthographic = false;
            cam.fieldOfView = CameraFieldOfView;
            float halfFovRad = CameraFieldOfView * 0.5f * Mathf.Deg2Rad;
            float distance = diagonal * 0.5f * 1.15f / Mathf.Tan(halfFovRad) + 10f;

            camObj.transform.SetPositionAndRotation(center - IsometricRotation * Vector3.forward * distance, IsometricRotation);
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = distance + diagonal * 2f + 20f;
        }

        private static void CreateObstacleCube(string name, Vector3 worldPos, Material material, Transform parent)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            // 障碍判定改为直接查字典（见 PathFindingSample3D.BuildObstacleMap），不再依赖物理碰撞体
            Object.DestroyImmediate(cube.GetComponent<Collider>());
            cube.transform.SetParent(parent, false);
            cube.transform.position = worldPos;
            cube.transform.localScale = Vector3.one * ObstacleScale;
            cube.GetComponent<Renderer>().sharedMaterial = material;
        }

        #endregion
    }
}
