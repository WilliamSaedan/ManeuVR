using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ProsepectiveCollider : MonoBehaviour
{
    private bool collision;

    private Player player;
    private Transform head;

    private CapsuleCollider collider;
    // Start is called before the first frame update
    void Awake()
    {
        player = Player.instance;
        head = Player.instance.transform;
        collider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceFromFloor = Vector3.Dot(head.localPosition, Vector3.up);
        collider.height = Mathf.Max((collider.radius * 2), distanceFromFloor);
        transform.localPosition = Vector3.up * ((collider.height / 2.0f) - collider.center.y);
        transform.rotation = Quaternion.Inverse(transform.parent.gameObject.transform.rotation);
    }

    public bool isColliding()
    {
        return collision;
    }

    private void OnTriggerEnter(Collider other)
    {
        collision = true;
    }

    private void OnTriggerExit(Collider other)
    {
        collision = false;
    }
}
