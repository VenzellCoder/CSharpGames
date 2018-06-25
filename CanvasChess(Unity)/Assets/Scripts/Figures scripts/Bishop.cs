using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Figure {

	// определение легитимных клеток для движения 
	public override bool[,] LegalMovements(Figure[,] figuresArray)
	{
		// создание пустоего масиива (нет клеток, доступных для передвижения)
		bool[,] movementArray = new bool[8, 8];
		// постепенное заполнение массива, при прохождении по 4-ём диагоналям
		movementArray = LineMovementLogic (+1,+1,figuresArray,movementArray);
		movementArray = LineMovementLogic (-1,-1,figuresArray,movementArray);
		movementArray = LineMovementLogic (-1,+1,figuresArray,movementArray);
		movementArray = LineMovementLogic (+1,-1,figuresArray,movementArray);

		return movementArray;
	}
}
