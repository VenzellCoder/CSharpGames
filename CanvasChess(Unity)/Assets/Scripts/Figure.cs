using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Figure : MonoBehaviour {

	// компонент управения анимацией фигур
	public Animator animator;
	// цвет фигуры - условно "чёрный / белый"
	[HideInInspector]
	public string colour = " ";
	// тип фигуры 
	[HideInInspector]
	public char type = ' '; 
	// координаты фигуры
	[HideInInspector]
	public int x;
	[HideInInspector]
	public int y;
	// первый шаг
	[HideInInspector]
	public bool firstStep = true;


	void Start()
	{
		animator = GetComponent<Animator> ();
	}

	// установка цвета
	public void PaintSprite()
	{
		switch (colour)
		{
		case "white":
			GetComponent<Image> ().color = new Color32 (79, 123, 159, 255);
			break;
		case "black":
			GetComponent<Image> ().color = new Color32 (210, 94, 63, 255);
			break;
		}
	}

	// движение на новую клетку
	public void MoveToCell(int cellSize, int offsetL, int offsetB)
	{
		GetComponent<Transform> ().position = new Vector2 (offsetL + cellSize/2 + x * cellSize, offsetB + cellSize/2 + y * cellSize);
	}

	// Логика движения. Реализована в дочерних классах - фигурах
	public abstract bool[,] LegalMovements (Figure[,] figuresArray);


	// Логика линейного движения Ладьи, Слона, Ферзя
	public bool[,] LineMovementLogic (int dx, int dy, Figure[,] figuresArray, bool[,] movementArray)
	{
		for (int i = 1; i < 8; i++) 
		{
			// контроль выхода за границу доски 
			if (x + dx*i < 8 && y + dy*i < 8 && x + dx*i >= 0 && y + dy*i >= 0) 
			{
				// если на клетке никого нет, то на неё можно идти
				if (figuresArray [x + dx*i, y + dy*i] == null) 
				{
					movementArray [x + dx*i, y + dy*i] = true;
				} 
				else 
				{
					// если линия оборвалась на противнике - добавить эту клетку в массив доступных
					if (figuresArray [x + dx*i, y + dy*i].colour != colour) 
					{
						movementArray [x + dx*i, y + dy*i] = true;
					}
					break;
				}
			}
		}
		return movementArray;
	}
}
