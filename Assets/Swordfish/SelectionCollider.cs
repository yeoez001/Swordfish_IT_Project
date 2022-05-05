using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

public class SelectionCollider : MonoBehaviour
{
    public BooleanAction selectionAction;

    private bool released = true;

    private void OnTriggerStay(Collider other)
    {
        if (released && selectionAction.IsActivated)
        {
            released = false;
            DataPoint point = other.gameObject.GetComponent<DataPoint>();
            if (point)
            {
                point.Select();
            }
        }

        if (selectionAction.IsActivated == false)
        {
            released = true;
        }
    }
}
