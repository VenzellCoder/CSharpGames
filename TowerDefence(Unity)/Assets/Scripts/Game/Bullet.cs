using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed;
	public int damage;
	public Transform target;

	// преследовать цель, цели нет --> destroy()
	void Update ()
	{
		if (target == null) 
		{
			Destroy (gameObject);
			return;
		}

		Vector3 direction = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		// иначе будет перелёт 
		if (direction.magnitude <= distanceThisFrame) 
		{
			HitEnemy ();
			return;
		}
		transform.Translate (direction.normalized * speed * Time.deltaTime, Space.World);
	}


	void HitEnemy()
	{
		target.GetComponent<Enemy> ().TakeDamage (damage);
		Destroy (gameObject);
	}
}
