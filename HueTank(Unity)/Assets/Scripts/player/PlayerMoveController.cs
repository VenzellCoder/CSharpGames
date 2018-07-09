using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour {

	float rotation;
	Vector3 movement;
	Rigidbody rigidbody;

	void Start () 
	{
		rigidbody = GetComponent<Rigidbody> ();	
	}

	void FixedUpdate()
	{
		// Движение 
		rigidbody.MovePosition (rigidbody.position + movement * Time.deltaTime);

		// Поворот
		Quaternion turnRotation = Quaternion.Euler (0f, rotation * Time.deltaTime, 0f);
		rigidbody.MoveRotation (rigidbody.rotation * turnRotation);
	}
		
	public void Move(Vector3 _movement)
	{
		movement = _movement;
	}

	public void Rotate (float _rotation)
	{
		rotation = _rotation;
	}
}
