using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed;
	public float damage;
	public LayerMask collisionLayerMask;

	public  float deltaY;
	public float deltaZ;

	public GameObject hitObstacleEffect;

	void Start()
	{
		Destroy (gameObject, 5f);
	}

	void Update () 
	{
		float moveDistance = speed * Time.deltaTime;
		CheckCollision (moveDistance);
		transform.Translate (Vector3.forward * moveDistance);

		if (deltaY != 0)
		{
			transform.Translate (Vector3.up * Time.deltaTime * deltaY);
		}
		if (deltaZ !=0)
		{
			transform.Translate (Vector3.right * Time.deltaTime * deltaZ);
		}

	}

	void CheckCollision(float moveDistance)
	{
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance))
		{
			OnEnemyHit (hit, ray);

			if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Obstacles")) 
			{
				//Instantiate (hitObstacleEffect, hit.point, transform.rotation);
				Destroy(Instantiate(hitObstacleEffect, hit.point, transform.rotation) as GameObject, 3f);
			}
		}

	}

	void OnEnemyHit(RaycastHit hit, Ray ray)
	{
		IDamagable damagableObject = hit.collider.GetComponent<IDamagable> ();
		if (damagableObject != null) 
		{
			damagableObject.TakeHit (damage, hit.point, ray.direction);
		}
		Destroy (gameObject);
	}
}
