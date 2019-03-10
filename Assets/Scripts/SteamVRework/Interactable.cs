using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Interactable : MonoBehaviour
{
	[Tooltip("Activates an action set on attach and deactivates on detach")]
    public SteamVR_ActionSet activateActionSetOnAttach;
	
	[Tooltip("Hide the controller part of the hand on attachment and show on detach")]
    public bool hideControllerOnAttach = false;
	
	public delegate void OnAttachedToHandDelegate( Hand hand );
	public delegate void OnDetachedFromHandDelegate( Hand hand );
	
	[HideInInspector]
	public event OnAttachedToHandDelegate onAttachedToHand;
	[HideInInspector]
	public event OnDetachedFromHandDelegate onDetachedFromHand;

    [System.NonSerialized]
    public Hand attachedToHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnAttachedToHand( Hand hand )
    {
        if (activateActionSetOnAttach != null)
            activateActionSetOnAttach.ActivatePrimary();

        if ( onAttachedToHand != null )
		{
			onAttachedToHand.Invoke( hand );
		}

        attachedToHand = hand;
    }
}
