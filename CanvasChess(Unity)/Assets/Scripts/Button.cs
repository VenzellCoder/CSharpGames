using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour {

	// показать всю кнопку
	public void Show()
	{
		GetComponent<RectTransform> ().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 700);
	}

	// спрятать кнопку
	public void Hide()
	{
		GetComponent<RectTransform> ().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 420);
	}

	// выход
	public void Exit()
	{
		Application.Quit ();
	}

	// рестарт игры
	public void Replay()
	{
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}

	// открыть резюме
	public void LinkCV()
	{
		Application.OpenURL ("https://drive.google.com/open?id=1B_HzlldhkOzCcepSLL3xo_N1-v2zSSuf");
	}
}
