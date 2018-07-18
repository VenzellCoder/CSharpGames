using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_old : MonoBehaviour {

	public Animator animator;
	public Rigidbody rigidbody;

	private float inputH;
	private float inputV;

	void Start () 
	{
		animator = GetComponent<Animator> ();
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		if (Input.GetKeyDown ("1"))  
		{
			animator.Play ("RUN", -1, 0f); 
		}

		inputH = Input.GetAxisRaw ("Horizontal");
		inputV = Input.GetAxisRaw ("Vertical");



		animator.SetFloat ("inputV", inputV);
		animator.SetFloat ("inputH", inputH);
	}

	private void FixedUpdate()
	{
		Move ();
		Turn ();
	}


	private void Move()
	{
		Vector3 movement = transform.forward * Input.GetAxisRaw("Vertical") * 3f * Time.deltaTime;
		rigidbody.MovePosition (rigidbody.position + movement);
	}


	private void Turn()
	{
		// Adjust the rotation of the tank based on the player's input.
		float turn = Input.GetAxisRaw("Horizontal") * 180f * Time.deltaTime;

		Quaternion rotation = Quaternion.Euler (0f, turn, 0f);
		rigidbody.MoveRotation (rigidbody.rotation * rotation);
	}
}
