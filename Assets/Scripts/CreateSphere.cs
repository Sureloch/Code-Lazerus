using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSphere : MonoBehaviour
{
    //Dictionary for each vertex to position.
    public Dictionary<int, Vector3> vertexLocator = new Dictionary<int, Vector3>();
    public GameObject surface;
    [Header("Sphere Settings")]
    public float radius;       // Radius of the spherical surface.
    public int segments = 32;          // Number of segments (horizontal slices).
    public int rings = 16;             // Number of rings (verticle slices)
    public Mesh sphereMesh;
    void Start()
    {
        sphereMesh = CreateSphericalMeshes(radius);
        PlacePrefabsOnVertices(sphereMesh);
    }






    //Find the edge of the sections and put a different materials
    Mesh CreateSphericalMeshes(float radius)
    {
        // Create a new GameObject to hold the spherical mesh.
        GameObject sphereObject = new GameObject("Spherical Mesh");
        sphereObject.transform.parent = transform;

        // Add necessary components.
        MeshFilter meshFilter = sphereObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = sphereObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = sphereObject.AddComponent<MeshCollider>();

        // Create the spherical mesh.
        Mesh sphereMesh = new Mesh();
        sphereMesh.name = "Spherical Mesh";

        int numVertices = (segments + 1) * (rings + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangles = new int[segments * rings * 6];

        float phiStep = 2 * Mathf.PI / segments;
        float thetaStep = Mathf.PI / rings;
        int index = 0;

        for (int ring = 0; ring <= rings; ring++)
        {
            float theta = Mathf.PI - ring * thetaStep; // Reverse the theta angle.
            for (int segment = 0; segment <= segments; segment++)
            {
                float phi = segment * phiStep;

                float x = Mathf.Sin(theta) * Mathf.Cos(phi);
                float y = Mathf.Cos(theta);
                float z = Mathf.Sin(theta) * Mathf.Sin(phi);

                vertices[index] = new Vector3(x, y, z) * radius;
                uv[index] = new Vector2((float)segment / segments, (float)ring / rings);

                index++;
            }
        }

        index = 0;
        for (int ring = 0; ring < rings; ring++)
        {
            for (int segment = 0; segment < segments; segment++)
            {
                int currentRow = ring * (segments + 1);
                int nextRow = (ring + 1) * (segments + 1);

                triangles[index++] = currentRow + segment;
                triangles[index++] = nextRow + segment;
                triangles[index++] = currentRow + segment + 1;

                triangles[index++] = nextRow + segment;
                triangles[index++] = nextRow + segment + 1;
                triangles[index++] = currentRow + segment + 1;
            }
        }

        sphereMesh.vertices = vertices;
        sphereMesh.uv = uv;
        sphereMesh.triangles = triangles;
        sphereMesh.RecalculateNormals();

        // Assign the created mesh to the Mesh Filter and Mesh Collider.
        meshFilter.mesh = sphereMesh;
        meshCollider.sharedMesh = sphereMesh;
        return sphereMesh;
    }

    void PlacePrefabsOnVertices(Mesh sphereMesh)
    {
        for (int i = 0; i < sphereMesh.vertices.Length; i++)
        {
            Vector3 vertex = sphereMesh.vertices[i];
            Vector3 enemyVertex = vertex * .95f;
            vertexLocator.Add(i, enemyVertex);
            Vector3 prefabPosition = transform.position + vertex;

            Vector3 normal = vertex * radius;
            normal = normal.normalized;
            Quaternion prefabRotation = Quaternion.FromToRotation(Vector3.up, -normal);
            GameObject block = Instantiate(surface, prefabPosition, prefabRotation);
            block.transform.parent = this.transform;

        }
    }
}
