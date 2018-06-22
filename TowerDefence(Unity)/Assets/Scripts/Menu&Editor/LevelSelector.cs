using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	public static int chosenLevel = 0;
	public Transform[] levels;
	int levelsAmount;
	public PanelMain panelMain;

	// считаем кол-во уровней по кол-ву кнопок уровней
	void Start()
	{
		levelsAmount = transform.childCount;
		levels = new Transform[levelsAmount];

		for (int i = 0; i < levelsAmount; i++) 
		{
			levels [i] = transform.GetChild (i);
		}

		// по умолчанию выбран первый уровень
		SelectLevel (0);

	}
		
	public void SelectLevel(int _chosenLevel)
	{
		chosenLevel = _chosenLevel;
		DataTransfer.chosenLevel = chosenLevel;
		// перерисовка GUI панели редактора волн
		panelMain.Clear ();
		panelMain.Initialization ();

		for (int i = 0; i < levelsAmount; i++) 
		{
		
			if (i == chosenLevel) 
			{
				levels [i].GetComponent<Image> ().color = Color.green;
			} else 
			{
				levels [i].GetComponent<Image> ().color = Color.white;
			}
		}
	}
}
