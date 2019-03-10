using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AlternativeController : MonoBehaviour
{
    /*[EnumFlags]
    [Tooltip("The flags used to attach this object to the hand.")]
    public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

    [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
    public Transform attachmentOffset;

    [Tooltip("The grabbed object will stay attached until a grab action is finished")]
    public bool detachOnGrabLeave = true;

    [Tooltip("Drop the object with this action (If null, object will be dropped on grab end)")]
    public SteamVR_Action_Boolean dropAction;

    [Tooltip("How fast must this object be moving to attach due to a trigger hold instead of a trigger press? (-1 to disable)")]
    public float catchingSpeedThreshold = -1;

    public ReleaseStyle releaseVelocityStyle = ReleaseStyle.GetFromHand;

    [Tooltip("The time offset used when releasing the object with the RawFromHand option")]
    public float releaseVelocityTimeOffset = -0.011f;

    public float scaleReleaseVelocity = 1.1f;

    [Tooltip("When detaching the object, should it return to its original parent?")]
    public bool restoreOriginalParent = false;

    public bool attachEaseIn = false;
    public AnimationCurve snapAttachEaseInCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    public float snapAttachEaseInTime = 0.15f;

    protected VelocityEstimator velocityEstimator;
    protected bool attached = false;
    protected float attachTime;
    protected Vector3 attachPosition;
    protected Quaternion attachRotation;
    protected Transform attachEaseInTransform;

    public UnityEvent onPickUp;
    public UnityEvent onDetachFromHand;

    public bool snapAttachEaseInCompleted = false;

    protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;

    protected new Rigidbody rigidbody;
    [HideInInspector]
    public Interactable interactable;
    [HideInInspector]
    public bool IsThrown { get; private set; }



    //-------------------------------------------------
    protected virtual void Awake()
    {
        velocityEstimator = GetComponent<VelocityEstimator>();
        interactable = GetComponent<Interactable>();

        if (attachEaseIn)
        {
            attachmentFlags &= ~Hand.AttachmentFlags.SnapOnAttach;
        }

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 50.0f;


        if (attachmentOffset != null)
        {
            interactable.handFollowTransform = attachmentOffset;
        }

    }


    //-------------------------------------------------
    protected virtual void OnHandHoverBegin(Hand hand)
    {
        bool showHint = false;

        // "Catch" the throwable by holding down the interaction button instead of pressing it.
        // Only do this if the throwable is moving faster than the prescribed threshold speed,
        // and if it isn't attached to another hand
        if (!attached && catchingSpeedThreshold != -1)
        {
            float catchingThreshold = catchingSpeedThreshold * SteamVR_Utils.GetLossyScale(Player.instance.trackingOriginTransform);

            GrabTypes bestGrabType = hand.GetBestGrabbingType();

            if (bestGrabType != GrabTypes.None)
            {
                if (rigidbody.velocity.magnitude >= catchingThreshold)
                {
                    hand.AttachObject(gameObject, bestGrabType, attachmentFlags);
                    showHint = false;
                }
            }
        }

        if (showHint)
        {
            hand.ShowGrabHint();
        }
    }


    //-------------------------------------------------
    protected virtual void OnHandHoverEnd(Hand hand)
    {
        hand.HideGrabHint();
    }


    //-------------------------------------------------
    protected virtual void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (startingGrabType != GrabTypes.None)
        {
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
            hand.HideGrabHint();
        }
    }

    //-------------------------------------------------
    protected virtual void OnAttachedToHand(Hand hand)
    {
        //Debug.Log("Pickup: " + hand.GetGrabStarting().ToString());

        hadInterpolation = this.rigidbody.interpolation;

        attached = true;

        onPickUp.Invoke();

        hand.HoverLock(null);

        rigidbody.interpolation = RigidbodyInterpolation.None;

        velocityEstimator.BeginEstimatingVelocity();

        attachTime = Time.time;
        attachPosition = transform.position;
        attachRotation = transform.rotation;

        if (attachEaseIn)
        {
            attachEaseInTransform = hand.objectAttachmentPoint;
        }

        snapAttachEaseInCompleted = false;
    }


    //-------------------------------------------------
    protected virtual void OnDetachedFromHand(Hand hand)
    {
        attached = false;

        IsThrown = true;

        onDetachFromHand.Invoke();

        hand.HoverUnlock(null);

        rigidbody.interpolation = hadInterpolation;

        Vector3 velocity;
        Vector3 angularVelocity;

        GetReleaseVelocities(hand, out velocity, out angularVelocity);

        rigidbody.velocity = velocity;
        rigidbody.angularVelocity = angularVelocity;
    }*/
}
