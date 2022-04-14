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
    public Material selectedMaterial;

    private bool success = true;

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

    private void OnTriggerStay(Collider other)
    {
        Hand hand = GetComponentInParent<Hand>();
        if (hand)
        {
            if (selectAction.GetStateDown(GetComponentInParent<Hand>().handType))
            {
                DataPoint point = other.gameObject.GetComponent<DataPoint>();
                if (point)
                {
                    point.Select();   
                }
            }
        }
    }
}
