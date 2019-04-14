using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class HandCollider : MonoBehaviour
    {
        private Collider physicsCollider;
        //private GameObject touched;
        private Hand hand;
        private Hand otherHand;
        public LayerMask hoverLayerMask = -1;
        public float collisionCheckRadius = 0.05f;
        private const int ColliderArraySize = 16;
        private Collider[] overlappingColliders;

        private void Awake()
        {
            physicsCollider = GetNonTrigger();
            hand = GetComponentInParent<Hand>();
            otherHand = hand.otherHand;
            overlappingColliders = new Collider[ColliderArraySize];
        }

        protected virtual bool CollisionCandidacyCheck(Vector3 hoverPosition, float hoverRadius, ref float closestDistance, ref Interactable closestInteractable)
        {
            bool turnOffCollision = false;

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

                Interactable contacting = collider.GetComponentInParent<Interactable>();

                // Yeah, it's null, skip
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
                    if (hand.AttachedObjects[attachedIndex].attachedObject == contacting.gameObject)
                    {
                        hoveringOverAttached = true;
                        break;
                    }
                }

                BodyCollider bodyCollider = collider.GetComponentInParent<BodyCollider>();

                if (bodyCollider != null)
                    turnOffCollision = true;

                if (hoveringOverAttached)
                    turnOffCollision = true;

                // Occupied by another hand, so we can't touch it
                if (otherHand && otherHand.hoveringInteractable == contacting)
                    turnOffCollision = true;

                // Best candidate so far...
                float distance = Vector3.Distance(contacting.transform.position, hoverPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = contacting;
                    turnOffCollision = true;
                }
            }

            return turnOffCollision;
        }

        private void OnTriggerStay(Collider other)
        {
            float closestDistance = float.MaxValue;
            Interactable closestInteractable = null;

            if (CollisionCandidacyCheck(this.transform.position, collisionCheckRadius, ref closestDistance, ref closestInteractable))
            {
                //physicsCollider.isTrigger = true;
            }
            else
            {
                //physicsCollider.isTrigger = false;
            }
        }

        private Collider GetNonTrigger()
        {
            foreach (Collider col in this.GetComponents<Collider>())
            {
                if (!col.isTrigger)
                {
                    return col;
                }
            }
            return null;
        }

    }
}