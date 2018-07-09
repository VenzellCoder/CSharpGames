using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// менеджер отвечает за GUI жизней игрока и отожражение GUI оружия
public class GUIManager : MonoBehaviour {

	// GUI элементы, отображающиеся во время игры
	public Transform gameModeGUI;
	// GUI элементы, отображающиеся при проигрыше
	public Transform gameOverGUI;

	public Image GUIhp;
	public Transform GUIallGunsPanel;
	// префаб GUI пушки
	public GUIgunPanel GUIgunPanel;

	private GUIgunPanel[] GUIgunsArray;
	private int GUIgunsAmount;


	void Start () 
	{
		GUIgunsAmount = GetComponent<GunController> ().gunArsenal.Length;
		GUIgunsArray = new GUIgunPanel[GUIgunsAmount];

		CreateGUIguns ();
		ChangeGUIgunActive ();
	}

	// нарисуем все пушки игрока на GUI. Из пушек берём названия и их спрайты 
	void CreateGUIguns()
	{
		for (int i=0; i<GUIgunsAmount; i++)
		{
			GUIgunPanel newGUIgaunPanel = Instantiate (GUIgunPanel, transform.position, Quaternion.identity);
			newGUIgaunPanel.transform.SetParent(GUIallGunsPanel);

			newGUIgaunPanel.GUIgunName.text = GetComponent<GunController> ().gunArsenal [i].gunName;
			newGUIgaunPanel.GUIgunImage.sprite = GetComponent<GunController> ().gunArsenal [i].gunImage;

			GUIgunsArray [i] = newGUIgaunPanel;
		}
	}

	public void ChangeGUIgunActive()
	{
		for (int i=0; i<GUIgunsAmount; i++)
		{
			if (i == GetComponent<GunController> ().currentGunIndex) 
			{
				GUIgunsArray [i].FrameOn();
			} 
			else 
			{
				GUIgunsArray [i].FrameOff();
			}
		}
	}

	public void GameOverGUI()
	{
		gameModeGUI.gameObject.SetActive (false);
		gameOverGUI.gameObject.SetActive (true);
	}

	public void UpdateHpGUI(float _hpStart, float _hp)
	{	
		GUIhp.fillAmount = _hp / _hpStart;
	}
}
