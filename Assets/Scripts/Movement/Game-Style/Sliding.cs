using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Sliding : MonoBehaviour
{
    private Player player;
    private Rigidbody playerBody;
    private VelocityEstimator playerVelocity;
    private Hand hand;
    private Interactable interactable;
    private Vector3 handMotionRef;
    private SteamVR_Behaviour_Boolean caller;

    public float velocityAmplification = 1f;

    void OnEnable()
    {
        player = Player.instance;
        if (playerBody == null)
        {
            playerBody = player.GetComponent<Rigidbody>();
        }
        playerVelocity = player.GetComponent<VelocityEstimator>();
        interactable = this.GetComponentInParent<Interactable>();
        caller = this.GetComponent<SteamVR_Behaviour_Boolean>();
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
            playerBody.velocity = Vector3.zero;
            Vector3 deltaMovment = new Vector3((handMotionRef - hand.transform.localPosition).x, 0, (handMotionRef - hand.transform.localPosition).z) * velocityAmplification;
            playerBody.transform.position += deltaMovment;
        }
    }

    public void OnPressUp()
    {
        if (interactable.attachedToHand)
        {
            playerBody.useGravity = true;
            playerBody.velocity = playerVelocity.GetVelocityEstimate() * velocityAmplification;
        }
    }
}
