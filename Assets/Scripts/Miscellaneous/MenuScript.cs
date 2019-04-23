using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MenuScript : MonoBehaviour
{
    bool menuOpen = false;

    private Player player;
    private Rigidbody playerBody;
    private Hand hand;
    private GameObject menu;
    private Rigidbody[] columnBody;

    void OnEnable()
    {
        player = Player.instance;
        if (playerBody == null)
        {
            playerBody = player.GetComponent<Rigidbody>();
        }
    }

    public void onPressDown()
    {
        if (!menuOpen)
        {
            StartCoroutine(DoOpen());
        }
        else
        {
            StartCoroutine(DoClose());
        }
    }

    private IEnumerator DoOpen()
    {
        menu = GameObject.Instantiate<GameObject>(Resources.Load("Items/Menu/Menu") as GameObject);
        if (!Physics.Raycast(player.transform.position + player.transform.up / 8, player.transform.position + player.transform.up))
        {
            menu.transform.position = player.hmdTransform.transform.position + Vector3.up * 10;
            menu.transform.rotation = Quaternion.Euler(0f, player.hmdTransform.transform.rotation.y * 180f, 0f);
            Debug.Log(player.hmdTransform.transform.rotation.y);
        }
        else
        {
            menu.transform.position = player.hmdTransform.transform.position + Vector3.up * -2;
            menu.transform.rotation = Quaternion.Euler(0f, player.hmdTransform.transform.rotation.y * 180f, 0f);

            Vector3 initialPosition = menu.transform.position;
            Vector3 targetPosition = player.hmdTransform.transform.position;

            columnBody = menu.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody body in columnBody)
            {
                body.isKinematic = true;
            }

            float startTime = Time.time;
            float overTime = 1f;
            float endTime = startTime + overTime;

            while (Time.time < endTime)
            {
                menu.transform.position = Vector3.Slerp(initialPosition, targetPosition, (Time.time - startTime) / overTime);
                //menu.transform.localScale = Vector3.Slerp(menu.transform.localScale*0.01f, menu.transform.localScale, (Time.time - startTime) / overTime);
                yield return null;
            }

            foreach (Rigidbody body in columnBody)
            {
                body.isKinematic = false;
            }
        }

        menuOpen = true;
    }

    private IEnumerator DoClose()
    {
        columnBody = menu.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody body in columnBody)
        {
            body.isKinematic = true;
        }

        Vector3 initialPosition = menu.transform.position;
        Vector3 targetPosition = menu.transform.position - Vector3.up*2;

        float startTime = Time.time;
        float overTime = 1f;
        float endTime = startTime + overTime;

        while (Time.time < endTime)
        {
            menu.transform.position = Vector3.Slerp(initialPosition, targetPosition, (Time.time - startTime) / overTime);
            //menu.transform.localScale = Vector3.Slerp(menu.transform.localScale, menu.transform.localScale*0.01f, (Time.time - startTime) / overTime);
            yield return null;
        }

        Destroy(menu);
        menuOpen = false;
    }

}
