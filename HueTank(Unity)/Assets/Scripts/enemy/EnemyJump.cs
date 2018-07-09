using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// поведение: Прыжок в сторону игрока.  Опциональный компонент
public class EnemyJump : MonoBehaviour {

	Enemy enemy;
	Rigidbody rigidbody;
	NavMeshAgent pathfinder;
	Animator animator;

	public float jumpDistanceTreshold;
	public AudioClip jumpSound;

	void Start () 
	{
		enemy = GetComponent<Enemy> ();
		rigidbody = GetComponent<Rigidbody> ();
		pathfinder = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();

		enemy.target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update () 
	{
		// если приследует игрока и оказался на расстоянии прыжка - прыгнуть
		if (!enemy.isDead && enemy.currentState == Enemy.State.Chasing)
		{
			float sqrDistToTarget = (enemy.target.position - transform.position).sqrMagnitude;
			if (sqrDistToTarget < Mathf.Pow (jumpDistanceTreshold, 2)) 
			{
				Jump();
			}
		}

		// если в прыжке и приземлился - преследует
		if (enemy.currentState == Enemy.State.Jumping) 
		{
			if (rigidbody.velocity.magnitude < 0.1) 
			{
				enemy.currentState = Enemy.State.Chasing;
				pathfinder.enabled = true;
			}
		}
	}

	void Jump()
	{
		SoundManager.instance.PlaySound (jumpSound, transform.position);

		enemy.currentState = Enemy.State.Jumping;
		transform.LookAt (enemy.target);
		pathfinder.enabled = false;

		float jumpForce = 100f;
		Vector3 jumpVector = transform.forward.normalized * jumpForce;
		jumpVector.y = jumpForce*2f;
		rigidbody.AddForce (jumpVector);
	}
}
