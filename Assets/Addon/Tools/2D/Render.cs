using UnityEngine;

namespace Tools
{
    public static partial class Tool2D
    {
        public const float Gamma = 2.2f;
        public const float ReciprocalOfGamma = 1f / Gamma;

        /// <summary>
        /// 计算Gamma校正
        /// </summary>
        /// <param name="La_final">环境光实际亮度</param>
        /// <param name="Li_original">光源原始亮度</param>
        /// <returns>光源实际亮度</returns>
        public static float GammaCorrection(float La_final, float Li_original)
        {
            float Lg_original = Mathf.Pow(La_final, Gamma);
            float L_original = Lg_original + Li_original;       //原始亮度，在线性空间叠加
            float L_final = Mathf.Pow(L_original, ReciprocalOfGamma);
            return L_final - La_final;
        }

    }
}
