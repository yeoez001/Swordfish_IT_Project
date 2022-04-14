using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour
{
    public Material hoverMaterial;
    public Material selectedMaterial;

    private Single[] data;
    private bool selected = false;
    private MeshRenderer meshRenderer;
    private Material originalMaterial;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

    public void SetData(Single[] data)
    {
        this.data = data;
    }

    public void DebugToScreen()
    {
        String values = "";
        for (var i = 0; i < data.Length; i++)
            values += data[i] + " ";
        Debug.Log(values);
    }

    private void OnTriggerEnter(Collider other)
    {
        ControllerSelectionCone cone = other.gameObject.GetComponent<ControllerSelectionCone>();

        // Collided with controller 
        if (cone)
        {
            // Change to Hover colour
            meshRenderer.material = hoverMaterial;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        ControllerSelectionCone cone = other.gameObject.GetComponent<ControllerSelectionCone>();

        // Collided with controller cone
        if (cone)
        {
            if (meshRenderer.material != selectedMaterial)
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
            meshRenderer.material = originalMaterial;
        } 
        // Was not selected, now select
        else
        {
            selected = true;
            meshRenderer.material = selectedMaterial;
            DebugToScreen();
        }
        
    }
}
