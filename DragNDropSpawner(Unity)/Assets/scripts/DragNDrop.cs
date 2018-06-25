using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour {

	// главная панель
	public GUIpanel panel;
	// префаб создаваемого юнита
	public Unit unit;
	// перетаскиваемое изображение
	public Texture2D dragImage;
	// размер перетаскиваемого изображения
	public int dragImageSize = 300;
	// главная камера
	public Camera camera;
	// канва для цифр
	public Canvas numberCanvas;

	// сейчас DnD
	private bool isDraging = false;
	// находится ли курсор над панелью
	private bool cursorOverGUI = false;
	// кол-во созданных юнитов
	private int unitNumber = 1;


	// начало DnD
	public void DragStart()
	{
		isDraging = true;
	}

	// конец DnD
	public void DragFinish()
	{
		isDraging = false;

		// проверка нахождения курсора над панелью GUI
		if (!panel.GetCursorOverPanel ())
		{
			// создание луча
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;

			// проверка попадания луча в поверхность
			if (Physics.Raycast (ray, out hitInfo)) {
				// позиция спавна юнита
				Vector3 spawnPlace = new Vector3 (hitInfo.point.x, -4.5f, hitInfo.point.z);
				// спавн юнита 
				Unit newUnit = Instantiate (unit, spawnPlace, Quaternion.identity);
				newUnit.textCanvas = numberCanvas;

				newUnit.text.text = unitNumber.ToString();
				unitNumber++;
			}
		}
	}

	// отрисовка изображения при перетаскивании 
	void OnGUI()
	{
		if (isDraging) 
		{
			GUI.DrawTexture (new Rect (Event.current.mousePosition.x - (dragImageSize / 2), Event.current.mousePosition.y - (dragImageSize / 2), dragImageSize, dragImageSize), dragImage);
		}
	}
}
