using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Figure {

	// определение легитимных клеток для движения 
	public override bool[,] LegalMovements(Figure[,] figuresArray)
	{
		bool[,] movementArray = new bool[8, 8];

		if (colour == "white") 
		{
			// первый шаг может быть на 2 клетки, если путь не загорожен
			if (firstStep && x + 2 < 8 && figuresArray [x + 2, y] == null && figuresArray [x + 1, y] == null)
			{
				movementArray [x + 2, y] = true;
			}
			// сюда можно идти (белые ходят только вправо)
			if (x + 1 < 8 && figuresArray [x + 1, y] == null) 
			{
				movementArray [x + 1, y] = true;
			}
			// сюда можно атаковать (диагональ вверх)
			if (y + 1 < 8 && figuresArray [x + 1, y + 1] != null && figuresArray [x + 1, y + 1].colour != colour) 
			{
				movementArray [x + 1, y + 1] = true;
			}
			// сюда можно атаковать (диагональ вниз)
			if (y - 1 >= 0 && figuresArray [x + 1, y - 1] != null && figuresArray [x + 1, y - 1].colour != colour) 
			{
				movementArray [x + 1, y - 1] = true;
			}
		}
		if (colour == "black") 
		{
			// первый шаг может быть на 2 клетки
			if (firstStep && x - 2 < 8 && figuresArray [x - 2, y] == null && figuresArray [x - 1, y] == null)
				{
					movementArray [x - 2, y] = true;
				}
			// сюда можно идти (чёрные ходят только влево)
			if (x - 1 >= 0 && figuresArray [x - 1, y] == null) 
			{
				movementArray [x - 1, y] = true;
			}
			// сюда можно атаковать (диагональ вверх)
			if (y + 1 < 8 && figuresArray [x - 1, y + 1] != null && figuresArray [x - 1, y + 1].colour != colour) 
			{
				movementArray [x - 1, y + 1] = true;
			}
			// сюда можно атаковать (диагональ вниз)
			if (y - 1 >= 0 && figuresArray [x - 1, y - 1] != null && figuresArray [x - 1, y - 1].colour != colour) 
			{
				movementArray [x - 1, y - 1] = true;
			}
		}
		return movementArray;
	}

}
