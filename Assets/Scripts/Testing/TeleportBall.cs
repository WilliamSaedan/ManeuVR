using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TeleportBall : MonoBehaviour
{
    public bool matchBallVelocity = true;
    public float ballVelocityMatchRate = 1.0f;

    private Rigidbody playerBody;
    private GameObject teleportSphere = null;
    private ProsepectiveCollider prospectiveCollider;
    public Hand throwHand;
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
        //TEMPORARY
        throwHand = this.GetComponentInParent<Hand>();
    }

    // Update is called once per frame
    public void OnPress()
    {
        if (teleportSphere == null)
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
        Vector3 spawnLoc = (player.hmdTransform.transform.forward/2) + player.feetPositionGuess + Vector3.up * player.GetComponentInChildren<BodyCollider>().Height;

        teleportSphere = GameObject.Instantiate<GameObject>(Resources.Load("TeleportSphere") as GameObject);
        teleportSphere.transform.position = spawnLoc;
        prospectiveCollider = teleportSphere.GetComponent<ProsepectiveCollider>();

        Vector3 initialScale = Vector3.one * 0.01f;
        Vector3 targetScale = Vector3.one * 0.1f;

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
        Debug.Log("Hello: " + teleportSphere == null);
        if (teleportSphere != null && teleportSphere.GetComponent<Throwable>().IsThrown)
        {
            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            player.trackingOriginTransform.position = teleportSphere.transform.position;// + playerFeetOffset;
            //player.transform.position = teleportSphere.transform.position;
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
        Vector3 initialScale = Vector3.one * 0.1f;
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
