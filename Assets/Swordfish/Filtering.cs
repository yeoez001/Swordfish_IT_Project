using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;


public class Filtering : MonoBehaviour
{
    public static List<string> dataNames = new List<string>() { "None" }; // Gets the data names to populate below dropdown list
    [FilteringEditor(typeof(Filtering), "dataNames")]
    public string filterVariable;   // Dropdown list in Unity Editor to select filtering variables

    // Filtering types for the Unity Editor dropdown.
    public enum FilterType
    {
        None,
        HideAbove,
        HideBelow,
        Between
    }
    [SerializeField]
    private FilterType filterType = new FilterType();

    [SerializeField]
    private float filterValue = 0.0f;
    [SerializeField]
    private float filterRangeMin = 0.0f;
    [SerializeField]
    private bool doFilter = false;

    [SerializeField]
    private DataFiles files;
    private List<GameObject> dataObjects = new List<GameObject>();
    private bool loaded = false;
    private string currentFilter = "None";

    // Update is called once per frame
    void Update()
    {
        // Load the data sources
        if (loaded == false)
        {

            int children = files.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                dataObjects.Add(files.transform.GetChild(i).gameObject);
            }

            // Add the data names to the unity editor drop down box
            for (int i = 0; i < dataObjects[0].GetComponent<CSVDataSource>().getDimensions().Count; i++)
            {
                dataNames.Add(dataObjects[0].GetComponent<CSVDataSource>().getDimensions()[i].Identifier);
            }
            loaded = true;
        }

        // Filtering logic for the editor
        // UNCOMMENT AND UI FILTERING WILL NOT WORK.
        //FilterThroughEditor();
    }

    // Hide all values BELOW a provided filterValue
    public void FilterAbove(int index, float filterValue) 
    {
        try
        {
            for (int i = 0; i < dataObjects.Count; i++)
            {
                if (dataObjects[i].GetComponent<CSVDataSource>().getDimensions()[index].MetaData.maxValue <= filterValue)
                {
                    files.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    // Hide all values ABOVE a provided filterValue
    public void FilterBelow(int index, float filterValue)
    {
        try
        {
            for (int i = 0; i < dataObjects.Count; i++)
            {
                if (dataObjects[i].GetComponent<CSVDataSource>().getDimensions()[index].MetaData.maxValue >= filterValue)
                {
                    files.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    // Hide all values outside of a provided range.
    public void FilterBetween(int index, float maxValue, float minValue)
    {
        try
        {
            for (int i = 0; i < dataObjects.Count; i++)
            {
                if (dataObjects[i].GetComponent<CSVDataSource>().getDimensions()[index].MetaData.maxValue >= maxValue ||
                    dataObjects[i].GetComponent<CSVDataSource>().getDimensions()[index].MetaData.maxValue <= minValue)
                {
                    files.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    // Remove all filters
    public void ResetFilters()
    {
        foreach(GameObject trajectory in dataObjects)
        {
            trajectory.SetActive(true);
        }
    }

    // Run the filter
    public void RunFilter(List<float> filterValues)
    {
        int index = dataNames.IndexOf(filterVariable) - 1;

        // Determine filter type
        if (filterType == FilterType.HideBelow)
        {
            FilterAbove(index, filterValues[0]);
        }
        else if (filterType == FilterType.HideAbove)
        {
            FilterBelow(index, filterValues[0]);
        }
        else if (filterType == FilterType.Between)
        {
            FilterBetween(index, filterValues[1], filterValues[0]);
        }
        else if (filterType == FilterType.None)
        {
            ResetFilters();
        }
        
        currentFilter = filterVariable;
    }

    // Set the filter type to run
    public void SetFilterType(int filterTypeIndex)
    {        
        // Check which filter type was selected in the dropdwon
        switch (filterTypeIndex)
        {
            // Based on the order the Enums were added to the dropdown options
            case 0:
                filterType = FilterType.None;
                break;
            case 1:
                filterType = FilterType.HideBelow;
                break;
            case 2:
                filterType = FilterType.HideAbove;
                break;
            case 3:
                filterType = FilterType.Between;
                break;
        }
    }

    // Test filtering through the Unity Editor
    private void FilterThroughEditor()
    {
        // filter logic
        if (doFilter == true)
        {
            int index = dataNames.IndexOf(filterVariable)-1;


            if (filterVariable != "None")
            {
                // Determine filter type
                if (filterType == FilterType.HideBelow)
                {
                    FilterBelow(index, filterValue);
                }
                else if (filterType == FilterType.HideAbove)
                {
                    FilterAbove(index, filterValue);
                }
                else if (filterType == FilterType.Between)
                {
                    FilterBetween(index, filterValue, filterRangeMin);
                }
            }
            doFilter = false;
            currentFilter = filterVariable;
        }
        else if (filterVariable == "None" && filterVariable != currentFilter)
        {
            ResetFilters();
            currentFilter = filterVariable;
        }
    }

    public void SetFilterVariable(string filterString)
    {
        filterVariable = filterString;
    }

    public List<string> GetDataNames()
    {
        return dataNames;
    }

    public List<GameObject> GetDataObjects()
    {
        return dataObjects;
    }
}
