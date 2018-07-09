using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// класс управляет состоянием игры и изменением цветов.
public class GameManager : MonoBehaviour {

	private enum GameState {mainMenu, gameMode, gameOver};
	private GameState gameState;

	[HideInInspector]
	public Color floorCol;
	[HideInInspector]
	public Color obstacleCol;
	[HideInInspector]
	public Color enemyCol;
	[HideInInspector]
	public Color playerCol;

	// HSV массивы цветов: 0-земля, 1-препятствия, 2-враги, 3-игрок
	// тон
	float[] hueArray;
	// насыщенность
	float[] saturationArray;
	// значение
	float[] valueArray;

	// изменение насыщенности и значения в главном меню
	float[] saturationChangeDir;
	float[] valueDeltaChangeDir;

	// границы насыщенности
	public float[] saturationMin;
	public float[] saturationMax;
	// границы значения 
	public float[] valueMin;
	public float[] valueMax;

	public delegate void OnPalleteChangeDelegate();
	public event OnPalleteChangeDelegate OnPalleteChange;

	static public GameManager instance = null;


	// реализация паттерна Singltone
	void Awake () 
	{
		gameState = GameState.mainMenu;

		if (instance != null) 
		{
			Destroy (gameObject);
		} 
		else 
		{
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}

		// инициализация массивов
		hueArray = new float[4];
		saturationArray = new float[4];
		valueArray = new float[4];
		saturationChangeDir = new float[4];
		valueDeltaChangeDir = new float[4];

		// случайные скорости изменения насыщенности и значения в главном меню
		for (int i = 0; i < 4; i++) 
		{
			saturationChangeDir [i] = Random.Range(0.0005f, 0.001f);
			valueDeltaChangeDir [i] = -Random.Range(0.0005f, 0.001f);
		}

		RandomizePallete ();
		Invoke ("ColourShow", 0.5f);
	}
	

	void Update () 
	{
		// ГЛАВНОЕ МЕНЮ
		if (gameState == GameState.mainMenu) 
		{
			// вращение камеры
			Camera.main.transform.Rotate (Vector3.up * Time.deltaTime * 10f, Space.World);

			if (Input.anyKey) 
			{
				CancelInvoke ();
				SceneManager.LoadScene ("level");
				gameState = GameState.gameMode;
			}

			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				Application.Quit ();
			}
		}

		// ИГРА
		if (gameState == GameState.gameMode) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				Invoke ("ColourShow", 0.5f);
				SceneManager.LoadScene ("menu");
				gameState = GameState.mainMenu;
			}
		}

		// GAME OVER 
		if (gameState == GameState.gameOver) 
		{
			// вращение камеры
			Camera.main.transform.Rotate (Vector3.up * Time.deltaTime * 10f, Space.World);

			if (Input.GetKeyDown (KeyCode.Return)) 
			{
				Invoke ("ColourShow", 0.5f);
				SceneManager.LoadScene ("menu");
				gameState = GameState.mainMenu;
			}
		}
	}

	// Случайный выбор цветов. 
	// Учитывается ограничения насыщенности и значений
	// Цвета четырёх игровых объектов(пол, стены, игрок, враги) на цветовой диаграмме представляют крест (теория цвета)
	void RandomizePallete()
	{
		hueArray[0] = Random.Range (0f, 1f);

		for (int i = 0; i < 4; i++) 
		{
			saturationArray [i] = Random.Range (saturationMin[i], saturationMax[i]);
			valueArray [i] = Random.Range (valueMin[i], valueMax[i]);
		}

		for (int i = 1; i < 4; i++) 
		{
			// квадратная схема теории цвета
			hueArray[i] = hueArray[i-1] + 0.25f;
			if (hueArray[i] > 1f) 
			{
				hueArray[i] -= 1f;
			}
		}
	}

	void ChangeColours()
	{
		floorCol = Color.HSVToRGB (hueArray [0], saturationArray[0], valueArray[0]);
		obstacleCol = Color.HSVToRGB (hueArray [1], saturationArray[1], valueArray[1]);
		enemyCol = Color.HSVToRGB (hueArray [2], saturationArray[2], valueArray[2]);
		playerCol = Color.HSVToRGB (hueArray [3], saturationArray[3], valueArray[3]);
	}

	// при каждом ранении игрока цвета меняются
	public void OnPlayerHit()
	{
		RandomizePallete ();
		ChangeColours ();
		// вызов метода из класса TuneColour() - компонента на объектах, изменяющих цвета
		OnPalleteChange ();
	}

	void ColourShow()
	{
		for (int i = 0; i < 4; i++) 
		{
			hueArray [i] += 0.01f;
			if (hueArray [i] > 1f) 
			{
				hueArray [i] -= 1f;
			}

			saturationArray [i] += saturationChangeDir[i];
			if (saturationArray [i] >= saturationMax[i] || saturationArray [i] <= saturationMin[i]) 
			{
				saturationChangeDir[i] = -saturationChangeDir[i];
			}

			valueArray [i] += valueDeltaChangeDir[i];
			if (valueArray [i] >= valueMax[i] || valueArray [i] <= valueMin[i]) 
			{
				valueDeltaChangeDir[i] = -valueDeltaChangeDir[i];
			}
		}


		ChangeColours ();
		OnPalleteChange ();

		Invoke ("ColourShow", 0.1f);
	}


	public void GameOver()
	{
		gameState = GameState.gameOver;

		GameObject[] enemiesOnScene = new GameObject[10];

		// отключить более ненужные компоненты у врагов на сцене
		enemiesOnScene = GameObject.FindGameObjectsWithTag("Enemies");
		foreach(GameObject enemy in enemiesOnScene)
		{
			enemy.GetComponent<Enemy>().enabled = false;
			enemy.GetComponent<EnemyChase>().enabled = false;
			enemy.GetComponent<EnemyAttack>().enabled = false;
		}
	}
}
