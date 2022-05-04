using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

public class DataFiles : MonoBehaviour
{
    public List<CSVDataSource> files;
    public Visualisation visualisation;
    public GameObject dataPointPrefab;
    private bool load = false;

    private void Start()
    {
        // For each data file, create the trajectory within the visualisation object.
        for (int i = 0; i < files.Count; i++)
        {
            // Create the BigMesh object for respective trajectory.
            visualisation.dataSource = files[i];
            visualisation.CreateVisualisation(AbstractVisualisation.VisualisationTypes.SCATTERPLOT);
            BigMesh mesh = visualisation.theVisualizationObject.viewList[0].BigMesh;


            // Create the VisualisationPoints object for this trajectory
            GameObject point = new GameObject();
            point.SetActive(false);
            point.AddComponent<VisualisationPoints>();
            point.GetComponent<VisualisationPoints>().visualisationMesh = mesh;
            point.GetComponent<VisualisationPoints>().dataPointPrefab = dataPointPrefab;
            point.transform.SetParent(files[i].transform, false);
            point.SetActive(true);
            point.GetComponent<VisualisationPoints>().createPoints();

            // Create the VisualisationLine object for this trajectory
            GameObject line = new GameObject();
            line.SetActive(false);
            line.AddComponent<VisualisationLine>();
            line.GetComponent<VisualisationLine>().visualisationMesh = mesh;
            line.transform.SetParent(files[i].transform, false);
            line.SetActive(true);

            // TODO
            // THIS IS A BAD WORKAROUND.
            // Create a new script and link to each "DataSource". When the above point and line
            // have been created, add them to that data source and then access it properly
            // from RocketTrajectory.
            line.name = "LineRenderer" + i;
        }

        // After final view has loaded, delete it from the visualisation object
        // All the trajectory data is in the visualisationPoints and visualisationLines objects .
        visualisation.destroyView();
    }

    //void Update()
    //{
    //    if (load == false)
    //    {
    //        // For each data file, create the trajectory within the visualisation object.
    //        for (int i = 0; i < files.Count; i++)
    //        {
    //            // Create the BigMesh object for respective trajectory.
    //            visualisation.dataSource = files[i];
    //            visualisation.CreateVisualisation(AbstractVisualisation.VisualisationTypes.SCATTERPLOT);
    //            BigMesh mesh = visualisation.theVisualizationObject.viewList[0].BigMesh;


    //            // Create the VisualisationPoints object for this trajectory
    //            GameObject point = new GameObject();
    //            point.SetActive(false);
    //            point.AddComponent<VisualisationPoints>();
    //            point.GetComponent<VisualisationPoints>().visualisationMesh = mesh;
    //            point.GetComponent<VisualisationPoints>().dataPointPrefab = dataPointPrefab;
    //            point.transform.SetParent(files[i].transform, false);
    //            point.SetActive(true);
    //            point.GetComponent<VisualisationPoints>().createPoints();

    //            // Create the VisualisationLine object for this trajectory
    //            GameObject line = new GameObject();
    //            line.SetActive(false);
    //            line.AddComponent<VisualisationLine>();
    //            line.GetComponent<VisualisationLine>().visualisationMesh = mesh;
    //            line.transform.SetParent(files[i].transform, false);
    //            line.SetActive(true);
    //        }

    //        // After final view has loaded, delete it from the visualisation object
    //        // All the trajectory data is in the visualisationPoints and visualisationLines objects .
    //        visualisation.destroyView();

    //        load = true;
    //    }
    //}
}
