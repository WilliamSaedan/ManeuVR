using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class Thruster : MonoBehaviour
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
        if (playerBody == null)
        {
            playerBody = player.GetComponent<Rigidbody>();
        }
        playerVelocity = player.GetComponent<VelocityEstimator>();
        interactable = this.GetComponentInParent<Interactable>();
        float idealDrag = playerSpeed / terminalVelocity;
        playerBody.drag = idealDrag / (idealDrag * Time.fixedDeltaTime + 1);
        if (flame != null)
            flame.SetActive(false);
    }

    public void OnPressDown()
    {
        if (flame != null)
            flame.SetActive(true);
    }

    // Start is called before the first frame update
    public void OnPress()
    {
        //playerBody.velocity = hand.transform.forward*playerSpeed;
        playerBody.AddForce(hand.transform.forward*playerSpeed*Time.deltaTime,ForceMode.Impulse);
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
            hand = interactable.attachedToHand;
    }
}
