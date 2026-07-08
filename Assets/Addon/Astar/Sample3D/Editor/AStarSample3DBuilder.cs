using System.Collections.Generic;
using AStar.ThreeD;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace AStar.Sample
{
    /// <summary>
    /// 一键搭建3D寻路演示：自动生成所需的策略SO资源、DebugNode3D预制体，
    /// 在当前场景里铺一整片"从起点一侧低、往终点一侧高"的地形（仿参考图的分层地块，不留看不到的中空区域），
    /// 起点/终点标记放在各自那一列地形的正上方，再把主摄像机摆到等距视角、正交投影。
    /// 全部通过代码在编辑器里生成，不需要手工摆放，也避免手写场景/预制体文件带来的风险。
    /// </summary>
    public static class AStarSample3DBuilder
    {
        private const string ResFolder = "Assets/Addon/Astar/Sample3D/Res";

        private const int SizeX = 10;
        private const int SizeZ = 10;
        /// <summary>地形最低高度（起点附近）</summary>
        private const int MinHeight = 1;
        /// <summary>地形最高高度（终点附近，还会叠加随机小山丘，实际可能更高）</summary>
        private const int MaxHeight = 6;
        /// <summary>地形之上再留出多少格的飞行/通行空间，供摄像机取景时预留</summary>
        private const int AirBuffer = 3;
        private const float ObstacleScale = 1f;

        /// <summary>
        /// 障碍物调色板：仿照参考图里石头/泥土/草地的配色，按绝对高度(y)取色——同一高度的地块无论在哪一列都用同种颜色
        /// </summary>
        private static readonly Color[] BlockPalette =
        {
            new(0.55f, 0.55f, 0.50f), // 浅灰石
            new(0.45f, 0.38f, 0.28f), // 泥土棕
            new(0.35f, 0.55f, 0.30f), // 草绿
            new(0.62f, 0.52f, 0.35f), // 沙黄
            new(0.42f, 0.44f, 0.48f), // 青灰石
            new(0.58f, 0.32f, 0.28f), // 红陶
        };

        /// <summary>
        /// 等距视角：绕X轴俯视45°，再绕Y轴转45°。
        /// 在这个角度下，画面"越靠上"对应世界坐标 x+z 越大——地形高度沿 x+z 增大而升高，
        /// 于是画面下方（起点附近，x+z小）低矮，画面上方（终点附近，x+z大）高耸，视觉上形成一路升高的坡地
        /// </summary>
        private static readonly Quaternion IsometricRotation = Quaternion.Euler(45f, 45f, 0f);

        [MenuItem("Tools/AStar/搭建3D寻路示例场景")]
        public static void Build()
        {
            EnsureFolder(ResFolder);

            GenerateSampleNode3DSO generateNodeSO = GetOrCreateAsset<GenerateSampleNode3DSO>($"{ResFolder}/生成SampleNode3D.asset");

            GetJumpPoint3DSO jumpPointSO = GetOrCreateAsset<GetJumpPoint3DSO>($"{ResFolder}/六向跳点.asset");
            if (jumpPointSO.depthOnDirection <= 0)
            {
                jumpPointSO.depthOnDirection = 50;
                EditorUtility.SetDirty(jumpPointSO);
            }
            // 顺带生成一份朴素六向策略资源，方便在Inspector里对比切换（不作为默认值使用）
            GetOrCreateAsset<Get6SO>($"{ResFolder}/六向移动.asset");

            GameObject prefab = GetOrCreateDebugPrefab($"{ResFolder}/DebugNode3D.prefab");

            System.Random rng = new(12345);
            int[,] heights = BuildHeightMap(rng);

            GameObject root = new("AStarSample3D_Demo");

            // 必须先创建 From/To 子物体，再挂 PathFindingSample3D 组件——
            // AddComponent 会立刻触发 Awake（即使在编辑模式下），Awake 里会 transform.Find("From")/("To")；
            // y 取该列地形高度之上半格，确保起点/终点落在地面以上，而不是被埋进地形里
            CreateMarker("From", new Vector3(0.5f, heights[0, 0] + 0.5f, 0.5f), Color.green, root.transform);
            CreateMarker("To", new Vector3(SizeX - 0.5f, heights[SizeX - 1, SizeZ - 1] + 0.5f, SizeZ - 0.5f), Color.cyan, root.transform);

            PathFindingSample3D sample = root.AddComponent<PathFindingSample3D>();

            int verticalExtent = MaxHeight + 2 + AirBuffer;

            SerializedObject so = new(sample);
            so.FindProperty("prefab").objectReferenceValue = prefab;
            so.FindProperty("gridSize").vector3IntValue = new Vector3Int(SizeX, verticalExtent, SizeZ);
            // 显式给一个覆盖全图的富余值：Node.Recall() 回溯输出路径时会用它过滤GCost超出预算的节点，
            // 留空/0会导致只有GCost=0的起点本身能进入output（表现为"寻路完成后只有起点被染成output颜色"）
            so.FindProperty("moveAbility").intValue = 999;
            so.FindProperty("process.mountPoint").objectReferenceValue = root.transform;
            so.FindProperty("process.settings.generateNodeSO").objectReferenceValue = generateNodeSO;
            so.FindProperty("process.settings.getAdjoinedNodesSO").objectReferenceValue = jumpPointSO;
            so.ApplyModifiedPropertiesWithoutUndo();

            BuildTerrain(root.transform, heights);
            SetupIsometricCamera();

            Selection.activeGameObject = root;
        }

        /// <summary>
        /// 生成整片地形的高度图：沿 x+z 方向从 <see cref="MinHeight"/> 线性升高到 <see cref="MaxHeight"/>，
        /// 再叠加少量随机抖动和偶尔出现的小山丘，让坡地不至于完全平滑
        /// </summary>
        private static int[,] BuildHeightMap(System.Random rng)
        {
            int[,] heights = new int[SizeX, SizeZ];
            float maxT = (SizeX - 1) + (SizeZ - 1);

            for (int x = 0; x < SizeX; x++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    float t = (x + z) / maxT;
                    int baseHeight = Mathf.RoundToInt(Mathf.Lerp(MinHeight, MaxHeight, t));
                    int jitter = rng.Next(-1, 2);
                    int hill = rng.Next(0, 100) < 20 ? rng.Next(1, 3) : 0;
                    heights[x, z] = Mathf.Clamp(baseHeight + jitter + hill, MinHeight, MaxHeight + 2);
                }
            }
            return heights;
        }

        /// <summary>按绝对高度(y)取调色板颜色，保证同一高度的地块无论在哪一列都是同种颜色</summary>
        private static Color GetLayerColor(int y) => BlockPalette[y % BlockPalette.Length];

        /// <summary>
        /// 按高度图把整片地形从地面(y=0)填到各自的高度，每一列内部连续没有空洞，
        /// 也不会出现悬空的孤立方块——因此整块地形没有任何从外部看不到的中空区域
        /// </summary>
        private static void BuildTerrain(Transform mount, int[,] heights)
        {
            Dictionary<Color, Material> materialCache = new();
            Material GetMaterial(Color color)
            {
                if (!materialCache.TryGetValue(color, out Material material))
                {
                    material = CreateColorMaterial(color);
                    materialCache[color] = material;
                }
                return material;
            }

            for (int x = 0; x < SizeX; x++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    int height = heights[x, z];

                    for (int y = 0; y < height; y++)
                    {
                        // 不再对每列的顶层单独提亮——那样会导致"同一高度"在不同列上出现两种颜色
                        // （某列的顶层被提亮，另一列在这个高度只是側面/非顶层，仍是原色），
                        // 统一只按绝对高度取色，保证同一高度的地块颜色完全一致
                        CreateObstacleCube("Block", new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), GetMaterial(GetLayerColor(y)), mount);
                    }
                }
            }
        }

        private static T GetOrCreateAsset<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
                return asset;

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
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

        /// <summary>
        /// 把场景里Tag为MainCamera的摄像机（没有则新建一个）摆到等距视角，正交投影，取景范围刚好框住整个地形
        /// </summary>
        private static void SetupIsometricCamera()
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

            int verticalExtent = MaxHeight + 2 + AirBuffer;
            Vector3 gridSize = new(SizeX, verticalExtent, SizeZ);
            Vector3 center = gridSize * 0.5f;
            float diagonal = gridSize.magnitude;

            camObj.transform.SetPositionAndRotation(center - IsometricRotation * Vector3.forward * (diagonal + 10f), IsometricRotation);
            cam.orthographic = true;
            cam.orthographicSize = diagonal * 0.5f * 1.15f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = diagonal * 2f + 20f;
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

        /// <summary>调试节点用的球体缩放比例，比整格的障碍物立方体（<see cref="ObstacleScale"/>=1）略小，便于区分</summary>
        private const float DebugNodeScale = 0.7f;

        private static GameObject GetOrCreateDebugPrefab(string path)
        {
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null)
                return existing;

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "DebugNode3D";
            Object.DestroyImmediate(sphere.GetComponent<Collider>());
            sphere.transform.localScale = Vector3.one * DebugNodeScale;

            GameObject textObj = new("Text");
            textObj.transform.SetParent(sphere.transform, false);
            // TextMeshPro（非UGUI）本身依赖RectTransform，先AddComponent让Transform升级为RectTransform，
            // 再设置缩放，避免在升级过程中丢失提前设置的数值
            TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
            // 抵消父物体(球体)的缩放，让文字的实际大小与父物体缩放无关
            textObj.transform.localScale = Vector3.one / DebugNodeScale * 0.2f;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 6;
            tmp.color = Color.black;
            tmp.rectTransform.sizeDelta = new Vector2(4, 4);

            SampleDebugNode3D debugNode = sphere.AddComponent<SampleDebugNode3D>();
            // 仿照2D版DebugNode.prefab里手动设置好的颜色方案，用不同颜色区分节点状态
            SerializedObject nodeSO = new(debugNode);
            nodeSO.FindProperty("color_open").colorValue = new Color(0.022067308f, 1f, 0f);
            nodeSO.FindProperty("color_close").colorValue = new Color(1f, 0f, 0f);
            nodeSO.FindProperty("color_obstacle").colorValue = new Color(0f, 0f, 0f);
            nodeSO.FindProperty("color_output").colorValue = new Color(0f, 0.89399576f, 1f);
            nodeSO.FindProperty("color_available").colorValue = new Color(1f, 0f, 0.7567086f);
            nodeSO.FindProperty("color_blank").colorValue = new Color(1f, 1f, 1f);
            nodeSO.ApplyModifiedPropertiesWithoutUndo();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(sphere, path);
            Object.DestroyImmediate(sphere);
            return prefab;
        }
    }
}
