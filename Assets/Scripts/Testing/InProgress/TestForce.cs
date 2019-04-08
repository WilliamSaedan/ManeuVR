using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForce : MonoBehaviour
{
    public Transform controller;
    private Rigidbody rigidbody;
    private bool colliding;
    // Update is called once per frame
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector3.Normalize(controller.position - transform.position) * 10f;
        if (!colliding)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, controller.rotation, 10f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }
}
