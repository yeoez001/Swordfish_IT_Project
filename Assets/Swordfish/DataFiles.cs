using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

public class DataFiles : MonoBehaviour
{
    public List<CSVDataSource> files;
    public Visualisation visualisation;
    private bool load = false;

    public GameObject dataPointPrefab;

    private List<GameObject> allPoints;
    private List<GameObject> allLines;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (load == false)
        {

            for (int i = 0; i < files.Count; i++)
            {
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


//                allPoints.Add(point);


                // Create the VisualisationLine object for this trajectory
                GameObject line = new GameObject();
                line.SetActive(false);
                line.AddComponent<VisualisationLine>();
                line.GetComponent<VisualisationLine>().visualisationMesh = mesh;
                line.transform.SetParent(files[i].transform, false);
                line.SetActive(true);

     //           allLines.Add(line);



                //var person = new Person { FirstName = "Scott", LastName = "Guthrie", Age = 32 };



                //GameObject line = new GameObject();
                //line.AddComponent<VisualisationLine>();
            }

            // After final view has loaded, delete it from the visualisation object.
            visualisation.destroyView();

            load = true;
        }
    }
}
