using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


	public Transform body;

	void Start () 
	{
		Invoke ("Spawn", 2f);
	}
	
	void Spawn()
	{
		int enemySpawnNumber = Random.Range (1, 4);
		for (int i=0; i<enemySpawnNumber; i++)
		{
			Transform newBody = Instantiate (body, transform.position, transform.rotation);
			//newBody.Rotate (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
		}
		Invoke ("Spawn", Random.Range (10f, 30f));
	}
}
