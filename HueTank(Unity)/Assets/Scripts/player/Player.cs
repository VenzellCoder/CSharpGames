using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

	[Header("Movement")]
	public float moveSpeed = 10f;
	public float rotationSpeed = 100f;

	[Header("Camera")]
	// родитель камеры (именно он следует за игроком)
	public Transform cameraHolder;
	// камера смотрит не на танк
	public Transform cameraTarget;
	// расстояние между танком и целью камеры
	public float cameraTargetOffset;
	// расстояние между камерой и целью
	public Vector3 cameraOffset;
	// скорость сглаживания движения камеры
	public float smoothCameraSpeed = 0.1f;

	[Header("Effects")]
	public Transform trailEffect;
	public Transform fireEffect;

	PlayerMoveController playerController;
	GunController gunController;
	Rigidbody rigidbody;

	private float inputMove;
	private float inputRotate;

	public override void Start () 
	{
		Cursor.visible = false;

		base.Start ();

		GetComponent<GUIManager> ().UpdateHpGUI (hpStart, hp);

		playerController = GetComponent<PlayerMoveController> ();
		gunController = GetComponent<GunController> ();
		rigidbody = GetComponent<Rigidbody> ();
	}
	

	public override void Update () 
	{
		// Ввод движения
		inputMove = Input.GetAxis ("Vertical");
		inputRotate = Input.GetAxis ("Horizontal");

		Vector3 movement = inputMove * transform.forward * moveSpeed;
		float rotation = inputRotate * rotationSpeed;

		playerController.Move (movement);
		playerController.Rotate (rotation);

		// Ввод стрельба
		if (Input.GetKey(KeyCode.X))
		{
			gunController.Shoot();
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			gunController.ChangeWeapon(-1);
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			gunController.ChangeWeapon(+1);
		}

		// цель камеры находится перед танком по направлению его движения
		Vector3 cameraTargetVector = transform.forward * cameraTargetOffset;
		cameraTarget.position = transform.position + cameraTargetVector;
	}


	void FixedUpdate () 
	{
		Vector3 targetPosition = cameraTarget.position + cameraOffset;
		// сглаживание позиции камеры
		Vector3 smoothPosition = Vector3.Lerp (cameraHolder.transform.position, targetPosition, smoothCameraSpeed);
		// камера не выходит на границы уровня
		Vector3 clampedPosition = new Vector3 (Mathf.Clamp (smoothPosition.x, -14f, 14f),
												smoothPosition.y,
												Mathf.Clamp (smoothPosition.z, -36f, 12f));

		cameraHolder.transform.position = clampedPosition;

	}


	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		base.TakeHit (damage, hitPoint, hitDirection);
		// Менять цвета при ранении игрока
		GameManager.instance.OnPlayerHit ();
		// обновить hp GUI
		GetComponent<GUIManager> ().UpdateHpGUI (hpStart, hp);
		// Скриншейк
		StartCoroutine(Camera.main.GetComponent<ScreenShaker>().Shake(0.1f, 0.5f));
	}

	public override void Die()
	{
		base.Die ();

		fireEffect.gameObject.SetActive (true);
		// отключить управление
		this.enabled = false;
		playerController.enabled = false;
		gunController.enabled = false;
		// Изменить GUI
		GetComponent<GUIManager> ().GameOverGUI ();
		// изменить режим игры
		GameManager.instance.GameOver ();
	}

}
