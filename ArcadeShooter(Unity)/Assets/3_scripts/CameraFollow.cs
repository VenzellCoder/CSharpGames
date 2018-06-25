using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


	public Transform target;
	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	

	void FixedUpdate () 
	{
		Transform playerCross = target.GetComponent<Player> ().cameraTarget;
		Vector3 targetposition = playerCross.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp (transform.position, targetposition, smoothSpeed);



		//transform.position = smoothedPosition;
		transform.position = smoothedPosition;



		//transform.LookAt (Vector3.Lerp(target.position, playerCross.position, 0.05f));

	}
}
