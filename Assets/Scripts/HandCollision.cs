using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandCollision : MonoBehaviour
{
    public Transform collider;
    private Rigidbody rigidbody;
    private bool colliding;
    //public Rigidbody player;
    //public PlayerDebug playerVelocity;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector3.Normalize(transform.position - collider.position) * 10f;
        if (!colliding)
        {
            collider.rotation = Quaternion.RotateTowards(collider.rotation, transform.rotation, 10f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
        //player.velocity = playerVelocity.velocity_()*2;
    }
}
