using System.IO;
using UnityEngine;

public class TextureTool 
{
    public static readonly Vector2 s_mid = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// 读取图片，转换为Texture2D
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture2D LoadImage(string path)
    {
        FileInfo fileInfo = FileTool.GetFileInfo(path);
        using FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        Texture2D ret = new Texture2D(0, 0);
        ret.LoadImage(bytes);
        return ret;
    }

    /// <summary>
    /// 加载一张图片，以原像素数显示
    /// </summary>
    public static Sprite LoadSptite(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), s_mid);
        return sprite;
    }
    /// <summary>
    /// 计算重心坐标
    /// </summary>
    /// <returns>三个分量先后表示A、B、C的权重</returns>
    public static Vector3 BarycentricCoordinates(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        float alpha, beta, gamma;
        float x, xa, xb, xc, y, ya, yb, yc;
        x = P.x; xa = A.x; xb = B.x; xc = C.x;
        y = P.y; ya = A.y; yb = B.y; yc = C.y;
        gamma = (ya * x - yb * x + xb * y - xa * y + xa * yb - xb * ya) / (ya * xc - yb * xc + xb * yc - xa * yc + xa * yb - xb * ya);
        beta = (ya * x - yc * x + xc * y - xa * y + xa * yc - xc * ya) / (ya * xb - yc * xb + xc * yb - xa * yb + xa * yc - xc * ya);
        alpha = 1 - gamma - beta;
        return new Vector3(alpha, beta, gamma);
    }
    /// <summary>
    /// 计算双线性插值
    /// </summary>
    /// <returns>四个分量先后表示左下、左上、右上、右下的权重</returns>
    public static Vector4 Bilinear(Vector2Int LeftBottom, Vector2Int RightUp, Vector2 P)
    {
        if (LeftBottom.x > RightUp.x || LeftBottom.y > RightUp.y)
            throw new System.ArgumentException();
        float alpha, beta, gamma, delta;
        if (LeftBottom == RightUp)
        {
            alpha = beta = gamma = delta = 0.25f;
        }
        else if (LeftBottom.x == RightUp.x)
        {
            alpha = delta = 0.5f * (P.y - LeftBottom.y) / (RightUp.y - LeftBottom.y);
            beta = gamma = 0.5f - alpha;
        }
        else if (LeftBottom.y == RightUp.y)
        {
            alpha = beta = 0.5f * (P.x - LeftBottom.x) / (RightUp.x - LeftBottom.x);
            gamma = delta = 0.5f - alpha;
        }
        else
        {
            float s = (RightUp.x - LeftBottom.x) * (RightUp.y - LeftBottom.y);
            alpha = (P.x - LeftBottom.x) * (P.y - LeftBottom.y) / s;
            beta = (P.x - LeftBottom.x) * (RightUp.y - P.y) / s;
            gamma = (RightUp.x - P.x) * (RightUp.y - P.y) / s;
            delta = 1 - alpha - beta - gamma;
        }
        return new Vector4(alpha, beta, gamma, delta);
    }
}
