using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerCameraControl : NetworkManager {

	[Header("Camera Proporties")]
	public Transform sceneCamera;
	public float cameraRotationRadius = 25f;
	public float cameraRotationSpeed = 3f;
	public bool cameraIsRotating = true;

	float rotation; 

	public override void OnStartClient(NetworkClient client)
	{
		cameraIsRotating = false;
	}

	public override void OnStartHost()
	{
		cameraIsRotating = false;
	}

	public override void OnStopClient()
	{
		cameraIsRotating = true;
	}

	public override void OnStopHost()
	{
		cameraIsRotating = true;
	}

	void Update()
	{
		if (!cameraIsRotating) 
		{
			return;
		}

		rotation += cameraRotationSpeed * Time.deltaTime;
		if (rotation >= 360) 
		{
			rotation -= 360;
		}

		sceneCamera.position = Vector3.zero;
		sceneCamera.rotation = Quaternion.Euler (0f, rotation, 0f);
		sceneCamera.Translate (0f, cameraRotationRadius, -cameraRotationRadius);
		sceneCamera.LookAt (Vector3.zero);
	}
}
