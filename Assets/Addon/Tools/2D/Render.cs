using UnityEngine;

namespace Tools
{
    public static partial class Tool2D
    {
        public const float Gamma = 2.2f;
        public const float ReciprocalOfGamma = 1f / Gamma;

        /// <summary>
        /// ����GammaУ��
        /// </summary>
        /// <param name="La_final">������ʵ������</param>
        /// <param name="Li_original">��Դԭʼ����</param>
        /// <returns>��Դʵ������</returns>
        public static float GammaCorrection(float La_final, float Li_original)
        {
            float Lg_original = Mathf.Pow(La_final, Gamma);
            float L_original = Lg_original + Li_original;       //ԭʼ���ȣ������Կռ����
            float L_final = Mathf.Pow(L_original, ReciprocalOfGamma);
            return L_final - La_final;
        }

    }
}
