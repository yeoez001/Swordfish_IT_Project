using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

[RequireComponent(typeof(LineRenderer))]
public class VisualisationLine : MonoBehaviour
{
    public Vector3[] vertices;
    public BigMesh visualisationMesh;
    public LineRenderer line;
    public Material lineMat;

    void Start()
    {
        vertices = visualisationMesh.getBigMeshVertices();

        // Creat the lineRenderer object from the BigMesh data
        line = GetComponent<LineRenderer>();
        line.startWidth=(0.006f);
        line.useWorldSpace = false;
        line.positionCount = vertices.Length;
        line.SetPositions(vertices);
        line.material = lineMat;
    }
}
