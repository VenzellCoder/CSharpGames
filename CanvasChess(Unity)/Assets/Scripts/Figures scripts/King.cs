using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Figure {

	// определение легитимных клеток для движения 
	public override bool[,] LegalMovements(Figure[,] figuresArray)
	{
		bool[,] movementArray = new bool[8, 8];
	
		// проверка окружающих короля клеток, начиная с верхней, по ходу часовой стрелки
		if (KingCheckMove(+0,+1,figuresArray)) movementArray [x+0, y+1] = true;
		if (KingCheckMove(+1,+1,figuresArray)) movementArray [x+1, y+1] = true;
		if (KingCheckMove(+1,+0,figuresArray)) movementArray [x+1, y+0] = true;
		if (KingCheckMove(+1,-1,figuresArray)) movementArray [x+1, y-1] = true;
		if (KingCheckMove(+0,-1,figuresArray)) movementArray [x+0, y-1] = true;
		if (KingCheckMove(-1,-1,figuresArray)) movementArray [x-1, y-1] = true;
		if (KingCheckMove(-1,+0,figuresArray)) movementArray [x-1, y-0] = true;
		if (KingCheckMove(-1,+1,figuresArray)) movementArray [x-1, y+1] = true;

		return movementArray;
	}

	// проверка одного шага
	// dx, dy - смещение относительно клетки с фигурой 
	bool KingCheckMove(int dx, int dy, Figure[,] figuresArray)
	{
		// вернуть true если на клетке пусто или есть фигура противника 
		return (y+dy<8 && y+dy>=0 && x+dx<8 && x+dx>=0 && (figuresArray [x + dx, y + dy] == null || (figuresArray [x + dx, y + dy] != null && figuresArray [x + dx, y + dy].colour != colour)));
	}


}
