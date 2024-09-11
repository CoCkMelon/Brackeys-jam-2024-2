using UnityEngine;

public class MeshVolumeCalculator : MonoBehaviour
{
    void Start()
    {
        // Get the mesh filter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter component not found!");
            return;
        }

        // Get the mesh
        Mesh mesh = meshFilter.mesh;

        // Calculate the volume of the mesh
        float volume = CalculateMeshVolume(mesh);
        Debug.Log("Mesh Volume: " + volume);
    }

    float CalculateMeshVolume(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        float volume = 0;

        // Iterate through each triangle
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);

            // Calculate the volume of the tetrahedron formed by the triangle and the origin
            volume += CalculateTetrahedronVolume(v0, v1, v2);
        }

        return Mathf.Abs(volume);
    }

    float CalculateTetrahedronVolume(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        // Volume of a tetrahedron formed by vertices v0, v1, v2, and the origin
        return Vector3.Dot(v0, Vector3.Cross(v1, v2)) / 6.0f;
    }
}
