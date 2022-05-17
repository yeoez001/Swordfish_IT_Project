using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;
using IATK;

public class SelectionCollider : MonoBehaviour
{
    public BooleanAction selectionAction;
    public BooleanAction trajectorySelectionAction;

    private bool released = true;

    private void OnTriggerStay(Collider other)
    {
        // Activated selectionAction after having released it previously
        if (released && selectionAction.IsActivated)
        {
            released = false;
            DataPoint point = other.gameObject.GetComponent<DataPoint>();
            if (point)
            {
                point.Select();
            }
        }

        // Released the selection action
        if (selectionAction.IsActivated == false)
        {
            released = true;
        }

        if (trajectorySelectionAction.IsActivated)
        {
            DataPoint point = other.gameObject.GetComponent<DataPoint>();
            if (point)
            {
                // Get the rocket from the visualisation that the selected data point belongs to
                Visualisation visualisation = point.GetComponentInParent(typeof(Visualisation)) as Visualisation;
                RocketTrajectory rocket = visualisation.GetComponentInChildren<RocketTrajectory>();
                if (rocket)
                {
                    rocket.SetSelectedTrajectory(point.gameObject);
                }
            }
        }
    }
}
