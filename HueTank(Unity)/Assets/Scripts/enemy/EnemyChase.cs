using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// поведение: Погоня за игроком. Опциональный компонент
public class EnemyChase : MonoBehaviour {

	Enemy enemy;
	NavMeshAgent pathfinder;
	Animator animator;


	void Start () 
	{
		enemy = GetComponent<Enemy> ();
		pathfinder = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();

		// цель NavMesh агента
		enemy.target = GameObject.FindGameObjectWithTag ("Player").transform;
		InvokeRepeating ("UpdateTarget", 0f, 0.2f);

	}


	void Update () 
	{
		
		if (!enemy.isDead && enemy.currentState == Enemy.State.Chasing) 
		{
			// управление анимацией
			if (pathfinder.velocity.magnitude < 0.1) 
			{

				animator.SetBool ("isMoving", false);
			} 
			else 
			{
				if (animator.GetBool ("isMoving") == false) 
				{
					// анимация начинается с случайного места, чтобы толпа не была одинаковой 
					animator.SetBool ("isMoving", true);
					animator.Play ("MOVE", 0, Random.Range (0f, 1f));
				}
			}
		}
	}


	// обновление цели NavMesh агента 
	void UpdateTarget ()
	{
		if (pathfinder.enabled) 
		{
			if (enemy.currentState == Enemy.State.Chasing) 
			{
				if (enemy.target != null) 
				{
					pathfinder.SetDestination (enemy.target.position);	
				}
			}
		}
	}
}
