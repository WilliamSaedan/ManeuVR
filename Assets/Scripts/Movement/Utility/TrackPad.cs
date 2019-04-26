using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TrackPad : MonoBehaviour
{
    private Player player;
    private Rigidbody playerBody;
    private Hand hand;
    private Interactable interactable;
    private SteamVR_Behaviour_Vector2 caller;

    public float playerSpeed = 100f;
    public SteamVR_Action_Vector2 touchAction;


    void OnEnable()
    {
        player = Player.instance;
        interactable = this.GetComponentInParent<Interactable>();
        caller = this.GetComponent<SteamVR_Behaviour_Vector2>();
        if (playerBody == null)
        {
            playerBody = player.GetComponent<Rigidbody>();
        }
        //hand = player.rightHand;
    }

    public void OnChange()
    {
        if (interactable.attachedToHand)
        {
            Vector2 touchPosition = touchAction.GetAxis(hand.handType);
            Vector3 moveVector = Vector3.ProjectOnPlane(touchPosition.x*player.hmdTransform.transform.right + touchPosition.y * player.hmdTransform.transform.forward, Vector3.up);
            playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);
        }
    }

    public void AssignController()
    {
        if (hand == null || hand != interactable.attachedToHand)
        {
            hand = interactable.attachedToHand;
        }
        //caller.ChangeInputSource(hand.handType);
    }
}
