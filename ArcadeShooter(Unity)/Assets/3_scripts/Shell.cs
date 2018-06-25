 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private Rigidbody rigidbody;
	public float forceMin;
	public float forceMax;

	public float lifeTime;

	void Start () 
	{	
		rigidbody = GetComponent<Rigidbody> ();
		float force = Random.Range (forceMin, forceMax);
		rigidbody.AddForce (transform.right * force);
		rigidbody.AddTorque (Random.insideUnitSphere * force);

		//Destroy (gameObject, lifeTime);
		Invoke("TurnOffPhysics", lifeTime);
	}

	void TurnOffPhysics()
	{
		GetComponent<Rigidbody> ().isKinematic = true;
		GetComponent<BoxCollider> ().enabled = false;
	}
}
