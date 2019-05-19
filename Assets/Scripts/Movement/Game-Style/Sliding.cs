using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Sliding : MonoBehaviour
{
    private Player player;
    private Rigidbody playerBody;
    private VelocityEstimator playerVelocity;
    private Hand hand;
    private Interactable interactable;
    private Vector3 handMotionRef;

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
    }

    public void GetReferencePoint()
    {
        if (hand == null)
            hand = interactable.attachedToHand;
        handMotionRef = hand.transform.localPosition;
    }

    public void OnPress()
    {
        playerBody.velocity = Vector3.zero;
        Vector3 deltaMovment = new Vector3((handMotionRef - hand.transform.localPosition).x, 0, (handMotionRef - hand.transform.localPosition).z) * velocityAmplification;
        playerBody.transform.position += deltaMovment;
    }

    public void OnPressUp()
    {
        playerBody.useGravity = true;
        playerBody.velocity = playerVelocity.GetVelocityEstimate() * velocityAmplification;
    }
}
