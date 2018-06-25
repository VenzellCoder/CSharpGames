using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable {
	
	public float hpStart; 

	protected float hp;
	protected bool isDead;


	public virtual void Start()
	{
		hp = hpStart;
	}

	public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
	{

		hp -= damage;

		if (hp <= 0 && !isDead) 
		{
			Die ();
		}
	}

	public virtual void Die()
	{
		isDead = true;
		Destroy (gameObject);
	}
}
