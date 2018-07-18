using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSolo : Unit {


	public float moveSpeed = 2.5f;
	PlayerMoveController playerController;
	GunController gunController;
	Animator animator;
	Rigidbody rigidbody;
	Transform camera;

	private float inputH;
	private float inputV;


	public Transform cross;
	public Transform cameraTarget;
	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	void Start () 
	{
		Cursor.visible = false;

		animator = GetComponent<Animator> ();
		playerController = GetComponent<PlayerMoveController> ();
		gunController = GetComponent<GunController> ();
		rigidbody = GetComponent<Rigidbody> ();
		camera = Camera.main.transform;
	}
	

	void Update () 
	{
		// Ввод движение
		inputH = Input.GetAxisRaw ("Horizontal");
		inputV = Input.GetAxisRaw ("Vertical");

		if (inputH == 0f && inputV == 0f) 
		{
			animator.SetBool ("run", false);
		} 
		else 
		{
			animator.SetBool ("run", true);
		}

		Vector3 moveInput = new Vector3 (inputH, 0f, inputV);
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;

		playerController.Move (moveVelocity);

		// Ввод взгляд 
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane plane = new Plane (Vector3.up, Vector3.up*1f);
		float rayDist;
		if (plane.Raycast (ray, out rayDist)) 
		{
			Vector3 point = ray.GetPoint (rayDist);
			playerController.LookAtMouse (point);
			cross.position = point;
			cameraTarget.position = Vector3.Lerp (transform.position, cross.position, 0.1f);

		}

		// Ввод прыжок
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			playerController.Jump ();
		}

		// Ввод стрельба

		if (Input.GetMouseButton(0))
		{
			animator.Play ("IDLE_SHOOT");
			//gunController.CmdShoot();
		}


	}

	/*
	void Boom(Vector3 explosionPos)
	{
		Debug.Log ("BOOOOM!!!!");

		float radius = 5F;
		float power = 1000F;

		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponent<Rigidbody>();

			if (rb != null)
				rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
		}
	}
	*/

	void FixedUpdate () 
	{
		Vector3 targetposition = cameraTarget.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp (transform.position, targetposition, smoothSpeed);
		camera.transform.position = targetposition;
	}

}
