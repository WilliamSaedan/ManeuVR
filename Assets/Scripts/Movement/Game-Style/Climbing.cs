using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Climbing : MonoBehaviour
{
    private Player player;
    private Rigidbody playerBody;
    private VelocityEstimator playerVelocity;
    private Hand hand;
    private Interactable interactable;
    private Vector3 handMotionRef;

    void OnEnable()
    {
        player = Player.instance;
        if (playerBody == null)
        {
            playerBody = player.GetComponent<Rigidbody>();
        }
        playerVelocity = player.GetComponent<VelocityEstimator>();
        //TEMPORARY
        //hand = player.rightHand;
        interactable = this.GetComponentInParent<Interactable>();
    }

    public void GetReferencePoint()
    {
        hand = interactable.attachedToHand;
        handMotionRef = hand.transform.localPosition;
    }

    public void DuringPress()
    {
        playerBody.useGravity = false;
        playerBody.velocity = Vector3.zero;
        //playerBody.transform.position = Vector3.MoveTowards(playerBody.transform.position , playerBody.transform.position + (handMotionRef - hand.transform.localPosition), 10f*Time.deltaTime);
        playerBody.transform.position += handMotionRef - hand.transform.localPosition;
    }

    public void OnRelease()
    {
        playerBody.useGravity = true;
        playerBody.velocity = playerVelocity.GetVelocityEstimate();
    }
}
