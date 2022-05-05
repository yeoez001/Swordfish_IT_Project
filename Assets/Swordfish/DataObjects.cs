using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;

public class DataObjects : MonoBehaviour
{
    [SerializeField]
    private List<CSVDataSource> dataSources;
    private int selected = 0;

    // TODO
    // Method which changes the currently selected trajectory.


    // Adds a new dataSource (i.e. trajectory object) to visualisation list. 
    public void addSources(List<CSVDataSource> sources)
    {
        dataSources = sources;
    }

    // Returns the VisualisationLine object of the currently selected dataSource
    public VisualisationLine getSelectedLine()
    {
        if (dataSources.Count == 0)
        {
            return null;
        }
        return (VisualisationLine)dataSources[selected].GetComponentInChildren(typeof(VisualisationLine));
        //.GetComponent<VisualisationLine>();
    }

    // Returns the VisualisationPoints object of the currently selected dataSource
    public VisualisationPoints getSelectedPoints()
    {
        if (dataSources.Count == 0)
        {
            return null;
        }
        return (VisualisationPoints)dataSources[selected].GetComponentInChildren(typeof(VisualisationPoints));
        //return dataSources[selected].GetComponent<VisualisationPoints>();
    }
}
