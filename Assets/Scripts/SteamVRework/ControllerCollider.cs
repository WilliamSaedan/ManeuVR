using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ControllerCollider : MonoBehaviour
{
    private List<Collider> physicsCollider;
    //private GameObject touched;
    private Hand hand;
    private Hand otherHand;
    public LayerMask hoverLayerMask = -1;
    public float collisionCheckRadius = 0.05f;
    private const int ColliderArraySize = 16;
    private Collider[] overlappingColliders;

    public bool controllerCollision { get; private set; }

    private void Awake()
    {
        physicsCollider = GetNonTrigger();
        hand = Player.instance.rightHand;
        otherHand = hand.otherHand;
        controllerCollision = false;
        overlappingColliders = new Collider[ColliderArraySize];
    }

    protected virtual bool CollisionCandidacyCheck(Vector3 hoverPosition, float hoverRadius)
    {
        bool turnOffCollision = false;

        controllerCollision = false;

        // null out old vals
        for (int i = 0; i < overlappingColliders.Length; ++i)
        {
            overlappingColliders[i] = null;
        }

        int numColliding = Physics.OverlapSphereNonAlloc(hoverPosition, hoverRadius, overlappingColliders, hoverLayerMask.value);

        if (numColliding == ColliderArraySize)
            Debug.LogWarning("This hand is overlapping the max number of colliders: " + ColliderArraySize + ". Some collisions may be missed. Increase ColliderArraySize on Hand.cs");


        // Pick the closest hovering
        for (int colliderIndex = 0; colliderIndex < overlappingColliders.Length; colliderIndex++)
        {
            Collider collider = overlappingColliders[colliderIndex];

            if (collider == null)
                continue;

            if (collider.GetComponentInParent<AlternativeController>() != null)
            {
                controllerCollision = true;
            }

            if (collider.tag == "PlayerCollider")
            {
                continue;
            }

            //Debug.Log(collider);
            Interactable contacting = collider.GetComponentInParent<Interactable>();
            // Yeah, it's null, skip
            //Debug.Log(contacting);
            if (contacting == null)
                continue;

            // Ignore this collider for hovering
            IgnoreHovering ignore = collider.GetComponent<IgnoreHovering>();
            if (ignore != null)
            {
                if (ignore.onlyIgnoreHand == null || ignore.onlyIgnoreHand == this)
                {
                    continue;
                }
            }

            // Can't hover over the object if it's attached
            bool hoveringOverAttached = false;
            for (int attachedIndex = 0; attachedIndex < hand.AttachedObjects.Count; attachedIndex++)
            {
                //Debug.Log(hand.AttachedObjects[attachedIndex].attachedObject);
                //Debug.Log(contacting.gameObject);

                if (hand.AttachedObjects[attachedIndex].attachedObject == contacting.gameObject)
                {
                    hoveringOverAttached = true;
                    break;
                }
            }

            if (hoveringOverAttached)
                turnOffCollision = true;

            BodyCollider bodyCollider = collider.GetComponentInParent<BodyCollider>();

            if (bodyCollider != null)
                turnOffCollision = true;

            // Occupied by another hand, so we can't touch it
            if (otherHand && otherHand.hoveringInteractable == contacting)
                turnOffCollision = true;

            // Best candidate so far...
            /*float distance = Vector3.Distance(contacting.transform.position, hoverPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = contacting;
                turnOffCollision = true;
            }*/
        }

        return turnOffCollision;
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (CollisionCandidacyCheck(this.transform.position, collisionCheckRadius))
        {
            foreach (Collider collider in physicsCollider) {
                collider.isTrigger = true;
            }
            //Debug.Log(other);
        }
        else
        {
            foreach (Collider collider in physicsCollider)
            {
                collider.isTrigger = false;
            }
        }

    }

    private List<Collider> GetNonTrigger()
    {
        List<Collider> colliderArray = new List<Collider>();
        foreach (Collider collider in this.GetComponents<Collider>())
        {
            if (!collider.isTrigger)
            {
                colliderArray.Add(collider);
            }
        }
        return colliderArray;
    }

    public bool IsCollidingWithController()
    {
        CollisionCandidacyCheck(this.transform.position, collisionCheckRadius);
        return controllerCollision;
    }
}
