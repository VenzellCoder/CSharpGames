using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit {

	public enum State{Idle, Chasing, Attacking};
	State currentState;

	public Color liveColour;
	public Color deadColour;
	public float colourLerpDuration;
	private float colourLerpCounter = 0f;

	Animator animator;
	NavMeshAgent pathfinder;
	Transform target;
	float targetUpdateTime;
	public Transform ragDoll;
	public Transform materialHolderBody;
	public Transform materialHolderHead;

	public float attackDistanceTreshold;
	public float msTimeBetweenAttacks;
	private float timeNextAttack;

	public GameObject hitEffect;

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
	

	void Update ()
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


		if (isDead) 
		{
			materialHolderBody.GetComponent<Renderer> ().material.color = Color.Lerp (liveColour, deadColour, colourLerpCounter);
			materialHolderHead.GetComponent<Renderer> ().material.color = Color.Lerp (liveColour, deadColour, colourLerpCounter);
			if (colourLerpCounter < 1) 
			{ 
				colourLerpCounter += Time.deltaTime / colourLerpDuration;
			}
		}
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
			target.GetComponent<Player>().PlayerHit ();
			//target.GetComponent<Rigidbody> ().AddForce ((target.position - transform.position).normalized * 500);
			//Debug.Log ("HIT!");
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

	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		////// МАГИЯ 
		Destroy(Instantiate(hitEffect, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, 3f);

		base.TakeHit (damage, hitPoint, hitDirection);
	}

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
	}



}
