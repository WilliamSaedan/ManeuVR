using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerDebug : MonoBehaviour
{
    // variables for: instancing //
    public static PlayerDebug singleton;       // connection - automatic: the singleton instance of this class

    // variables for: velocity determination //
    private new Rigidbody rigidbody;		// connection - automatic: the player's rigidbody (attached)

    protected VelocityEstimator velocityEstimator;

    // method: determine the current velocity of the player //
    public Vector3 velocity_()
    {
        return rigidbody.velocity;
    }
    // method: determine the current velocity of the player //
    public static Vector3 velocity()
    {
        return singleton.velocity_();
    }
    // method: determine the current x axis of velocity of the player //
    public float velocityX_()
    {
        return rigidbody.velocity.x;
    }
    // method: determine the current x axis of velocity of the player //
    public static float velocityX()
    {
        return singleton.velocityX_();
    }
    // method: determine the current y axis of velocity of the player //
    public float velocityY_()
    {
        return rigidbody.velocity.y;
    }
    // method: determine the current y axis of velocity of the player //
    public static float velocityY()
    {
        return singleton.velocityY_();
    }
    // method: determine the current z axis of velocity of the player //
    public float velocityZ_()
    {
        return rigidbody.velocity.z;
    }
    // method: determine the current z axis of velocity of the player //
    public static float velocityZ()
    {
        return singleton.velocityZ_();
    }
    // method: determine the current speed of the player //
    public float speed_()
    {
        return velocity_().magnitude;
    }
    // method: determine the current speed of the player //
    public static float speed()
    {
        return singleton.speed_();
    }
    // method: determine the current x axis of speed of the player //
    public float speedX_()
    {
        return Mathf.Abs(velocityX_());
    }
    // method: determine the current x axis of speed of the player //
    public static float speedX()
    {
        return singleton.speedX_();
    }
    // method: determine the current y axis of speed of the player //
    public float speedY_()
    {
        return Mathf.Abs(velocityY_());
    }
    // method: determine the current y axis of speed of the player //
    public static float speedY()
    {
        return singleton.speedY_();
    }
    // method: determine the current z axis of speed of the player //
    public float speedZ_()
    {
        return Mathf.Abs(velocityZ_());
    }
    // method: determine the current z axis of speed of the player //
    public static float speedZ()
    {
        return singleton.speedZ_();
    }

    private void Awake()
    {
        // connect to the singleton instance of this class //
        singleton = this;

        // connect to the player's rigidbody //
        rigidbody = GetComponent<Rigidbody>();
        velocityEstimator = GetComponent<VelocityEstimator>();
    }

    private void Update()
    {
        //Debug.Log(velocityEstimator.GetVelocityEstimate());
    }
}
