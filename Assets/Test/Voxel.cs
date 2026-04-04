using UnityEngine;

public class Voxel : MonoBehaviour
{
    public Triangle triangle;
    private Vector3Int position;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        position = Vector3Int.FloorToInt(transform.position);

        if (Intersect(triangle.points, position + 0.5f * Vector3.one, Vector3.one))
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    bool Intersect(Vector3[] points, Vector3 center, Vector3 extent)
    {
        // Get the triangle points as vectors
        Vector3 v0 = points[0] - center;
        Vector3 v1 = points[1] - center;
        Vector3 v2 = points[2] - center;

        // Compute the edge vectors of the triangle  (ABC)
        // That is, get the lines between the points as vectors
        Vector3 f0 = v1 - v0; // B - A
        Vector3 f1 = v2 - v1; // C - B
        Vector3 f2 = v0 - v2; // A - C

        // Compute the face normals of the AABB, because the AABB
        // is at center, and of course axis aligned, we know that 
        // it's normals are the X, Y and Z axis.
        Vector3 u0 = new(1.0f, 0.0f, 0.0f);
        Vector3 u1 = new(0.0f, 1.0f, 0.0f);
        Vector3 u2 = new(0.0f, 0.0f, 1.0f);


        // There are a total of 13 axis to test!

        // We first test against 9 axis, these axis are given by
        // cross product combinations of the edges of the triangle
        // and the edges of the AABB. You need to get an axis testing
        // each of the 3 sides of the AABB against each of the 3 sides
        // of the triangle. The result is 9 axis of seperation
        // https://awwapp.com/b/umzoc8tiv/

        // Compute the 9 axis
        Vector3[] axis = new Vector3[13];
        axis[0] = Vector3.Cross(u0, f0).normalized;
        axis[1] = Vector3.Cross(u0, f1).normalized;
        axis[2] = Vector3.Cross(u0, f2).normalized;
        axis[3] = Vector3.Cross(u1, f0).normalized;
        axis[4] = Vector3.Cross(u1, f1).normalized;
        axis[5] = Vector3.Cross(u1, f2).normalized;
        axis[6] = Vector3.Cross(u2, f0).normalized;
        axis[7] = Vector3.Cross(u2, f1).normalized;
        axis[8] = Vector3.Cross(u2, f2).normalized;

        // Testing axis: axis_u0_f0
        // Project all 3 vertices of the triangle onto the Seperating axis
        axis[9] = u0;
        axis[10] = u1;
        axis[11] = u2;
        // Finally, we have one last axis to test, the face normal of the triangle
        // We can get the normal of the triangle by crossing the first two line segments
        axis[12] = Vector3.Cross(f0, f1).normalized;

        for (int i = 0; i < 13; i++)
        {
            // Testing axis: axis_u0_f0
            // Project all 3 vertices of the triangle onto the Seperating axis
            float p0 = Vector3.Dot(v0, axis[i]);
            float p1 = Vector3.Dot(v1, axis[i]);
            float p2 = Vector3.Dot(v2, axis[i]);

            float r = extent.x * Mathf.Abs(Vector3.Dot(u0, axis[i])) +
                extent.y * Mathf.Abs(Vector3.Dot(u1, axis[i])) +
                extent.z * Mathf.Abs(Vector3.Dot(u2, axis[i]));
            // Now do the actual test, basically see if either of
            // the most extreme of the triangle points intersects r
            // You might need to write Min & Max functions that take 3 arguments
            if (Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r)
            {
                // This means BOTH of the points of the projected triangle
                // are outside the projected half-length of the AABB
                // Therefore the axis is seperating and we can exit
                return false;
            }
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        if (meshRenderer.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.one * 0.5f, Vector3.one);
        }
    }
}
