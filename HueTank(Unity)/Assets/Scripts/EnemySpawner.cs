using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	private int spawnersAmount;
	private Transform[] spawnPointsArray;
	public Transform[] enemiesArray;
	public int enemyAmountMax;
	private int enemyAmount;

	void Start () 
	{
		spawnersAmount = transform.childCount;
		spawnPointsArray = new Transform[spawnersAmount];

		for (int i = 0; i < spawnersAmount; i++) 
		{
			spawnPointsArray [i] = transform.GetChild (i);
		}

		Invoke ("Spawn", 2f);
	}
	
	void Spawn()
	{
		int spawnerIndex = Random.Range (0, spawnPointsArray.Length);
		Transform spawner = spawnPointsArray [spawnerIndex];

		int enemyIndex = Random.Range (0, enemiesArray.Length);

		Transform newEnemy = Instantiate (enemiesArray[enemyIndex], spawner.position, spawner.rotation);
		newEnemy.GetComponent<Enemy>().OnDeath += OnEnemyDeath;

		enemyAmount++;

		if (enemyAmount < enemyAmountMax) 
		{
			Invoke ("Spawn", 0.5f);
		}
	}

	// вызывается при каждой смерти врага
	public void OnEnemyDeath()
	{
		enemyAmount--;
		Spawn ();
	}
}
