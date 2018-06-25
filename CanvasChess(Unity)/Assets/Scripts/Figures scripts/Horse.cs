using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Figure {

	// определение легитимных клеток для движения 
	public override bool[,] LegalMovements(Figure[,] figuresArray)
	{
		bool[,] movementArray = new bool[8, 8];
	
		// сюда можно прыгать и/или атаковать
		// "Г" вверх
		if (HorceCheckMove(+1,+2,figuresArray)) movementArray [x+1, y+2] = true;
		if (HorceCheckMove(-1,+2,figuresArray)) movementArray [x-1, y+2] = true;
		if (HorceCheckMove(+2,+1,figuresArray)) movementArray [x+2, y+1] = true;
		if (HorceCheckMove(-2,+1,figuresArray)) movementArray [x-2, y+1] = true;
		// "Г" вниз
		if (HorceCheckMove(+1,-2,figuresArray)) movementArray [x+1, y-2] = true;
		if (HorceCheckMove(-1,-2,figuresArray)) movementArray [x-1, y-2] = true;
		if (HorceCheckMove(+2,-1,figuresArray)) movementArray [x+2, y-1] = true;
		if (HorceCheckMove(-2,-1,figuresArray)) movementArray [x-2, y-1] = true;

		return movementArray;
	}

	// проверка одной "Г"
	// dx, dy - смещение относительно клетки с фигурой 
	bool HorceCheckMove(int dx, int dy, Figure[,] figuresArray)
	{
		// вернуть true если на клетке пусто или есть фигура противника 
		return (y+dy<8 && y+dy>=0 && x+dx<8 && x+dx>=0 && (figuresArray [x + dx, y + dy] == null || (figuresArray [x + dx, y + dy] != null && figuresArray [x + dx, y + dy].colour != colour)));
	}
}
