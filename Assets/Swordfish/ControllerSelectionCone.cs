using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerSelectionCone : MonoBehaviour
{   
    [SerializeField]
    public SteamVR_Action_Boolean selectAction;
    public Material highlightMaterial;

    private bool success = false;

    private GameObject currentGO;
    private Material currentGOMat;

    void Update()
    {
        if (!success)
        {
            RenderModel controller = transform.parent.GetComponentInChildren<RenderModel>();
            if (controller != null)
            {
                transform.parent = controller.gameObject.transform;
                success = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentGO == null)
        {
            currentGO = other.gameObject;
            currentGOMat = currentGO.GetComponent<MeshRenderer>().material;
            if (highlightMaterial)
                currentGO.GetComponent<MeshRenderer>().material = highlightMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentGO)
        {
            currentGO.GetComponent<MeshRenderer>().material = currentGOMat;
            currentGOMat = null;
            currentGO = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == currentGO && selectAction.GetStateDown(GetComponentInParent<Hand>().handType))
        {
            Debug.Log("Selected");
        }
    }
}
