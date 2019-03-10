using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Attach : MonoBehaviour
{
    public Hand hand;
    public GameObject objectToAttach;

    void Awake()
    {
        if (objectToAttach.tag != "GameController")
        {
            hand.AttachObject(objectToAttach, GrabTypes.None);
            Debug.Log("tttt");
        }
        else
        {
            StartCoroutine("attachLate");
        }
        //StartCoroutine("attachLate");
        //hand.AttachObject(objectToAttach, GrabTypes.None);
        Debug.Log("tttt");
        Debug.Log(hand.AttachedObjects.Count);
    }

    protected virtual IEnumerator attachLate()
    {
        yield return new WaitForSeconds(1);

        hand.AttachObject(objectToAttach, GrabTypes.None);
        Debug.Log("tttt");
        Debug.Log(hand.AttachedObjects.Count);
    }

}
