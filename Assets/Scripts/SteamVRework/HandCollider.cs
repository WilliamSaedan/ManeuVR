using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class HandCollider : MonoBehaviour
    {
        private BoxCollider collider;
        private GameObject touched;
        private Hand hand;

        private void Awake()
        {
            collider = GetNonTrigger();
            hand = GetComponentInParent<Hand>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Interactable>() != null || other.GetComponentInParent<Interactable>() != null)
            {
                touched = other.gameObject;
                collider.isTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((other.GetComponent<Interactable>() != null || other.GetComponentInParent<Interactable>() != null) && other.gameObject == touched && hand.AttachedObjects.Count < 1)
            {
                collider.isTrigger = false;
            }
        }

        private BoxCollider GetNonTrigger()
        {
            foreach (BoxCollider col in GetComponents<BoxCollider>())
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