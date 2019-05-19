using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MenuScript : MonoBehaviour
{
    bool menuOpen = false;
    bool secondClick = false;

    private Player player;
    private Rigidbody playerBody;
    private Hand hand;



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
        StartCoroutine(DoubleClick());
        if (secondClick) {
            if (menuOpen)
            {
                StartCoroutine(DoOpen());
            }
            else
            {
                StartCoroutine(DoClose());
            }
        }
    }

    private IEnumerator DoOpen()
    {

        return null;
    }

    private IEnumerator DoClose()
    {

        return null;
    }

    private IEnumerator DoubleClick()
    {
        secondClick = true;
        yield return new WaitForSeconds(.5f);
        secondClick = false;
    }
}
