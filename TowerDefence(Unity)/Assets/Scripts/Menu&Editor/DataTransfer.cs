using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : MonoBehaviour {

	// данные могут поступать из JSON
	// тут - заглушка из уже созданных массивов
	public SubWave[,] wavesLevel1;
	public SubWave[,] wavesLevel2;
	public SubWave[,] wavesLevel3;

	// доступ к паттернам уровней по индеку 0,1,2...
	public static List<SubWave[,]> levelsWavesList; 

	public static int chosenLevel;
	public static int subWavesAmount;
	public static int wavesAmount;
	string[] arrayTypes;
	public static DataTransfer instance; 


	// Одиночка
	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		} 
		else if (instance != this) 
		{
			Destroy (gameObject);
		}
		// для передачи данных между сценами
		DontDestroyOnLoad (gameObject);
	}

	void Start () 
	{
		// стандартные значения 
		subWavesAmount = 3;
		wavesAmount = 2;

		wavesLevel1 = new SubWave[10,10];
		wavesLevel2 = new SubWave[10,10];
		wavesLevel3 = new SubWave[10,10];
		arrayTypes = new string[]{"A", "A", "B", "C"};

		// заполнение массивов паттернов волн случайными значениями
		for (int w = 0; w < 10; w++) 
		{
			for (int sw = 0; sw < 10; sw++) 
			{
				wavesLevel1 [w, sw] = new SubWave  (arrayTypes [Random.Range (0, 4)]);
				wavesLevel2 [w, sw] = new SubWave  (arrayTypes [Random.Range (0, 4)]);
				wavesLevel3 [w, sw] = new SubWave  (arrayTypes [Random.Range (0, 4)]);
			}	
		}
		// Лист с паттернами волн всех уровней
		levelsWavesList = new List<SubWave[,]>();
		levelsWavesList.Add(wavesLevel1);
		levelsWavesList.Add(wavesLevel2);
		levelsWavesList.Add(wavesLevel3);
	}
}

// структура хранения данных о subwave (этап волны)
public class SubWave
{	
	public string type; // тип врагов
	public int amount; // количество врагов в этапе
	public float delayBetween; // время между спавном врагов в этае
	public float delayEnd; // задержка перед следующим этапом в волне

	public SubWave(string _type)
	{
		
		type = _type;

		// обычные
		if (type == "A") 
		{
			amount = Random.Range (3, 10);
			delayBetween = Mathf.Round (Random.Range (0.2f, 0.8f) * 10f) / 10f;
			delayEnd = Mathf.Round (Random.Range (3f, 5f) * 10f) / 10f;
		}
		// медленные, мощные
		else if (type == "B") 
		{
			amount = Random.Range (1, 4);
			delayBetween = Mathf.Round (Random.Range (3f, 5f) * 10f) / 10f;
			delayEnd = Mathf.Round (Random.Range (5f, 7f) * 10f) / 10f;
		}
		// быстрые, слабые
		else if (type == "C") 
		{
			amount = Random.Range (5, 15);
			delayBetween = Mathf.Round (Random.Range (0.2f, 0.4f) * 10f) / 10f;
			delayEnd = Mathf.Round (Random.Range (3f, 5f) * 10f) / 10f;
		}
	}
}
