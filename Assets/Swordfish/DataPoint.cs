using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using UnityEngine.UI;

public class DataPoint : MonoBehaviour
{
    public Material hoverMaterial;
    public Material selectedMaterial;
    public GameObject valuesDisplay;

    private Single[] data;
    private CSVDataSource dataSource;
    private bool selected = false;
    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    private String valuesString;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;

        // Values of data point as a string
        valuesString = "";
        for (var i = 0; i < data.Length; i++)
            valuesString += dataSource[i].Identifier + ": " + data[i] + "\n";
        valuesDisplay.GetComponentInChildren<Text>().text = valuesString;

        valuesDisplay.SetActive(false);
    }

    public void SetData(CSVDataSource source, Single[] data)
    {
        dataSource = source;
        this.data = data;
    }

    private void OnTriggerEnter(Collider other)
    {
        SelectionCollider cone = other.gameObject.GetComponent<SelectionCollider>();

        // Collided with controller 
        if (cone)
        {
            if (!selected)
            {
                // Change to Hover colour
                meshRenderer.material = hoverMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SelectionCollider cone = other.gameObject.GetComponent<SelectionCollider>();

        // Collided with controller cone
        if (cone)
        {
            if (!selected)
            {
                // Change back to original colour
                meshRenderer.material = originalMaterial;
            }
        }
    }

    public void Select()
    {
        // Was selected, now deselect
        if (selected)
        {
            selected = false;
            meshRenderer.material = hoverMaterial;
            if (valuesDisplay)
            {
                valuesDisplay.SetActive(false);
            }
        }
        // Was not selected, now select
        else
        {
            selected = true;
            meshRenderer.material = selectedMaterial;

            // Show UI display of values
            if (valuesDisplay)
            {
                valuesDisplay.SetActive(true);
            }
        }
    }

    public String GetValuesAsString()
    {
        return valuesString;
    }
}
