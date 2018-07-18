using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (new Vector3 (500f, 0f, 500f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
