using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit {

	public enum State{Idle, Chasing, Attacking};
	State currentState;

	Animator animator;
	NavMeshAgent pathfinder;
	Transform target;
	float targetUpdateTime;
	public Transform ragDoll;


	public float attackDistanceTreshold;
	public float msTimeBetweenAttacks;
	private float timeNextAttack;



	public override void Start () 
	{
		base.Start ();

		currentState = State.Chasing;

		animator = GetComponent<Animator> ();
		pathfinder = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		targetUpdateTime = Random.Range (0.2f, 2.0f);
		InvokeRepeating ("UpdateTarget", 0f, 0.2f);


	}
	

	public override void Update ()
	{
		if (currentState == State.Chasing) 
		{
			if (pathfinder.velocity.magnitude < 0.1) {
				
				animator.SetBool ("run", false);
			} else {
				if (animator.GetBool ("run") == false) {
					animator.SetBool ("run", true);
					animator.Play ("RUN", 0, Random.Range (0f, 1f));
				}
			}
		}

		if (Time.time > timeNextAttack) 
		{
			float sqrDistToTarget = (target.position - transform.position).sqrMagnitude;
			if (sqrDistToTarget < Mathf.Pow (attackDistanceTreshold, 2)) 
			{
				timeNextAttack = Time.time + msTimeBetweenAttacks/1000;
				AttackStart();
			}
		}


		base.Update ();



	}


	void AttackStart()
	{
		currentState = State.Attacking;
		animator.Play("ATTACK");
	}

	public void AttackHit()
	{
		float sqrDistToTarget = (target.position - transform.position).sqrMagnitude;
		if (sqrDistToTarget < Mathf.Pow (attackDistanceTreshold, 2)) 
		{
			Vector3 targetHitPlace = target.position;
			targetHitPlace.y = 1f;
			target.GetComponent<PlayerNet>().TakeHit (1, targetHitPlace, -transform.forward);
		}
	}

	public void AttackEnd()
	{
		currentState = State.Chasing;
	}


	void UpdateTarget ()
	{
		if (currentState == State.Chasing) {
			if (target != null)
			{
				pathfinder.SetDestination (target.position);	
				pathfinder.stoppingDistance = attackDistanceTreshold;	
			}
		}
	}
	/*
	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		

		base.TakeHit (damage, hitPoint, hitDirection);
	}
*/
	public override void Die()
	{
		CancelInvoke ();
		GetComponent<NavMeshAgent> ().enabled = false;
		GetComponent<CapsuleCollider> ().enabled = false;
		GetComponent<Animator>().enabled = false;
		GetComponent<Rigidbody> ().isKinematic = true;

		GetComponent<RagDoll> ().TurnOnRagDall ();
		GetComponent<RagDoll> ().HeadDown ();
		GetComponent<RagDoll> ().StartCoroutine ("TurnOffRagDoll");

		isDead = true;

		//gameObject.layer = LayerMask.NameToLayer ("DeadBodies");
	}



}
