using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

public class ListPopupAttribute : PropertyAttribute
{
    public Type list;
    public string listName;

    public ListPopupAttribute(Type _list, string _listName)
    {
        list = _list;
        listName = _listName;
    }
}

[CustomPropertyDrawer(typeof(ListPopupAttribute))]
public class ListPopupDrawer : PropertyDrawer
{
    int selectedIndex = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ListPopupAttribute attr = attribute as ListPopupAttribute;
        List<string> stringList = null;

        if (attr.list.GetField(attr.listName) != null)
        {
            stringList = attr.list.GetField(attr.listName).GetValue(attr.list) as List<string>;
        }

        if (stringList != null && stringList.Count != 0)
        {
            selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex, stringList.ToArray());
            property.stringValue = stringList[selectedIndex];
        }
        else EditorGUI.PropertyField(position, property, label);
    }
}

// MAIN FILTERING CLASS 
public class Filtering : MonoBehaviour
{
    public static List<string> dataNames = new List<string>() { "None" };// = new List<string>();
    [ListPopup(typeof(Filtering), "dataNames")]
    public string filterVariable;

    // UI Components
    //public Dropdown filterVarDropdown;
  //  public Dropdown filterTypeDropdown;
   // public Keypad filterUIValue;

    public enum FilterType
    {
        None,
        HideAbove,
        HideBelow,
        Between
    }
    [SerializeField]
    private FilterType filterType = new FilterType();

   //[SerializeField]
   // private List<float> filterValues;
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

        // Do filter logic              UNCOMMENT TO WORK IN EDITOR. UI WILL THEN NOT WORK.
        //if (doFilter == true) {
        //    int index = dataNames.IndexOf(filterVariable)-1;


        //    if (filterVariable != "None")
        //    {
        //        // Determine filter type
        //        if (filterType == FilterType.HideBelow)
        //        {
        //            filterAbove(index, filterValue);
        //        }
        //        else if (filterType == FilterType.HideAbove)
        //        {
        //            filterBelow(index, filterValue);
        //        }
        //        else if (filterType == FilterType.Between)
        //        {
        //            filterBetween(index, filterValue, filterRangeMin);
        //        }
        //    }
        //    doFilter = false;
        //    currentFilter = filterVariable;
        //}
        //else if (filterVariable == "None" && filterVariable != currentFilter)
        //{
        //    resetFilters();
        //    currentFilter = filterVariable;
        //}
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
