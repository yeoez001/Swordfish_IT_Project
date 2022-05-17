using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

public class DataFiles : MonoBehaviour
{
    public List<CSVDataSource> files;
    public Visualisation visualisation;
    public GameObject dataPointPrefab;

    [SerializeField]
    private RocketTrajectory rocket;

    private void Start()
    {
        // For each data file, create the trajectory within the visualisation object.
        for (int i = 0; i < files.Count; i++)
        {
            // Create the BigMesh object for respective trajectory.
            visualisation.dataSource = files[i];
            visualisation.CreateVisualisation(AbstractVisualisation.VisualisationTypes.SCATTERPLOT);
            BigMesh mesh = visualisation.theVisualizationObject.viewList[0].BigMesh;

            // Create a randomly coloured material to use for the VisualisationLine and VisualisationPoints objects
            var rand = new System.Random();
            Material mat = new Material(Shader.Find("Standard"));
            Color color = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1);
            mat.color = color;

            // Create the VisualisationLine object for this trajectory
            GameObject line = new GameObject();
            line.SetActive(false);
            line.AddComponent<VisualisationLine>();
            line.GetComponent<VisualisationLine>().visualisationMesh = mesh;
            line.GetComponent<VisualisationLine>().lineMat = mat;
            line.transform.SetParent(files[i].transform, false);

            GameObject lineMesh = new GameObject();
            MeshCollider meshCollider = lineMesh.AddComponent<MeshCollider>();
            Mesh newMesh = new Mesh();
            line.GetComponent<LineRenderer>().BakeMesh(newMesh, true);
            meshCollider.sharedMesh = newMesh;
            lineMesh.transform.SetParent(files[i].transform, false);
            //meshCollider.convex = true;
            //meshCollider.isTrigger = true;

            line.SetActive(true);

            // Add the LineRenderer to the list in rocketTrajectory
            rocket.lineList.Add(line.GetComponent<LineRenderer>());

            // Create the VisualisationPoints object for this trajectory
            GameObject point = new GameObject();
            point.SetActive(false);
            point.AddComponent<VisualisationPoints>();
            point.GetComponent<VisualisationPoints>().visualisationMesh = mesh;
            point.GetComponent<VisualisationPoints>().dataPointPrefab = dataPointPrefab;
            point.GetComponent<VisualisationPoints>().pointMat = mat;
            point.transform.SetParent(files[i].transform, false);
            point.SetActive(true);
            point.GetComponent<VisualisationPoints>().createPoints();
        }

        // After final view has loaded, delete it from the visualisation object
        // All the trajectory data is in the visualisationPoints and visualisationLines objects .
        visualisation.destroyView();
    }

    public List<CSVDataSource> GetFiles()
    {
        return files;
    }
}
