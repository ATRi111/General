using UnityEngine;

namespace MyEditor.PointEditor
{
    public class PointEditorSettings : ScriptableObject
    {
        public float DefaultLineThickness;
        public float DefaultDotSize;

        public Color LineColor;
        public Color PointColor;
        public Color SelectedPointColor;
        public Color NewPointColor;

        /// <summary>
        /// ��꿿������Ĵ˷�Χ��ʱ����ѡ�е㣨��Ļ����ϵ�£�
        /// </summary>
        public float HitPointDistance;
        /// <summary>
        /// ��꿿�����ߵĴ˷�Χ��ʱ����ѡ���ߣ���Ļ����ϵ�£�
        /// </summary>
        public float HitLineDistance;
        /// <summary>
        /// ��꿿������Ϸ����Ĵ˷�Χ��ʱ���ᱣ��ѡ����Ϸ�����״̬����Ļ����ϵ�£�
        /// </summary>
        public float ContainHitObjectDistance;
        /// <summary>
        /// ��꿿������Ϸ����Ĵ˷�Χ��ʱ����ѡ����Ϸ���壨��Ļ����ϵ�£�
        /// </summary>
        public float HitObjectDistance;

        public float DefaultDistance;
    }
}