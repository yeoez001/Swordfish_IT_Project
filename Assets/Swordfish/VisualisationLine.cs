using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

[RequireComponent(typeof(LineRenderer))]
public class VisualisationLine : MonoBehaviour
{
    public Vector3[] vertices;

    void Start()
    {
        Visualisation visualisation = GetComponentInParent<Visualisation>();
        BigMesh visualisationMesh = visualisation.GetComponentInChildren<BigMesh>();
        vertices = visualisationMesh.getBigMeshVertices();

        LineRenderer line = GetComponent<LineRenderer>();
        line.startWidth=(0.006f);
        line.useWorldSpace = false;
        line.positionCount = vertices.Length;
        line.SetPositions(vertices);
    }
}
