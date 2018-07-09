using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Панель одной пушки (рисунок, рамка выбора оружия, название)
public class GUIgunPanel : MonoBehaviour {

	public Text GUIgunName;
	public Image GUIgunImage;
	public Transform GUIactiveFrame;

	[HideInInspector]
	public bool isActive;

	// оружие выбрано
	public void FrameOn()
	{
		GUIactiveFrame.gameObject.SetActive (true);
		GUIgunName.gameObject.SetActive (true);
	}

	// оружие не выбрано
	public void FrameOff()
	{
		GUIactiveFrame.gameObject.SetActive (false);
		GUIgunName.gameObject.SetActive (false);
	}
}
