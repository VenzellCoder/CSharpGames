using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private float bulletLifeTimeMax = 3f;
	private float bulletLifeTime;

	[HideInInspector]
	public float speed;
	[HideInInspector]
	public float damage;
	public LayerMask collisionLayerMask;
	[HideInInspector]
	public  float deltaY;
	[HideInInspector]
	public float deltaZ;
	[HideInInspector]
	public bool flyThroughEnemy;

	public GameObject hitObstacleEffect;
	public AudioClip hitObstacleSound;


	void Update () 
	{
		// уничтожение пуль по таймеру (для улетевших за границу уровня)
		bulletLifeTime += Time.deltaTime;
		if (bulletLifeTime > bulletLifeTimeMax) 
		{
			Destroy (gameObject);
		}

		// движение пули
		float moveDistance = speed * Time.deltaTime;
		CheckCollision (moveDistance);
		transform.Translate (Vector3.forward * moveDistance);

		// разброс пуль
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
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, moveDistance)) 
		{
			OnEnemyHit (hit, ray);
			OnObstacleHit (hit, ray);
		}
	}

	// обработка столкновений с врагами
	void OnEnemyHit(RaycastHit hit, Ray ray)
	{
		IDamagable damagableObject = hit.collider.GetComponent<IDamagable> ();
		if (damagableObject != null) 
		{
			damagableObject.TakeHit (damage, hit.point, ray.direction);

			if (!flyThroughEnemy) 
			{
				Destroy (gameObject);
			}
		}

	}

	// обработка столкновений с препятствиями
	void OnObstacleHit(RaycastHit hit, Ray ray)
	{
		if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Obstacles")) 
		{
			Destroy (Instantiate (hitObstacleEffect, hit.point, transform.rotation) as GameObject, 3f);
			SoundManager.instance.PlaySound (hitObstacleSound, transform.position);
			Destroy (gameObject);
		}
	}
}
