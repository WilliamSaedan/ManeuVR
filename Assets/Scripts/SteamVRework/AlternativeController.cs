using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    
    public class AlternativeController : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

        [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
        public Transform attachmentOffset;

        [Tooltip("The grabbed object will stay attached until a grab action is finished")]
        public bool detachOnGrabLeave = false;

        [Tooltip("Drop the object with this action (If null, object will be dropped on grab end)")]
        public SteamVR_Action_Boolean dropAction;

        public bool denyGrabOnPickup = false;

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;

        public bool attachEaseIn = false;
        public AnimationCurve snapAttachEaseInCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
        public float snapAttachEaseInTime = 0.15f;

        protected bool attached = false;
        protected float attachTime;
        protected Vector3 attachPosition;
        protected Quaternion attachRotation;
        protected Transform attachEaseInTransform;

        public UnityEvent onPickUp;
        public UnityEvent onDetachFromHand;

        public bool snapAttachEaseInCompleted = false;

        //protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;

        //protected new Rigidbody rigidbody;
        [HideInInspector]
        public Interactable interactable;
        [HideInInspector]
        public bool IsAttached { get; private set; }



        //-------------------------------------------------
        protected virtual void Awake()
        {
            interactable = GetComponent<Interactable>();

            if (attachEaseIn)
            {
                attachmentFlags &= ~Hand.AttachmentFlags.SnapOnAttach;
            }

            //rigidbody = GetComponent<Rigidbody>();
            //rigidbody.maxAngularVelocity = 50.0f;


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
            if (!attached)
            {
                GrabTypes bestGrabType = hand.GetBestGrabbingType();

                if (bestGrabType != GrabTypes.None)
                {
                    hand.AttachObject(gameObject, bestGrabType, attachmentFlags);
                    showHint = false;
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

            //hadInterpolation = this.rigidbody.interpolation;

            attached = true;

            IsAttached = true;

            onPickUp.Invoke();

            if (denyGrabOnPickup)
            {
                hand.HoverLock(null);
            }

            //rigidbody.interpolation = RigidbodyInterpolation.None;


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

            IsAttached = false;

            onDetachFromHand.Invoke();

            if (denyGrabOnPickup)
            {
                hand.HoverUnlock(null);
            }

            //rigidbody.interpolation = hadInterpolation;

            //Vector3 velocity;
            //Vector3 angularVelocity;

            //GetReleaseVelocities(hand, out velocity, out angularVelocity);

            //rigidbody.velocity = velocity;
            //rigidbody.angularVelocity = angularVelocity;
        }

        protected virtual void HandAttachedUpdate(Hand hand)
        {
            if (attachEaseIn)
            {
                float t = Util.RemapNumberClamped(Time.time, attachTime, attachTime + snapAttachEaseInTime, 0.0f, 1.0f);
                if (t < 1.0f)
                {
                    t = snapAttachEaseInCurve.Evaluate(t);
                    transform.position = Vector3.Lerp(attachPosition, attachEaseInTransform.position, t);
                    transform.rotation = Quaternion.Lerp(attachRotation, attachEaseInTransform.rotation, t);
                }
                else if (!snapAttachEaseInCompleted)
                {
                    gameObject.SendMessage("OnThrowableAttachEaseInCompleted", hand, SendMessageOptions.DontRequireReceiver);
                    snapAttachEaseInCompleted = true;
                }
            }

            if (hand.IsGrabEnding(this.gameObject) && detachOnGrabLeave)
            {
                hand.DetachObject(gameObject, restoreOriginalParent);

                // Uncomment to detach ourselves late in the frame.
                // This is so that any vehicles the player is attached to
                // have a chance to finish updating themselves.
                // If we detach now, our position could be behind what it
                // will be at the end of the frame, and the object may appear
                // to teleport behind the hand when the player releases it.
                //StartCoroutine( LateDetach( hand ) );
            }
            else if (dropAction != null && dropAction.GetStateDown(interactable.attachedToHand.handType))
            {
                hand.DetachObject(gameObject, restoreOriginalParent);
            }
        }
    }
}