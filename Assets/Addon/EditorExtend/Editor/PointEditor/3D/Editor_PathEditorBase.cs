using UnityEditor;
using UnityEngine;

namespace EditorExtend.PointEditor
{
    [CustomEditor(typeof(PathEditorBase))]
    public class Editor_PathEditorBase : GameObjectsManagerEditor
    {
        protected override GameObject CreateObject(bool first)
        {
            Vector3 newPosition = manager.transform.position;

            if (GameObjects.Length > 1)
            {
                if (first)
                {
                    Vector3 pos1 = GameObjects[0].transform.position;
                    Vector3 pos2 = GameObjects[1].transform.position;
                    newPosition = pos1 + settings.DefaultDistance * (pos1 - pos2).normalized;
                }
                else
                {
                    Vector3 pos1 = GameObjects[^1].transform.position;
                    Vector3 pos2 = GameObjects[^2].transform.position;
                    newPosition = pos1 + settings.DefaultDistance * (pos1 - pos2).normalized;
                }
            }
            else if (GameObjects.Length == 1)
            {
                if (first)
                    newPosition = GameObjects[0].transform.position + settings.DefaultDistance * Vector3.left;
                else
                    newPosition = GameObjects[0].transform.position + settings.DefaultDistance * Vector3.right;
            }
            GameObject obj = base.CreateObject(first);
            obj.transform.position = newPosition;
            return obj;
        }

        protected override void Paint()
        {
            base.Paint();
            for (int i = 0; i < GameObjects.Length - 1; i++)
            {
                PaintObject(GameObjects[i]);
                PaintLine(GameObjects[i], GameObjects[i + 1]);
            }
        }

        protected virtual void PaintObject(GameObject obj)
        {

        }

        protected virtual void PaintLine(GameObject from, GameObject to)
        {

        }
    }
}