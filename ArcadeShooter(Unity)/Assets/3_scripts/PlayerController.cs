using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Vector3 velocity;
	Rigidbody rigidbody;

	void Start () 
	{
		rigidbody = GetComponent<Rigidbody> ();	
	}
	void FixedUpdate()
	{
		rigidbody.MovePosition (rigidbody.position + velocity * Time.deltaTime);
	}
	
	public void Move(Vector3 _velocity)
	{
		velocity = _velocity;
	}
		
	public void LookAtMouse(Vector3 point)
	{
		Vector3 heightVector = new Vector3 (point.x, transform.position.y, point.z);
		transform.LookAt (heightVector);
	}

	public void Jump()
	{
		rigidbody.AddForce (Vector3.up * 200);
	}

}
