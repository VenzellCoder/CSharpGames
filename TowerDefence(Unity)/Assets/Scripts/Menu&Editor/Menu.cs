using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	
	private int chosenLevel;


	public void StartGame()
	{
		chosenLevel = LevelSelector.chosenLevel+1;
		SceneManager.LoadScene (chosenLevel.ToString ());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit ();
		}
	}
}
