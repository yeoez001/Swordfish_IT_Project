using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

// Creates the points of a visualisation as separate GameObjects based on the IATK combined mesh
public class VisualisationPoints : MonoBehaviour
{
    // Prefab of object to instantiate the data point as
    private GameObject dataPointPrefab;

    private Vector3[] vertices;
    private List<GameObject> dataPoints;
    private BigMesh visualisationMesh;
    private Material pointMat;

    // Creates a GameObject dataPointPrefab consisted of individual objects representing
    // each data point from a CSV file.
    public void createPoints()
    {
        vertices = visualisationMesh.getBigMeshVertices();
        CSVDataSource dataSource = (CSVDataSource)GetComponentInParent<Visualisation>().dataSource;
        dataPoints = new List<GameObject>();

        // Instantiate separate GameObjects for each data point
        for (int i = 0; i < vertices.Length; i++)
        {
            GameObject pointGO = Instantiate(dataPointPrefab, Vector3.zero, Quaternion.identity);
            pointGO.transform.parent = gameObject.transform;
            pointGO.transform.localPosition = vertices[i];
            dataPoints.Add(pointGO);
            DataPoint point = pointGO.GetComponent<DataPoint>();

            // Set the data values for that point (i.e. the row of values from CSV)
            point.SetData(dataSource, dataSource.GetRow(dataSource.dataArray, i));
            point.GetComponent<MeshRenderer>().material = pointMat;
        }
    }

    public List<GameObject> DataPoints()
    {
        return dataPoints;
    }

    public void setDataPointPrefab(GameObject prefab)
    {
        dataPointPrefab = prefab;
    }

    public void setVisualisationMesh(BigMesh mesh)
    {
        visualisationMesh = mesh;
    }

    public void setPointMaterial(Material mat)
    {
        pointMat = mat;
    }
}