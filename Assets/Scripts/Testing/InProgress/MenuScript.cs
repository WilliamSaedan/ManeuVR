using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MenuScript : MonoBehaviour
{
    bool menuOpen = false;

    public void onPress()
    {
        if (menuOpen)
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
        return null;
    }

    private IEnumerator DoClose()
    {
        return null;
    }
}
