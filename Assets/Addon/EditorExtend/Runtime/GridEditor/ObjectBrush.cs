using System;
using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [RequireComponent(typeof(GridManagerBase))]
    public abstract class ObjectBrush : MonoBehaviour
    {
#if UNITY_EDITOR
        private GridManagerBase manager;
        public GridManagerBase Manager
        {
            get
            {
                if (manager == null)
                    manager = GetComponentInParent<GridManagerBase>();
                return manager;
            }
        }

        public GameObject prefab;
        public bool overrideMode;

        [NonSerialized]
        public Vector3Int cellPosition;

        private List<Transform> mountPoints;
        public List<Transform> MountPoints
        {
            set => mountPoints = value;
            get
            {
                if(mountPoints == null)
                {
                    mountPoints = new()
                    {
                        transform
                    };
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Transform child = transform.GetChild(i);
                        if (!child.GetComponent<GridObject>())
                            mountPoints.Add(child);
                    }
                }
                return mountPoints;
            }
        }
        public Transform MountPoint
        {
            get
            {
                if(mountIndex >=  0 && mountIndex < mountPoints.Count)
                    return mountPoints[mountIndex];
                return transform;
            }
        }
        public int mountIndex;

        public abstract Vector3Int CalculateCellPosition(Vector3 worldPosition, Vector3Int lockedPosition);
#endif
    }
}