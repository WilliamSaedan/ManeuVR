using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Climbing : MonoBehaviour
{
    private Player player;
    private Rigidbody playerBody;
    private VelocityEstimator playerVelocity;
    private Hand hand;
    private Interactable interactable;
    private Vector3 handMotionRef;
    private SteamVR_Behaviour_Boolean caller;

    void OnEnable()
    {
        player = Player.instance;
        if (playerBody == null)
        {
            playerBody = player.GetComponent<Rigidbody>();
        }
        playerVelocity = player.GetComponent<VelocityEstimator>();
        caller = this.GetComponent<SteamVR_Behaviour_Boolean>();
        interactable = this.GetComponentInParent<Interactable>();
    }

    public void GetReferencePoint()
    {
        if (interactable.attachedToHand)
        {
            if (hand == null)
            {
                hand = interactable.attachedToHand;
            }
            //caller.ChangeInputSource(hand.handType);
            handMotionRef = hand.transform.localPosition;
        }
    }

    public void OnPress()
    {
        if (interactable.attachedToHand)
        {
            playerBody.useGravity = false;
            playerBody.velocity = Vector3.zero;
            playerBody.transform.position += handMotionRef - hand.transform.localPosition;
        }
    }

    public void OnPressUp()
    {
        if (interactable.attachedToHand)
        {
            playerBody.useGravity = true;
            playerBody.velocity = playerVelocity.GetVelocityEstimate();
        }
    }
}
