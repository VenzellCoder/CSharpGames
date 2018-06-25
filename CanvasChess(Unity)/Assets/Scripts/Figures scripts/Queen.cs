﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Figure {

	// определение легитимных клеток для движения 
	public override bool[,] LegalMovements(Figure[,] figuresArray)
	{
		// создание пустоего масиива (нет клеток, доступных для передвижения)
		bool[,] movementArray = new bool[8, 8];

		// постепенное заполнение массива, при прохождении по 4-ём диагоналям, как Ладья
		movementArray = LineMovementLogic (+1,+1,figuresArray,movementArray);
		movementArray = LineMovementLogic (-1,-1,figuresArray,movementArray);
		movementArray = LineMovementLogic (-1,+1,figuresArray,movementArray);
		movementArray = LineMovementLogic (+1,-1,figuresArray,movementArray);

		// постепенное заполнение массива, при прохождении по 4-ём направлениям, как Слон
		movementArray = LineMovementLogic (+1,+0,figuresArray,movementArray);
		movementArray = LineMovementLogic (-1,+0,figuresArray,movementArray);
		movementArray = LineMovementLogic (+0,+1,figuresArray,movementArray);
		movementArray = LineMovementLogic (+0,-1,figuresArray,movementArray);

		return movementArray;
	}
}