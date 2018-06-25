using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

	public Color liveColour;
	public Color hitColour;

	public float moveSpeed = 2.5f;
	PlayerController playerController;
	GunController gunController;
	Animator animator;
	Rigidbody rigidbody;

	private float inputH;
	private float inputV;



	public Transform materialHolderBody;
	public Transform materialHolderHead;

	public Transform cross;
	public Transform cameraTarget;

	public override void Start () 
	{
		Cursor.visible = false;

		base.Start ();

		animator = GetComponent<Animator> ();
		playerController = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();
		rigidbody = GetComponent<Rigidbody> ();
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
		Plane plane = new Plane (Vector3.up, Vector3.up*gunController.GetGunHeight());
		float rayDist;
		if (plane.Raycast (ray, out rayDist)) 
		{
			Vector3 point = ray.GetPoint (rayDist);
			playerController.LookAtMouse (point);
			cross.position = point;
			cameraTarget.position = Vector3.Lerp (transform.position, cross.position, 0.1f);


			if (Input.GetMouseButton(1))
			{
				Boom (point);
			}
		}

		// Ввод прыжок
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			playerController.Jump ();
		}

		// Ввод стрельба
		if (Input.GetMouseButton(0))
		{
			gunController.Shoot();
		}



	}

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

	public void PlayerHit()
	{
		materialHolderBody.GetComponent<Renderer> ().material.color = hitColour;
		materialHolderHead.GetComponent<Renderer> ().material.color = hitColour;
		Invoke("RecoverAfterHit", 0.2f);
	}

	public void RecoverAfterHit()
	{
		materialHolderBody.GetComponent<Renderer> ().material.color = liveColour;
		materialHolderHead.GetComponent<Renderer> ().material.color = liveColour;
	}
}
