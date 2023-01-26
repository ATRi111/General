using UnityEngine;

namespace Tools
{
    public static partial class GeneralTool
    {
        public static Color SetAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);

        public static string ToHRGB(this Color32 color)
        {
            return "#"
                + color.r.ToString("x").PadLeft(2, '0')
                + color.g.ToString("x").PadLeft(2, '0')
                + color.b.ToString("x").PadLeft(2, '0');
        }

        /// <summary>
        /// 将颜色转换为形如#FFFFFF的字符串
        /// </summary>
        public static string ToHRGB(this Color color)
        {
            return ToHRGB((Color32)color);
        }
    }

}
