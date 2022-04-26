using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class CustomSlider : MonoBehaviour
{
    [SerializeField]
    public SteamVR_Action_Single squeezeAction;

    private GameObject selectingCone;
    private Hand selectingHand;

    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }
    private void Update()
    {
        if (selectingCone)
        {
            if (squeezeAction.GetAxis(selectingHand.handType) > 0.0f)
                transform.position = new Vector3(originalPos.x, originalPos.y, selectingCone.transform.position.z);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        ControllerSelectionCone cone = other.gameObject.GetComponent<ControllerSelectionCone>();
        // Collided with controller cone
        if (cone)
        {
            Hand hand = cone.GetComponentInParent<Hand>();
            if (hand)
            {
                if (squeezeAction.GetAxis(hand.handType) > 0.0f)
                {
                    selectingCone = other.gameObject;
                    selectingHand = hand;
                }
            }
        }
    }
}
