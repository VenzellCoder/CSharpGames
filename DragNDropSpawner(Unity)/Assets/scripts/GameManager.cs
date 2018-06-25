using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// размер курсора
	public int cursorSize = 60;
	// изображение курсора 
	public Texture2D cursorTex;

	void Start()
	{
		// отключение вертикальной синхронизации
		QualitySettings.vSyncCount = 0;
		// настоящий курсор невидим
		Cursor.visible = false;
	}
		
	// отрисовка кастомного курсора
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, cursorSize, cursorSize), cursorTex);
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Application.Quit ();
		}
	}
}
