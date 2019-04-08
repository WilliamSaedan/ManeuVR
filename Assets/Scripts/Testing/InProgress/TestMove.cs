using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TestMove : MonoBehaviour
{
    public SteamVR_Action_Boolean moveAction;

    private Player player;

    private Rigidbody playerBody;

    private void OnEnable()
    {
        if (player == null)
            player = Player.instance;

        if (moveAction == null)
        {
            Debug.LogError("No plant action assigned");
            return;
        }

        //plantAction.AddOnChangeListener(OnPlantActionChange, hand.handType);
    }

    public void MoveDude()
    {
        playerBody = player.GetComponent<Rigidbody>();
        playerBody.AddForce(Vector3.up*1000f);
    }
}
