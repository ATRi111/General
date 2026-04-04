using UnityEngine;

public class Triangle : MonoBehaviour
{
    public Vector3[] points;

    private void Awake()
    {
        points = new Vector3[3];
    }

    private void Update()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        for (int i = 1; i < transforms.Length; i++)
        {
            points[i - 1] = transforms[i].position;
        }
    }
}
