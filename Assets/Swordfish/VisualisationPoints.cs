using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

public class VisualisationPoints : MonoBehaviour
{
    public GameObject dataPointPrefab;
    private Vector3[] vertices;

    void Start()
    {
        Visualisation visualisation = GetComponentInParent<Visualisation>();
        BigMesh visualisationMesh = visualisation.GetComponentInChildren<BigMesh>();
        vertices = visualisationMesh.getBigMeshVertices();
        print(vertices.Length);
        for (int i = 0; i < vertices.Length; i++)
        {
            GameObject point = Instantiate(dataPointPrefab, Vector3.zero, Quaternion.identity);
            point.transform.parent = gameObject.transform;
            point.transform.localPosition = vertices[i];
        }
    }
}