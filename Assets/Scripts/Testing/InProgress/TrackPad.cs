using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TrackPad : MonoBehaviour
{
    private Player player;
    private Rigidbody playerBody;
    private VelocityEstimator playerVelocity;
    private Hand hand;
    private Interactable interactable;

    public float playerSpeed = 100f;
    public float terminalVelocity = Mathf.Infinity;
    public GameObject flame;

    void OnEnable()
    {
        player = Player.instance;
        interactable = this.GetComponentInParent<Interactable>();
    }

    public void OnChange()
    {
        
    }

    public void AssignController()
    {
        if (hand == null)
            hand = interactable.attachedToHand;
    }
}
