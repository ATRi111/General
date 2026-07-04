namespace AStar
{
    /// <summary>
    /// 与具体空间表示（2D网格 / 稀疏八叉树等）无关的共享常量与默认方法
    /// </summary>
    public static class PathFindingUtility
    {
        public const float Diagonal2D = 1.41421356f; // √2
        public const float Diagonal3D = 1.73205081f; // √3
        public const float Epsilon = 1e-6f;

        public static float CalculateWeight_Default()
        {
            return 1f;
        }

        public static bool CheckObstacle_Default()
        {
            return true;
        }
    }
}
