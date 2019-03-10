using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Rigidbody elevator;
	private bool rising;
	public float rate;
	public float max;
	
    // Start is called before the first frame update
    void Awake()
    {
        elevator = GetComponent<Rigidbody>();
		rising = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (rising){
			elevator.velocity = new Vector3(0.0f,rate,0.0f);
			if (this.transform.position.y >= max - this.transform.localScale.y/2){
				rising = false;
			}
		}
		if (!rising){
			elevator.velocity = new Vector3(0.0f,-rate,0.0f);
			if (this.transform.position.y <= this.transform.localScale.y/1.99f){
				rising = true;
			}
		}
    }

}
