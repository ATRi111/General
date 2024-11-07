using UnityEngine;

namespace EditorExtend
{
    public class InteractiveEditor : AutoEditor
    {
        protected bool editorModeOnly;

        protected override void MyOnInspectorGUI()
        {
            if (editorModeOnly && Application.isPlaying)
                return;

            HandleKeyInput();
        }

        //必要时调用currentEvent.Use()
        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            if (editorModeOnly && Application.isPlaying)
                return;

            if (currentEvent.type == EventType.Repaint)
                Paint();
            HandleKeyInput();
            HandleMouseInput();
        }

        protected virtual void HandleKeyInput()
        {
            if (currentEvent == null)
                return;
            switch (currentEvent.type)
            {
                case EventType.KeyDown:
                    OnKeyDown(currentEvent.keyCode);
                    break;
                case EventType.KeyUp:
                    OnKeyUp(currentEvent.keyCode);
                    break;
            }
        }

        protected virtual void HandleMouseInput()
        {
            if (currentEvent == null)
                return;
            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    OnMouseDown(currentEvent.button);
                    break;
                case EventType.MouseDrag:
                    OnMouseDrag(currentEvent.button);
                    break;
                case EventType.MouseUp:
                    OnMouseUp(currentEvent.button);
                    break;
            }
        }

        protected virtual void Paint() { }
        protected virtual void OnMouseDown(int button) { }
        protected virtual void OnMouseDrag(int button) { }
        protected virtual void OnMouseUp(int button) { }
        protected virtual void OnKeyUp(KeyCode keyCode) { }
        protected virtual void OnKeyDown(KeyCode keyCode) { }
    }
}