using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System.IO;

public class DataFiles : MonoBehaviour
{
    //public List<CSVDataSource> files;
    public Visualisation visualisation;
    public GameObject dataPointPrefab;
    public float[] dimensionMin;
    public float[] dimensionMax;
    private int maxIndexX = 0;
    private int maxIndexY = 0;
    private int maxIndexZ = 0;

    private string path = "/Resources/rocketData";
    private string nameStructure = "trajectory-";
    private List<CSVDataSource> files;

    [SerializeField]
    private RocketAnimation rocket;

    private void Start()
    {
        // First find files and create csvDataSource objects
        files = new List<CSVDataSource>();
        createObjects();

        dimensionMin = new float[files[0].DimensionCount];
        dimensionMax = new float[files[0].DimensionCount];

        // TODO
        // INEFFICIENT?? TEMP FIX 
        // Determine global min/max for scaling
        // Loop each file
        for (int i = 0; i < files.Count; i++)
        {
            // Loop each variable in files
            for (int j = 0; j < files[i].DimensionCount; j++)
            {
                // Get min and max values for variable and compare against current
                // global min/max
                if (i == 0)
                {
                    dimensionMin[j] = files[i].getDimensions()[j].MetaData.minValue;
                }
                else if (files[i].getDimensions()[j].MetaData.minValue < dimensionMin[j])
                {
                    dimensionMin[j] = files[i].getDimensions()[j].MetaData.minValue;
                }

                if (i == 0)
                {
                    dimensionMax[j] = files[i].getDimensions()[j].MetaData.maxValue;
                }
                else if (files[i].getDimensions()[j].MetaData.maxValue > dimensionMax[j])
                {
                    dimensionMax[j] = files[i].getDimensions()[j].MetaData.maxValue;

                    // VERY inelegant solution
                    // Finding the file indexes which have the largest x,y,z values so we
                    // can set the axis ticks correctly
                    if (j == 1)
                    {
                        maxIndexX = i;
                    }
                    else if (j == 2)
                    {
                        maxIndexY = i;
                    }
                    else if (j == 3)
                    {
                        maxIndexZ = i;
                    }
                }
            }
        }

        // For each data file, create the trajectory within the visualisation object.
        for (int i = 0; i < files.Count; i++)
        {
            // Rescale the values based upon global min/max
            files[i].repopulate(dimensionMin, dimensionMax);

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
            line.GetComponent<VisualisationLine>().setVisualisationMesh(mesh);
            line.GetComponent<VisualisationLine>().setLineMaterial(mat);
            line.transform.SetParent(files[i].transform, false);

            GameObject lineMesh = new GameObject();
            MeshCollider meshCollider = lineMesh.AddComponent<MeshCollider>();
            Mesh newMesh = new Mesh();
            line.GetComponent<LineRenderer>().BakeMesh(newMesh, true);
            meshCollider.sharedMesh = newMesh;
            lineMesh.transform.SetParent(files[i].transform, false);

            line.SetActive(true);

            // Add the LineRenderer to the list in rocketTrajectory
            rocket.lineList.Add(line.GetComponent<LineRenderer>());

            // Create the VisualisationPoints object for this trajectory
            GameObject point = new GameObject();
            point.SetActive(false);
            point.AddComponent<VisualisationPoints>();
            point.GetComponent<VisualisationPoints>().setVisualisationMesh(mesh);
            point.GetComponent<VisualisationPoints>().setDataPointPrefab(dataPointPrefab);
            point.GetComponent<VisualisationPoints>().setPointMaterial(mat);
            point.transform.SetParent(files[i].transform, false);
            point.SetActive(true);
            point.GetComponent<VisualisationPoints>().createPoints();
        }

        // After all trajectories have been created, create a new object to set the
        // axis ticks correctly
        UpdateAxisTicks();

        // After final view has loaded, delete it from the visualisation object
        // All the trajectory data is in the visualisationPoints and visualisationLines objects .
        visualisation.destroyView();
    }

    // For each csv file in the directory, create a csvDataSourceObject
    private void createObjects()
    {
        string[] filePaths = Directory.GetFiles(Application.dataPath + path, "*.csv");
        for (int i = 0; i < filePaths.Length; i++)
        {
            // Create new game object with CSVDataSource component
            GameObject dataSourceObj = new GameObject("DataSource" + (i + 1));
            dataSourceObj.transform.SetParent(this.transform, false);
            
            dataSourceObj.AddComponent<CSVDataSource>();

            // Set CSVDataSource data to file data.
            TextAsset data = textfromFile(path + "/" + nameStructure + (i + 1) + ".csv");
            dataSourceObj.GetComponent<CSVDataSource>().data = data;
            dataSourceObj.GetComponent<CSVDataSource>().loadHeader();
            dataSourceObj.GetComponent<CSVDataSource>().load();
            files.Add(dataSourceObj.GetComponent<CSVDataSource>());
        }
    }

    // Creates TextAsset object from a filepath
    private TextAsset textfromFile(string path)
    {
        var sr = new StreamReader(Application.dataPath + path);
        string contents = sr.ReadToEnd();
        sr.Close();
        return new TextAsset(contents);
    }

    public List<CSVDataSource> GetFiles()
    {
        return files;
    }

    // Update the axis ticks 
    private void UpdateAxisTicks()
    {
        // Update X axis
        DestroyImmediate(visualisation.theVisualizationObject.X_AXIS);
        visualisation.dataSource = files[maxIndexY];
        visualisation.theVisualizationObject.ReplaceAxis(AbstractVisualisation.PropertyType.X);

        // Update Y axis
        DestroyImmediate(visualisation.theVisualizationObject.Y_AXIS);
        visualisation.dataSource = files[maxIndexZ];
        visualisation.theVisualizationObject.ReplaceAxis(AbstractVisualisation.PropertyType.Y);

        // Update Z axis
        DestroyImmediate(visualisation.theVisualizationObject.Z_AXIS);
        visualisation.dataSource = files[maxIndexX];
        visualisation.theVisualizationObject.ReplaceAxis(AbstractVisualisation.PropertyType.Z);
    }
}
