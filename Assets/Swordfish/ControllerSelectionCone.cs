using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerSelectionCone : MonoBehaviour
{   
    [SerializeField]
    public SteamVR_Action_Boolean selectAction;

    private bool success = false;

    public SteamVR_Behaviour_Pose controllerBehaviourPose;

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
        if (selectAction.GetStateDown(controllerBehaviourPose.inputSource))
        {
            DataPoint point = other.gameObject.GetComponent<DataPoint>();
            if (point)
            {
                point.Select();   
            }
        }        
    }
}
