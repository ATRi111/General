using UnityEngine;

namespace EditorExtend.ShapeEditor
{
    public abstract class Shape2DEditor : ShapeEditor
    {
        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            SpriteRenderer spriteRenderer = (target as MonoBehaviour).GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                if (GUILayout.Button("适配图片形状"))
                {
                    MatchSprite(spriteRenderer.sprite);
                }
            }
        }

        protected override void MyOnSceneGUI()
        {
            Select();
            base.MyOnSceneGUI();
        }

        protected abstract void MatchSprite(Sprite sprite);

        protected override void OnMouseDrag(int button)
        {
            base.OnMouseDrag(button);
            switch(button)
            {
                case 0:
                    Drag();
                    break;
            }
        }

        protected override void OnMouseUp(int button)
        {
            base.OnMouseUp(button);
            selectedIndex = -1;
            currentEvent.Use();
        }

        protected virtual void Drag() { }
    }
}