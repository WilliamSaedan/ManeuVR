using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TeleportBall : MonoBehaviour
{
    public float spawnDistance = .5f;
    public bool matchBallVelocity = true;
    public float ballVelocityMatchRate = 1.0f;

    private Rigidbody playerBody;
    private GameObject teleportSphere = null;
    private Hand throwHand;
    private Interactable interactable;
    private Player player;
    private bool spawned;

    // Start is called before the first frame update
    void OnEnable()
    {
        player = Player.instance;
        if (playerBody == null)
        {
            playerBody = player.GetComponent<Rigidbody>();
        }
        interactable = this.GetComponentInParent<Interactable>();
        //TEMPORARY
        //throwHand = this.GetComponentInParent<Hand>();
    }

    public void AssignController()
    {
        if (throwHand == null)
            throwHand = interactable.attachedToHand;
    }

    // Update is called once per frame
    public void OnPressDown()
    {
        if (!spawned)
        {
            StartCoroutine(DoSpawn());
        }
        else
        {
            StartCoroutine(Despawn());
        }
    }

    private IEnumerator DoSpawn()
    {
        Vector3 spawnLoc = (player.hmdTransform.transform.forward/(1/spawnDistance));
        RaycastHit hitInfo;
        teleportSphere = GameObject.Instantiate<GameObject>(Resources.Load("Items/TeleportBall") as GameObject);
        if (Physics.Raycast((player.hmdTransform.transform.position) + (player.hmdTransform.transform.forward / 8), spawnLoc, out hitInfo, spawnDistance))
        {
            teleportSphere.transform.position = hitInfo.point;
        }
        else
        {
            teleportSphere.transform.position = (player.hmdTransform.transform.position) + spawnLoc;
        }

        Vector3 initialScale = Vector3.one * 0.01f;
        Vector3 targetScale = Vector3.one * 1f;

        spawned = true;

        float startTime = Time.time;
        float overTime = 0.25f;
        float endTime = startTime + overTime;
        throwHand.TriggerHapticPulse(10000);
        while (Time.time < endTime)
        {
            teleportSphere.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
            yield return null;
        }
        throwHand.TriggerHapticPulse(20000);
    }

    public void Teleport()
    {
        if (teleportSphere != null && teleportSphere.GetComponent<Throwable>().IsThrown)
        {
            playerBody.transform.position = teleportSphere.transform.position;
            if (matchBallVelocity)
                playerBody.velocity = teleportSphere.GetComponent<VelocityEstimator>().GetVelocityEstimate() * ballVelocityMatchRate;
            StartCoroutine(Despawn());
        }
        else
        {
            throwHand.TriggerHapticPulse(2500);
        }

    }

    private IEnumerator Despawn()
    {
        Vector3 initialScale = Vector3.one * 1f;
        Vector3 targetScale = Vector3.one * 0.01f;

        float startTime = Time.time;
        float overTime = 0.25f;
        float endTime = startTime + overTime;

        spawned = false;

        while (Time.time < endTime)
        {
            teleportSphere.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
            yield return null;
        }
        throwHand.TriggerHapticPulse(3000);
        Destroy(teleportSphere);
    }

}
