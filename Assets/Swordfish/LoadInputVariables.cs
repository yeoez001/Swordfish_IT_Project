using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System.IO;

public class LoadInputVariables : MonoBehaviour
{
    public Visualisation visualisation;  // IATK visualisation object
    private float[] dimensionMin;
    private float[] dimensionMax;
    private int maxIndexX = 0;
    private int maxIndexY = 0;
    private string path = "/Resources/MainData/InputData";
    private List<CSVDataSource> files;

    // Start is called before the first frame update
    void Start()
    {
        // First find files and create csvDataSource objects
        files = new List<CSVDataSource>();
        createCSVDataSource();

        //dimensionMin = new float[files[0].DimensionCount];
        //dimensionMax = new float[files[0].DimensionCount];

        //// TODO
        //// INEFFICIENT?? TEMP FIX 
        //// Determine global min/max for scaling
        //// Loop each file
        //for (int i = 0; i < files.Count; i++)
        //{
        //    // Loop each variable in files
        //    for (int j = 0; j < files[i].DimensionCount; j++)
        //    {
        //        // Get min and max values for variable and compare against current
        //        // global min/max
        //        if (i == 0)
        //        {
        //            dimensionMin[j] = files[i].getDimensions()[j].MetaData.minValue;
        //        }
        //        else if (files[i].getDimensions()[j].MetaData.minValue < dimensionMin[j])
        //        {
        //            dimensionMin[j] = files[i].getDimensions()[j].MetaData.minValue;
        //        }

        //        if (i == 0)
        //        {
        //            dimensionMax[j] = files[i].getDimensions()[j].MetaData.maxValue;
        //        }
        //        else if (files[i].getDimensions()[j].MetaData.maxValue > dimensionMax[j])
        //        {
        //            dimensionMax[j] = files[i].getDimensions()[j].MetaData.maxValue;

        //            // VERY inelegant solution
        //            // Gets variable at index 0 (i.e. 'mass'). Fixed index for testing purposes
        //            if (j == 0)
        //            {
        //                maxIndexX = i;
        //            }
        //            else if (j == 2)
        //            {
        //                maxIndexY = i;
        //            }
        //        }
        //    }
        //}

        // For each data file, create the trajectory within the visualisation object.
        for (int i = 0; i < files.Count; i++)
        {
            // Rescale the values based upon global min/max
            files[i].repopulate(dimensionMin, dimensionMax);

            // Create the trajectory data objects
            createTrajectory(i);
        }
    }

    // For each csv file in the directory, create a csvDataSourceObject
    private void createCSVDataSource()
    {
        string[] filePaths = Directory.GetFiles(Application.dataPath + path, "*.csv");
        for (int i = 0; i < filePaths.Length; i++)
        {
            // Create new game object with CSVDataSource component
            GameObject dataSourceObj = new GameObject("DataSource" + (i + 1));
            dataSourceObj.transform.SetParent(this.transform, false);

            dataSourceObj.AddComponent<CSVDataSource>();

            // Set CSVDataSource data to file data.
            TextAsset data = textfromFile(filePaths[i]);
            dataSourceObj.GetComponent<CSVDataSource>().data = data;
            dataSourceObj.GetComponent<CSVDataSource>().loadHeader();
            dataSourceObj.GetComponent<CSVDataSource>().load();
            files.Add(dataSourceObj.GetComponent<CSVDataSource>());
            Debug.Log(dataSourceObj.GetComponent<CSVDataSource>().data);
        }
    }

    // Creates trajectory data objects (BigMesh, LineRenderer, MeshCollider, VisualisationPoints)
    private void createTrajectory(int fileIndex)
    {
        // Create the BigMesh object for respective trajectory.
        visualisation.dataSource = files[fileIndex];
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
        line.GetComponent<VisualisationLine>().setVisualisationMesh(mesh);
        line.GetComponent<VisualisationLine>().setLineMaterial(mat);
        line.transform.SetParent(files[fileIndex].transform, false);

        // Create the MeshCollider for the data object
        GameObject lineMesh = new GameObject();
        MeshCollider meshCollider = lineMesh.AddComponent<MeshCollider>();
        Mesh newMesh = new Mesh();
        line.GetComponent<LineRenderer>().BakeMesh(newMesh, true);
        meshCollider.sharedMesh = newMesh;
        lineMesh.transform.SetParent(files[fileIndex].transform, false);

        line.SetActive(true);

        // Add the LineRenderer to the list in rocketTrajectory
        //rocket.lineList.Add(line.GetComponent<LineRenderer>());

        // Create the VisualisationPoints object for this trajectory
        GameObject point = new GameObject();
        point.SetActive(false);
        point.AddComponent<VisualisationPoints>();
        point.GetComponent<VisualisationPoints>().setVisualisationMesh(mesh);
        //point.GetComponent<VisualisationPoints>().setDataPointPrefab(dataPointPrefab);
        point.GetComponent<VisualisationPoints>().setPointMaterial(mat);
        point.transform.SetParent(files[fileIndex].transform, false);
        point.SetActive(true);
        point.GetComponent<VisualisationPoints>().createPoints();
    }


    // Creates TextAsset object from a filepath
    private TextAsset textfromFile(string path)
    {
        var sr = new StreamReader(path);
        string contents = sr.ReadToEnd();
        sr.Close();
        return new TextAsset(contents);
    }
}
