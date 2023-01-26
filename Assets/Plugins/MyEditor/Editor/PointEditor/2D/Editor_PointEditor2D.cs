using UnityEngine;

namespace MyEditor.PointEditor
{
    public abstract class Editor_PointEditor2D : Editor_PointEditor
    {
        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            SpriteRenderer spriteRenderer = (target as MonoBehaviour).GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                if (GUILayout.Button("����ͼƬ��״"))
                {
                    MatchSprite(spriteRenderer.sprite);
                }
            }
        }

        protected abstract void MatchSprite(Sprite sprite);

        protected override void OnLeftMouseUp()
        {
            selectedIndex = -1;
        }
    }
}