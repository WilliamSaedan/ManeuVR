﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Collider dangling from the player's head
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( CapsuleCollider ) )]
	public class BodyCollider : MonoBehaviour
	{
		public Transform head;

		private CapsuleCollider[] capsuleCollider;

		//-------------------------------------------------
		void Awake()
		{
			capsuleCollider = GetComponents<CapsuleCollider>();
		}


		//-------------------------------------------------
		void FixedUpdate()
		{
			float distanceFromFloor = Vector3.Dot( head.localPosition, Vector3.up );
            foreach (CapsuleCollider collider in capsuleCollider)
            {
                collider.height = Mathf.Max((collider.radius * 2), distanceFromFloor);
            }
            transform.localPosition = new Vector3(head.localPosition.x, ((capsuleCollider[0].height / 2.0f) - capsuleCollider[0].center.y), head.localPosition.z);
        }
	}
}
