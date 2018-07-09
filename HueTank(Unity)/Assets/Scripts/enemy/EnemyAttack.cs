using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// поведение: Атака игрока. Опциональный компонент
public class EnemyAttack : MonoBehaviour {

	Enemy enemy;
	Rigidbody rigidbody;
	Animator animator;

	private float timeNextAttack;


	public int damage;
	public float attackDistanceTreshold;
	public float msTimeBetweenAttacks;
	public AudioClip atackSound;

	void Start () 
	{
		enemy = GetComponent<Enemy> ();
		rigidbody = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
	}

	void Update () 
	{
		if (!enemy.isDead && enemy.currentState != Enemy.State.Jumping)
		{
			if (Time.time > timeNextAttack) 
			{
				float sqrDistToTarget = (enemy.target.position - transform.position).sqrMagnitude;
				if (sqrDistToTarget < Mathf.Pow (attackDistanceTreshold, 2)) 
				{
					timeNextAttack = Time.time + msTimeBetweenAttacks/1000;
					AttackStart();
				}
			}
		}
	}
		
	void AttackStart()
	{
		transform.LookAt (enemy.target);
		enemy.currentState = Enemy.State.Attacking;
		animator.Play("ATTACK");
		Invoke ("AttackHit", 0.5f);
	}

	public void AttackHit()
	{
		if (!enemy.isDead) 
		{
			// проверка расстояния до цели
			float sqrDistToTarget = (enemy.target.position - transform.position).sqrMagnitude;
			if (sqrDistToTarget < Mathf.Pow (attackDistanceTreshold, 2)) 
			{
				Vector3 targetHitPlace = enemy.target.position;
				targetHitPlace.y = 1f;
				enemy.target.GetComponent<Player> ().TakeHit (damage, targetHitPlace, -transform.forward);
			}

			SoundManager.instance.PlaySound (atackSound, transform.position);
			enemy.currentState = Enemy.State.Chasing;

		}
	}
}
