using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GarmentVisualStabilizer : MonoBehaviour
{
    [Header("Visual Fit Stabilization")]
    public float normalOffset = 0.003f;
    public bool applyOnStart = true;

    private Mesh originalMesh;
    private Mesh workingMesh;

    void Start()
    {
        if (applyOnStart)
        {
            ApplyNormalOffset();
        }
    }

    [ContextMenu("Apply Normal Offset")]
    public void ApplyNormalOffset()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogWarning("No mesh found for GarmentVisualStabilizer.");
            return;
        }

        originalMesh = meshFilter.sharedMesh;
        workingMesh = Instantiate(originalMesh);
        workingMesh.name = originalMesh.name + "_visual_stabilized";

        Vector3[] vertices = workingMesh.vertices;
        Vector3[] normals = workingMesh.normals;

        if (normals == null || normals.Length != vertices.Length)
        {
            workingMesh.RecalculateNormals();
            normals = workingMesh.normals;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] += normals[i] * normalOffset;
        }

        workingMesh.vertices = vertices;
        workingMesh.RecalculateBounds();

        meshFilter.mesh = workingMesh;
    }

    [ContextMenu("Reset Mesh")]
    public void ResetMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null && originalMesh != null)
        {
            meshFilter.mesh = originalMesh;
        }
    }
}