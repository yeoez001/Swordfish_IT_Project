using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;
using IATK;

public class SelectionCollider : MonoBehaviour
{
    // Controller action to select a data point to show its data display
    public BooleanAction dataPointSelectionAction;
    // Controller action to select a data point to set the trajectory as the animation source
    public BooleanAction trajectoryAnimationSelectionAction;

    // dataPointSelectionAction is not being pressed
    private bool released = true;

    private void OnTriggerStay(Collider other)
    {
        // Activated selectionAction after having released it previously
        if (released && dataPointSelectionAction.IsActivated)
        {
            released = false;

            // Selected object is a DataPoint
            DataPoint point = other.gameObject.GetComponent<DataPoint>();
            if (point)
            {
                // Select DataPoint (i.e. show its data display)
                point.Select();
            }
        }

        // Released the DataPoint selection action
        if (dataPointSelectionAction.IsActivated == false)
        {
            released = true;
        }

        // Activated the trajectory selection action
        if (trajectoryAnimationSelectionAction.IsActivated)
        {
            // Selected object is a DataPoint
            DataPoint point = other.gameObject.GetComponent<DataPoint>();
            if (point)
            {
                // Get the rocket from the visualisation that the selected data point belongs to
                Visualisation visualisation = point.GetComponentInParent(typeof(Visualisation)) as Visualisation;
                RocketAnimation rocket = visualisation.GetComponentInChildren<RocketAnimation>();
                // Visualisation has a rocket
                if (rocket)
                {
                    // Set the rocket to animate over the trajectory of the selected data point
                    rocket.setSelectedTrajectory(point.gameObject);
                }
            }
        }
    }
}
