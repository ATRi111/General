namespace Tools
{
    public static partial class TextTool
    {
        public static string Bold(this object s)
            => $"<b>{s}</b>";
        public static string ColorText(this object s, string color)
            => $"<color={color.ToLower()}>{s}</color>";
        public static string FontSize(this object s, int size)
            => $"<size={size}>{s}</size>";
    }
}

