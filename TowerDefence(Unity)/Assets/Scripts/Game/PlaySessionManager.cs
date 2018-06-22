using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaySessionManager: MonoBehaviour {

	public int money;
	public int hp;
	public int wavesAmount;
	public int wavesCurren;
	// статистика
	public int statKills;
	public int statBuild;
	public int statMoney;

	public bool gameOver;
	private GameObject gameOverScreen;
	private Text gameOverText;

	public bool checkingEnemyVanish;

	// При событии "Изменение денег" все кнопки покупки башен обновят состояние доступности 
	public delegate void OnChangeMoneyDelegate ();
	public event OnChangeMoneyDelegate OnMoneyChange;

	// условие победы - нет врагов на сцене. Проверка включается после прохождения всех волн из WaveSpawner 
	void Update()
	{
		if (!gameOver && checkingEnemyVanish) 
		{
			if (GameObject.FindGameObjectsWithTag ("enemy").Length == 0) 
			{
				GameOver ("ПОБЕДА :)");
			}
		}
	}
		
	void Awake()
	{
		gameOverText = GameObject.Find ("gameOverTextTitle").GetComponent<Text>();
		gameOverScreen = GameObject.Find ("PanelGameOver");
		gameOverScreen.SetActive (false);
	}

	// обновление данных GUI
	void Start()
	{
		wavesAmount = DataTransfer.wavesAmount;

		WaveTextUpdate(1);
		MoneyTextUpdate();
		HealthTextUpdate();
	}

	public void ChangeMoney(int _moneyChange)
	{
		// если это доход - сохраним в статистику
		if (_moneyChange > 0) 
		{
			statMoney += _moneyChange;
		}

		money += _moneyChange;
		MoneyTextUpdate ();

		if (OnMoneyChange != null) 
		{
			OnMoneyChange ();
		}
	}

	public void TakeDamage(int _damage)
	{
		if (!gameOver) 
		{
			hp -= _damage;
			HealthTextUpdate ();

			if (hp <= 0) {
				GameOver ("ПРОИГРАЛ :(");
			}
		}
	}

	// Конец игры при выигрыше или поражении
	private void GameOver(string _winOrLoose)
	{
		gameOver = true;
		gameOverText.text = _winOrLoose;
		gameOverScreen.SetActive (true);
		// статистика
		GameObject.Find ("gameOverTextKils").GetComponent<Text>().text = "Убито врагов - " + statKills;
		GameObject.Find ("gameOverTextMoney").GetComponent<Text>().text = "Заработано денег - " + statMoney;
		GameObject.Find ("gameOverTextBuild").GetComponent<Text>().text = "Построено башен - " + statBuild;
		// спрятать игровой GUI
		GameObject.Find ("InGameGUI").SetActive (false);
	}


	public void MainMenu()
	{
		SceneManager.LoadScene ("menu");
	}


	// обноление GUI
	public void WaveTextUpdate(int _wavesCurren)
	{
		wavesCurren = _wavesCurren;
		GameObject.Find ("TextWave").GetComponent<Text> ().text = "Волна: " + wavesCurren + "/" + wavesAmount;
	}

	public void MoneyTextUpdate()
	{
		GameObject.Find ("TextMoney").GetComponent<Text> ().text = "Деньги: " + money;
	}

	public void HealthTextUpdate()
	{
		GameObject.Find ("TextHealth").GetComponent<Text> ().text = "Жизни: " + hp;
	}
 

}
