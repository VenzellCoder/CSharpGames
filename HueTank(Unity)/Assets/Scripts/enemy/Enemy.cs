using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Обязательный компонент на объекте противника
public class Enemy : Unit {

	[HideInInspector]
	public enum State{Idle, Chasing, Attacking, Jumping};
	[HideInInspector]
	public State currentState;
	[HideInInspector]
	public Transform target;

	Rigidbody rigidbody;
	Animator animator;
	NavMeshAgent pathfinder;

	// на это событие подписан спавн менеджер
	public event System.Action OnDeath;


	public override void Start () 
	{
		base.Start ();

		currentState = State.Chasing;

		rigidbody = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		pathfinder = GetComponent<NavMeshAgent> ();
	}
	

	public override void Die()
	{
		base.Die ();

		if (OnDeath != null) 
		{
			OnDeath ();
		}

		SoundManager.instance.PlaySound (dieSound, transform.position);
		animator.Play("DIE");

		pathfinder.enabled = false;
		GetComponent<CapsuleCollider> ().enabled = false;

		Destroy (gameObject, 2f);
	}
}
