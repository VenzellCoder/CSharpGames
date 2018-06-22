using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveSpawner : MonoBehaviour 
{
	private Transform enemy;
	public Transform enemyPrefabA;
	public Transform enemyPrefabB;
	public Transform enemyPrefabC;
	// параметры обновляются каждую новую подволну
	private string enemyType;
	private int unitsInSubWaveNumber;
	private float timeBetweenUnits;
	private float timeBetweenWaves = 3f; // стандартное значение 
	private float timeBetweenSubWaves;

	public SubWave[,] waves;
	private int subWaveIndex = 0;
	private int subWaveIndexMax;
	private int waveIndex = 0;
	private int waveIndexMax = 0;

	private GameObject spawnPoint;
	private PlaySessionManager playSessionManager;


	void Start () 
	{
		playSessionManager = GameObject.Find ("PlaySessionManager").GetComponent<PlaySessionManager> ();

		spawnPoint = GameObject.Find ("WayPointStart");

		waves = new SubWave[10,10];
		waves = DataTransfer.levelsWavesList[DataTransfer.chosenLevel];
		subWaveIndexMax = DataTransfer.subWavesAmount;
		waveIndexMax = DataTransfer.wavesAmount;

		// Спавн всех волн
		StartCoroutine(SpawnWaves());
	}
	

	IEnumerator SpawnWaves()
	{
		// wave
		for (waveIndex = 0; waveIndex < waveIndexMax; waveIndex++) 
		{
			playSessionManager.WaveTextUpdate (waveIndex + 1);
			yield return new WaitForSeconds (timeBetweenWaves);
			// subwave
			for (subWaveIndex = 0; subWaveIndex < subWaveIndexMax; subWaveIndex++) 
			{
				// обновление данныт данной subwave (какие юниты? сколько?)
				NewSubWaveUpdate ();
				// спавн юнитов
				for (int u=0; u<unitsInSubWaveNumber; u++)
				{
					SpawnEnemy ();
					yield return new WaitForSeconds (timeBetweenUnits);
				}
				yield return new WaitForSeconds (timeBetweenSubWaves);
			}
		}
		// волны закончились. Начало отслеживания уничтожения оставшихся врагов
		playSessionManager.checkingEnemyVanish = true;
	}


	void SpawnEnemy()
	{
		if (enemyType == "A") enemy = enemyPrefabA;
		else if (enemyType == "B") enemy = enemyPrefabB;
		else if (enemyType == "C") enemy = enemyPrefabC;
		Instantiate (enemy, spawnPoint.transform.position, spawnPoint.transform.rotation);
	}


	void NewSubWaveUpdate()
	{
		enemyType = waves [waveIndex, subWaveIndex].type;
		unitsInSubWaveNumber = waves [waveIndex, subWaveIndex].amount;
		timeBetweenUnits = waves [waveIndex, subWaveIndex].delayBetween;
		timeBetweenSubWaves = waves [waveIndex, subWaveIndex].delayEnd;
	}
}
