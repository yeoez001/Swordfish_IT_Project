//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using IATK;

//public class VisualisationPoints : MonoBehaviour
//{
//    public GameObject dataPointPrefab;
//    private Vector3[] vertices;
//    private List<GameObject> dataPoints;
//    void Start()
//    {
//        Visualisation visualisation = GetComponentInParent<Visualisation>();
//        BigMesh visualisationMesh = visualisation.GetComponentInChildren<BigMesh>();
//        vertices = visualisationMesh.getBigMeshVertices();
//        CSVDataSource dataSource = (CSVDataSource)GetComponentInParent<Visualisation>().dataSource;
//        dataPoints = new List<GameObject>();

//        // Instantiate separate GameObjects for each data point
//        for (int i = 0; i < vertices.Length; i++)
//        {
//            GameObject pointGO = Instantiate(dataPointPrefab, Vector3.zero, Quaternion.identity);
//            pointGO.transform.parent = gameObject.transform;
//            pointGO.transform.localPosition = vertices[i];            
//            dataPoints.Add(pointGO);
//            DataPoint point = pointGO.GetComponent<DataPoint>();

//            // Set the data values for that point (i.e. the row of values from CSV)
//            point.SetData(dataSource, dataSource.GetRow(dataSource.dataArray, i));
//        }
//    }
//}