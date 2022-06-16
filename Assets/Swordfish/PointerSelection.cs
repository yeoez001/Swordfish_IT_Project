using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using static Zinnia.Pointer.ObjectPointer;

// Used for registering pointer selection events on the Pointer Facade
public class PointerSelection : MonoBehaviour
{    public void PointerSelect(EventData eventData)
    {
        // Selected something
        if (eventData != default)
        {
            // Check selected a valid gameObject with a transform (i.e.not a UI component)
            Transform hitTransform = eventData.CollisionData.transform;
            if (hitTransform)
            {
                // Selected a data point object
                DataPoint dataPoint = hitTransform.gameObject.GetComponent<DataPoint>();
                if (dataPoint)
                {
                    // Get the rocket from the visualisation that the selected data point belongs to
                    Visualisation visualisation = dataPoint.GetComponentInParent(typeof(Visualisation)) as Visualisation;
                    RocketAnimation rocket = visualisation.GetComponentInChildren<RocketAnimation>();
                    if (rocket)
                    {
                        rocket.setSelectedTrajectory(dataPoint.gameObject);
                    }
                }
            }
        }
    }
}
