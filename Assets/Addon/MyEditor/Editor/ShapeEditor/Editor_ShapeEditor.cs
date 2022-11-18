using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MyEditor.ShapeEditor
{
    public abstract class Editor_ShapeEditor : AutoEditor
    {
        protected int selectedIndex;  //��ǰ��ק�ĵ������
        protected bool b_edit;        //�Ƿ��ڱ༭״̬

        protected override void OnEnable()
        {
            base.OnEnable();
            b_edit = false;
            selectedIndex = -1;
            focusMode = EFocusMode.Default;
        }

        protected override void MyOnInspectorGUI()
        {
            string s = b_edit ? "complete" : "edit";
            if (GUILayout.Button(s))
            {
                b_edit = !b_edit;
                focusMode = b_edit ? EFocusMode.Lock : EFocusMode.Default;
                UnityEditor.Tools.current = b_edit ? Tool.None : Tool.Move;
                SceneView.RepaintAll();
            }

            SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
            if(spriteRenderer!= null && spriteRenderer.sprite != null)
            {
                if(GUILayout.Button("match sprite"))
                {
                    MatchSprite(spriteRenderer.sprite);
                }
            }
        }

        protected override void MyOnSceneGUI()
        {
            //serializedObject��Ӧ�����������ȷʵ���ã����Ҳ�֪��Ӧ����ʲô����
            serializedObject.Update();
            if (currentEvent.type == EventType.Repaint)
                Paint();
            if (b_edit)
            {
                switch (currentEvent.type)
                {
                    case EventType.MouseDown:
                        if (currentEvent.button == 0)
                            OnLeftMouseDown();
                        else if (currentEvent.button == 1)
                            OnRightMouseDown();
                        break;
                    case EventType.MouseDrag:
                        if (currentEvent.button == 0)
                            OnLeftMouseDrag();
                        break;
                    case EventType.MouseUp:
                        if (currentEvent.button == 0)
                            OnLeftMouseUp();
                        break;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void Paint() { }

        protected virtual void OnLeftMouseDown() { }
        protected virtual void OnRightMouseDown() { }
        protected virtual void OnLeftMouseUp()
        {
            selectedIndex = -1;
        }
        protected virtual void OnLeftMouseDrag() { }

        protected abstract void MatchSprite(Sprite sprite);
    }
}