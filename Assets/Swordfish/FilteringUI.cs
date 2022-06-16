using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IATK;

public class FilteringUI : MonoBehaviour
{
    // UI Dropdown for selecting the filter variable
    public Dropdown filterVarDropdown;
    // UI Dropdown for selecting the filter type
    public Dropdown filterTypeDropdown;
    // UI InputField for entering the filter value when only 1 is needed
    public InputField singleFilterInputField;
    // UI InputField for entering the minimum filter value for the 'Between'/ range filter
    public InputField minFilterInputField;
    // UI InputField for entering the maximum filter value for the 'Between'/ range filter
    public InputField maxFilterInputField;

    private bool loaded;
    private Filtering filtering;
    private List<string> dataNames;

    void Start()
    {
        filtering = GetComponent<Filtering>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (!loaded)
        {
            initialLoad();
        }
    }

    private void initialLoad()
    {
        // Populate UI component with data source options
        dataNames = filtering.GetDataNames();

        if (dataNames.Count > 1)
        {
            // Load option to UI dropdowns if not null
            if (filterVarDropdown && filterTypeDropdown)
            {
                // Add data names to UI dropdown options
                filterVarDropdown.AddOptions(dataNames);

                // Add filter types to UI dropdown options
                List<string> filterTypesStrings = new List<string>();
                filterTypesStrings.Add(Filtering.FilterType.None.ToString());
                filterTypesStrings.Add(Filtering.FilterType.HideBelow.ToString());
                filterTypesStrings.Add(Filtering.FilterType.HideAbove.ToString());
                filterTypesStrings.Add(Filtering.FilterType.Between.ToString());
                filterTypeDropdown.AddOptions(filterTypesStrings);
            }
            loaded = true;
        }
    }

    public void UISetFilterVariable()
    {
        // Set the filter variable based on the text of the selected UI dropdown option
        filtering.SetFilterVariable(filterVarDropdown.options[filterVarDropdown.value].text);
    }

    public void UISetFilterType()
    {
        filtering.SetFilterType(filterTypeDropdown.value);
        // Set active of input fields according to filter type
        // Filter type is Between
        if (filterTypeDropdown.value == 3)
        {
            // Show min and max input fields
            singleFilterInputField.gameObject.SetActive(false);
            minFilterInputField.gameObject.SetActive(true);
            maxFilterInputField.gameObject.SetActive(true);
        } 
        // Filtering type is None
        else if (filterTypeDropdown.value == 0) 
        {
            // Hide all input fields
            singleFilterInputField.gameObject.SetActive(false);
            minFilterInputField.gameObject.SetActive(false);
            maxFilterInputField.gameObject.SetActive(false);
        } else
        {
            singleFilterInputField.gameObject.SetActive(true);
            minFilterInputField.gameObject.SetActive(false);
            maxFilterInputField.gameObject.SetActive(false);
        }
    }

    public void UIRunFilter()
    {
        List<float> inputValues = new List<float>();

        // Filter type is Between
        if (filterTypeDropdown.value == 3)
        {
            // Get two values for the min and max value of range
            inputValues.Add(float.Parse(minFilterInputField.text));
            inputValues.Add(float.Parse(maxFilterInputField.text));
        } 
        // Filter type if not None
        else if (filterTypeDropdown.value != 0) {
            // Get single value for the filter value 
            inputValues.Add(float.Parse(singleFilterInputField.text));
        }
        filtering.RunFilter(inputValues);
    }
}
