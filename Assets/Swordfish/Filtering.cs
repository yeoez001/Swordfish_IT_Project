using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System;
using UnityEditor;

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
    public string filterValue;

    [SerializeField]
    private float maxFilter = 0.0f;
    [SerializeField]
    private float minFilter = 0.0f;
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

        // Do filter logic
        if (filterValue != "None" && filterValue != currentFilter || doFilter == true) {
            int index = dataNames.IndexOf(filterValue);

            if (doFilter == true)
            {
                filterBetween(index, minFilter, maxFilter);
                doFilter = false;
            }

            currentFilter = filterValue;
        }
        else if (filterValue == "None" && filterValue != currentFilter)
        {
            resetFilters();
            currentFilter = filterValue;
        }
    }

    // Filter trajectories based on provided values
    public void filterBetween(int index, float min, float max) 
    {
        try
        {
            for (int i = 0; i < dataObjects.Count; i++)
            {
                if (dataObjects[i].GetComponent<CSVDataSource>().getDimensions()[index].MetaData.maxValue <= max)
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
    public void resetFilters()
    {
        foreach(GameObject trajectory in dataObjects)
        {
            trajectory.SetActive(true);
        }
    }
}
