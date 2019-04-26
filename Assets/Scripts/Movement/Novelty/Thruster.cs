﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class Thruster : MonoBehaviour
{
    private Player player;
    private Rigidbody playerBody;
    private VelocityEstimator playerVelocity;
    private Hand hand;
    private Interactable interactable;
    private SteamVR_Behaviour_Boolean caller;

    public float playerSpeed = 100f;
    public float terminalVelocity = Mathf.Infinity;
    public GameObject flame;

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
        float idealDrag = playerSpeed / terminalVelocity;
        playerBody.drag = idealDrag / (idealDrag * Time.fixedDeltaTime + 1);
        //hand = player.rightHand;
        if (flame != null)
            flame.SetActive(false);
    }

    public void OnPressDown()
    {
        if (interactable.attachedToHand)
        {
            if (flame != null)
                flame.SetActive(true);
        }
    }

    // Start is called before the first frame update
    public void OnPress()
    {
        //playerBody.velocity = hand.transform.forward*playerSpeed;
        if (interactable.attachedToHand)
        {
            playerBody.AddForce(hand.transform.forward * playerSpeed * Time.deltaTime, ForceMode.Impulse);
        }
    }

    public void OnPressUp()
    {
        if (flame != null)
            flame.SetActive(false);
        playerBody.velocity = playerVelocity.GetVelocityEstimate();
    }

    public void AssignController()
    {
        if (hand == null)
        {
            hand = interactable.attachedToHand;
        }
        //caller.ChangeInputSource(hand.handType);
    }
}
