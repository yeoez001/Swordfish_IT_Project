using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

public class DataFiles : MonoBehaviour
{
    public List<CSVDataSource> files;
    public Visualisation visualisation;
    public GameObject dataPointPrefab;
    public float[] dimensionMin;
    public float[] dimensionMax;
    private int maxIndexX = 0;
    private int maxIndexY = 0;
    private int maxIndexZ = 0;

    [SerializeField]
    private RocketTrajectory rocket;

    private void Start()
    {
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
            line.GetComponent<VisualisationLine>().visualisationMesh = mesh;
            line.GetComponent<VisualisationLine>().lineMat = mat;
            line.transform.SetParent(files[i].transform, false);
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

        // After all trajectories have been created, create a new object to set the
        // axis ticks correctly
        UpdateAxisTicks();

        // After final view has loaded, delete it from the visualisation object
        // All the trajectory data is in the visualisationPoints and visualisationLines objects .
        visualisation.destroyView();
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
