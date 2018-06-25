using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIpanel : MonoBehaviour {

	private bool cursorOverPanel = false;

	// курсор входит на панел
	public void CursorEnterPanel()
	{
		cursorOverPanel = true;
		Debug.Log ("IN");
	}

	// курсор выходит из области панели
	public void CursorExitPanel()
	{
		cursorOverPanel = false;
		Debug.Log ("OUT");
	}

	// метод получения положения курсора относительно панели 
	public bool GetCursorOverPanel()
	{
		return cursorOverPanel;
	}
}
