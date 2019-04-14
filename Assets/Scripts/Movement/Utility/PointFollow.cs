using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

//[RequireComponent(typeof(LineRenderer))]

public class PointFollow : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, this.transform.up, Color.yellow);
    }

    private Player player;
    private Rigidbody playerBody;
    private VelocityEstimator playerVelocity;
    private Hand hand;
    private Interactable interactable;
    private Vector3 handMotionRef;
    private bool canMove = false;
    private bool moved = false;

    public float playerSpeed = 100f;
    public float maxDistance = Mathf.Infinity;

    private RaycastHit hitInfo;

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

    public void OnPressDown()
    {
        canMove = Physics.Raycast(this.transform.position + this.transform.up/3, this.transform.up, out hitInfo, maxDistance);
    }

    public void OnPress()
    {
        if ( canMove )
        {
            playerBody.velocity = Vector3.Normalize(hitInfo.point - this.transform.position) * playerSpeed * Time.deltaTime;
            moved = false;
        }
    }

    public void OnPressUp()
    {
        if( moved )
        {
            StartCoroutine(SlowSpeed());
            moved = false;
        }
    }

    public void AssignController()
    {
        if (hand == null)
            hand = interactable.attachedToHand;
    }

    private IEnumerator SlowSpeed()
    {
        float startTime = Time.time;
        float overTime = 0.25f;
        float endTime = startTime + overTime;

        Vector3 initialVelocity = playerBody.velocity;
        Vector3 targetVelocity = Vector3.zero;

        while (Time.time < endTime)
        {
            playerBody.velocity = Vector3.Slerp(initialVelocity, targetVelocity, (Time.time - startTime) / overTime);
            yield return null;
        }
    }
}
