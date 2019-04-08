using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PointFollow : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, -this.transform.right, Color.yellow);
    }

    private Player player;
    private Rigidbody playerBody;
    private VelocityEstimator playerVelocity;
    private Hand hand;
    private Interactable interactable;
    private Vector3 handMotionRef;
    private bool canMove = false;
    private bool moved = false;

    public float playerSpeed = 1f;
    public float maxDistance = 2f;

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

    public void GetReferencePoint()
    {
        canMove = Physics.Raycast(this.transform.position - this.transform.right/4, -this.transform.right, out hitInfo, maxDistance);
    }

    public void DuringPress()
    {
        if ( canMove )
        {
            playerBody.velocity = Vector3.Normalize(hitInfo.point - this.transform.position) * playerSpeed;
            moved = false;
        }
    }

    public void OnRelease()
    {
        if( moved )
        {
            StartCoroutine(SlowSpeed());
            moved = false;
        }
    }

    public void AssignController()
    {
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
