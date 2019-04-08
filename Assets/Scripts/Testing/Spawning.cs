using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Spawning : MonoBehaviour {

    public SteamVR_Action_Boolean spawnAction;

    public Hand hand;

    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();

        spawnAction.AddOnChangeListener(OnSpawnAction, hand.handType);
    }

    private void OnDisable()
    {
        if (spawnAction != null)
            spawnAction.RemoveOnChangeListener(OnSpawnAction, hand.handType);
    }

    private void OnSpawnAction(SteamVR_Action_In actionIn)
    {
        if (spawnAction.GetStateDown(hand.handType))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        StartCoroutine(DoSpawn());
    }

    private IEnumerator DoSpawn()
    {
        Vector3 spawnLoc = hand.transform.position;

        GameObject spawning = GameObject.Instantiate<GameObject>(Resources.Load("Throwable1") as GameObject);
        spawning.transform.position = spawnLoc;
        spawning.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

        Vector3 initialScale = Vector3.one * 0.01f;
        Vector3 targetScale = Vector3.one * (1f + Random.value * 0.1f);

        float startTime = Time.time;
        float overTime = 0.25f;
        float endTime = startTime + overTime;

        while (Time.time < endTime)
        {
            spawning.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
            yield return null;
        }
    }
}
