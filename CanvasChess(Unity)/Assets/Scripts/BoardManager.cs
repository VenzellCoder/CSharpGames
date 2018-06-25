using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {


	// префаб клетки поля 
	public Cell cell;
	// канва для фигур
	public Canvas boardCanvas;
	// канва для фигур
	public Canvas figureCanvas;
	// префабы фигур
	public Pawn pawnFigure;
	public Horse horseFigure;
	public Rook rookFigure;
	public Bishop bishopFigure;
	public King kingFigure;
	public Queen queenFigure;
	// игровое поле
	private Figure[,] boardArray = new Figure[8,8];
	// массив клеток (храниятся для покраски и подсвечивания)
	private Cell[,] cellArray = new Cell[8,8];
	// активная фигура
	Figure activeFigure;
	// активный игрок
	private string activeColour = "white";

	// поля для авторассчёта пропорций 
	// размер клетки поля
	int cellSize;
	//отступы сниху и слева
	int offsetB;
	int offsetL;
	// разрешение экрана
	int screenW;
	int screenH;

	// текст "твой ход"
	public Text textYourTurn;

	/* Сокращения названий фигур:
	 * P - Pawn
	 * R - Rook
	 * H - Horse
	 * B - Bishop
	 * Q - Queen
	 * K - King	  
	*/
	// начальная расстановка фигур. 

	char[,] startBoardArray = new char[,]{{'R','P',' ',' ',' ',' ','P','R'},
										  {'H','P',' ',' ',' ',' ','P','H'},
									  	  {'B','P',' ',' ',' ',' ','P','B'},
										  {'Q','P',' ',' ',' ',' ','P','Q'},
										  {'K','P',' ',' ',' ',' ','P','K'},
										  {'B','P',' ',' ',' ',' ','P','B'},
										  {'H','P',' ',' ',' ',' ','P','H'},
										  {'R','P',' ',' ',' ',' ','P','R'}};

	// для нестандартных раскладок 
	/*
	char[,] startBoardArray = new char[,]{{' ',' ',' ',' ',' ',' ',' ',' '},
										  {' ',' ',' ',' ',' ',' ',' ',' '},
										  {' ',' ',' ',' ',' ',' ','R',' '},
										  {' ',' ','Q',' ',' ',' ','B',' '},
										  {' ',' ','Q',' ',' ',' ','B',' '},
									   	  {' ',' ',' ',' ',' ',' ','R',' '},
										  {' ',' ',' ',' ',' ',' ',' ',' '},
										  {' ',' ',' ',' ',' ',' ',' ',' '},};
	*/
	void Start () 
	{
		// размеры окна
		screenW = Screen.width;
		screenH = Screen.height;
		// грубый размер клетки поля
		cellSize = (screenH-100)/8;
		// размер клетки поля округлённый до целых десятков 
		//int cellSize = (int) Mathf.Round(rawCellSize/10) * 10;
		//отступы сниху и слева
		offsetB = (screenH - cellSize*8)/2;
		offsetL = (screenW - cellSize*8)/2;



		// создание игрового поля и расстановка фигур
		for (int x = 0; x < 8; x++) 
		{
			for (int y=0; y<8; y++)
			{
				// инстанцинация клеток игрового поля 
				Cell newCell = Instantiate (cell, new Vector2 (offsetL+x*cellSize, offsetB+y*cellSize), Quaternion.identity);
				newCell.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize);
				newCell.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);
				cellArray [x, y] = newCell;
				// родитель-холст
				newCell.GetComponent<RectTransform> ().parent = boardCanvas.GetComponent<RectTransform> ();
				// если на этой клетке в начале игры должна быть фигура, она создаётся, ссылка на фигуру хранится в клетке 
				if (startBoardArray [y, x] != ' ')
				{
					FigureInitialization (x, y);
				}
			}
		}
		// покраска клеток в шахматном порядке 
		ColourBoard ();
	}



	void Update () 
	{
		// подсветка клеток под курсором
		FlashCellUpdate ();
	}

	public void ClickOnCell()
	{
		ColourBoard ();

		// координаты нажатой клетки поля
		int clicCellX = (int)((Input.mousePosition.x-offsetL) / cellSize);
		int clicCellY = (int)((Input.mousePosition.y-offsetB) / cellSize);
		// Фигура (если есть) на выбранной клетке 
		Figure FigureOnCell = boardArray [clicCellX, clicCellY];

		// фигура не выбрана
		if (activeFigure == null) 
		{
			// выбор своей фигуры
			if (FigureOnCell != null && FigureOnCell.colour == activeColour) 
			{
				// теперь эта фигура активна
				activeFigure = FigureOnCell;
				// запуск анимации активной фигуры 
				activeFigure.animator.Play ("ACTIVE");

				// покраска клеток, доступных для передвижения (Дебаг)
				bool[,] arr = activeFigure.LegalMovements (boardArray);
				for (int x = 0; x < 8; x++) 
				{
					for (int y = 0; y < 8; y++) 
					{  
						if (arr [x, y] == true) 
						{
							// функция для Дебага
							//cellArray [x, y].GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
						} 
					}
				}
				return;
			}
		}

		// ранее была выбрана активная фигура 
		if (activeFigure != null) 
		{
			// выбор другой своей фигуры
			if (FigureOnCell != null && FigureOnCell.colour == activeColour) 
			{
				// анимация ожидания предыдущей активной фигуры
				activeFigure.animator.Play ("IDLE");
				activeFigure = FigureOnCell;
				// анимация новой активной фигуры
				activeFigure.animator.Play ("ACTIVE");
				return;
			}

			// массив доступных клеток для движения 
			bool[,] arr = activeFigure.LegalMovements (boardArray);
			// выбор доступной клетки для движения 
			if (arr[clicCellX,clicCellY] == true) 
			{
				// на целевой клетке фигура противника
				if (FigureOnCell != null) 
				{
					// съедание фигуры соперника
					Destroy (boardArray [clicCellX, clicCellY].gameObject);
					boardArray [clicCellX, clicCellY] = null;
				}
				// анимация атаки
				activeFigure.animator.Play ("MOVE");

				// запомнить, что фигура сделала первый ход (важно для пешек)
				if (activeFigure.firstStep) activeFigure.firstStep = false;

				// фигура уходит из предыдущего места в массиве
				boardArray[activeFigure.x, activeFigure.y] = null;
				// обновление координат фигуры
				activeFigure.x = clicCellX;
				activeFigure.y = clicCellY;
				// фигура приходит на новое место в массиве
				boardArray[activeFigure.x, activeFigure.y] = activeFigure;
				// перемещение фигуры на новую клетку
				activeFigure.MoveToCell(cellSize, offsetL, offsetB);
				// активная фигура больше не активная
				activeFigure.PaintSprite ();
				activeFigure = null;

				// смена игрока
				if (activeColour == "white") 
				{
					activeColour = "black";
					// красный текст справа
					textYourTurn.color = new Color32 (210, 94, 64, 255);
					textYourTurn.alignment = TextAnchor.MiddleRight;
				} 
				else 
				{
					activeColour = "white";
					// синий текст слева
					textYourTurn.color = new Color32 (79, 123, 159, 255);
					textYourTurn.alignment = TextAnchor.MiddleLeft;
				}

				return;
			}
		}
	}

	private void FigureInitialization(int x, int y)
	{
		Figure prefab = pawnFigure;

		switch (startBoardArray [y, x])
		{
		case 'P':
			prefab = pawnFigure;
			break;
		case 'R':
			prefab = rookFigure;
			break;
		case 'H':
			prefab = horseFigure;
			break;
		case 'B':
			prefab = bishopFigure;
			break;
		case 'Q':
			prefab = queenFigure;
			break;
		case 'K':
			prefab = kingFigure;
			break;
		}

		Figure newFigure = Instantiate (prefab, new Vector2 (offsetL+cellSize/2+x*cellSize, offsetB+cellSize/2+y*cellSize), Quaternion.identity);
		boardArray [x, y] = newFigure;
		// родитель-холст
		//newFigure.GetComponent<RectTransform> ().parent = GetComponent<RectTransform> ();
		newFigure.GetComponent<RectTransform> ().parent = figureCanvas.GetComponent<RectTransform> ();
		newFigure.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize);
		newFigure.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);
		// фигура хранит свои координаты 
		newFigure.x = x;
		newFigure.y = y;

		// фигура хранит свой тип
		newFigure.type = startBoardArray [y, x];

		// если фигура на левой половине поля - белая, на правой - чёрная
		if (x < 4) 
		{
			newFigure.colour = "white";
		} 
		else 
		{
			newFigure.colour = "black";
		}
			
		// покраска фигуры
		newFigure.PaintSprite ();


	}

	void FlashCellUpdate ()
	{
		int x = (int)((Input.mousePosition.x-offsetL) / cellSize);
		int y = (int)((Input.mousePosition.y-offsetB) / cellSize);

		// покраска клеток в шахматном порядке  
		if (x >= 0 && y >= 0 && x < 8 && y < 8) 
		{
			ColourBoard ();

			if (x % 2 != 0 && y % 2 != 0 || x % 2 == 0 && y % 2 == 0) {
				cellArray [x, y].GetComponent<Image> ().color = new Color32 (250, 250, 206, 255);
			} else {
				cellArray [x, y].GetComponent<Image> ().color = new Color32 (206, 197, 170, 255);
			}
		}
	}

	void ColourBoard()
	{
		for (int i = 0; i < 8; i++) 
		{
			for (int j = 0; j < 8; j++) 
			{
				// покраска клеток в шахматном порядке  
				if (i % 2 != 0 && j % 2 != 0 || i % 2 == 0 && j % 2 == 0) 
				{
					cellArray [i, j].GetComponent<Image> ().color = new Color32 (230, 230, 186, 255);
				} 
				else 
				{
					cellArray [i, j].GetComponent<Image> ().color = new Color32 (186, 177, 150, 255);
				}
			}
		}
	}
}