using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


	public Transform body;
	public float intervalMin;
	public float intervalMax;


	void Start () 
	{
		Invoke ("Spawn", 2f);
	}
	
	void Spawn()
	{
		int enemySpawnNumber = 1;//Random.Range (amountMin, amountMax);
		for (int i=0; i<enemySpawnNumber; i++)
		{
			//Vector3 delta = new Vector3 (Random.Range (-5f, 5f), transform.position.y, Random.Range (-5f, 5f));
			Transform newBody = Instantiate (body, transform.position, transform.rotation);
			//newBody.Rotate (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
		}
		Invoke ("Spawn", Random.Range (intervalMin, intervalMax));
	}
}
