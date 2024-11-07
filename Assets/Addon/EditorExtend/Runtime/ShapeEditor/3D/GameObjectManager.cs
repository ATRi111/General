using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.ShapeEditor
{
    [SelectionBase]
    public class GameObjectManager : MonoBehaviour
    {
        public GameObject prefab;
        public GameObject[] gameObjects;

        public virtual void RefreshGameObjects()
        {
            gameObjects = new GameObject[transform.childCount];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i] = transform.GetChild(i).gameObject;
            }
        }

        public void GetLocalPoints(List<Vector3> ret)
        {
            ret.Clear();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                ret.Add(gameObjects[i].transform.localPosition);
            }
        }

        public void GetWorldPoints(List<Vector3> ret)
        {
            ret.Clear();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                ret.Add(gameObjects[i].transform.position);
            }
        }

        private void OnDrawGizmos()
        {
            RefreshGameObjects();
        }
    }
}