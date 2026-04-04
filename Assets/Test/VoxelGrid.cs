using UnityEngine;

public class VoxelGrid : MonoBehaviour
{
    [Range(0, 1)]
    public float w;
    public int size;
    public GameObject prefab;
    public Triangle triangle;

    private void Awake()
    {
        triangle = GetComponentInChildren<Triangle>();
    }

    private void Start()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    GameObject cube = Instantiate(prefab);
                    cube.transform.position = new Vector3(x, y, z);
                    cube.transform.parent = transform;
                    Voxel voxel = cube.GetComponent<Voxel>();
                    voxel.triangle = triangle;
                }
            }
        }
    }

}
