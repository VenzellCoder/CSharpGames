/*
TROUBLE VOIDER - 2048 game by Ven Zell - console version
04/03/2018
*/


/* ИДЕИ ДЛЯ РЕФАКТОРИНГА:
	- переименовать переменные для эфекта волны логотипа
	- закомментировать блок кода эфекта волны логотипа
	- изменить условие конца игры 
*/

using System;
using System.Collections.Generic;
using System.Threading;

namespace MaApp
{
	class MyClass
	{
		static void Main()
		{
			
			
			// console window size
			int windWidth = 80;
			int windHeight = 25;
			// set console window size
			Console.SetWindowSize(windWidth, windHeight);
			// set console buffer size
			Console.SetBufferSize(windWidth, windHeight);
						
			// create main menu			
			Menu main = new Menu();
		}
	}
	
	
	class Menu
	{
		Random random;
		// colours 
		ConsoleColor[] colArray = new ConsoleColor[] {
			ConsoleColor.White, 
			ConsoleColor.Gray,
			ConsoleColor.Gray,
			ConsoleColor.Gray};
		
		// logo
		string[] logoArray = new string[]{
			" ooooooooooo         ooooooo       oooo     oooo        ooooooo    ",
			"ooooooooooooo      ooooooooooo     oooo     oooo      ooooooooooo  ",
			"ooooooooooooo     ooooooooooooo    oooo     oooo     ooooooooooooo ",
			"oooo    ooooo    ooooo     ooooo   oooo     oooo    ooooo     ooooo",
			"oooo    ooooo    oooo       oooo   oooo     oooo    oooo       oooo",
			"       ooooo     oooo       oooo   oooo     oooo    oooo       oooo",
			"      ooooo      oooo       oooo   oooo     oooo     oooo     oooo ",
			"     ooooo       oooo       oooo   ooooooooooooo     oooooooooooo  ",
			"    ooooo        oooo       oooo   ooooooooooooo     ooooooooooooo ",
			"   ooooo         oooo       oooo    oooooooooooo    ooooo     ooooo",
			"  ooooo          oooo       oooo            oooo    oooo       oooo",
			" ooooo           oooo       oooo            oooo    oooo       oooo",
			"ooooo            ooooo     ooooo            oooo    ooooo     ooooo",
			"oooooooooooo      ooooooooooooo             oooo     ooooooooooooo ",
			"oooooooooooo       ooooooooooo              oooo      ooooooooooo  ",
			"oooooooooooo         ooooooo                oooo        ooooooo    ",};
			
		public Menu()
		{	
			random = new Random();
			// Need to rename variables
			int YYYdir = -1;
			int YYYdirNext = -1;
			int[] XXX = new int[20];
			int YYY = 0;
			
			int[] deltaArr = new int[20];
			int[] dirArr = new int[20];
			
			while(true)
			{

				for (int i=0; i<logoArray.Length-1; i++)
					{
						
					YYY += YYYdir;
			
					if (Math.Abs(YYY) > 2)
					{
						if (YYYdir == 0) YYYdir = YYYdirNext;
						else
						{
							YYYdirNext = -YYYdir;
							YYYdir = 0;
						}
					}
					
					XXX[i] = YYY;

					if (Math.Abs(YYY) == 0) Console.ForegroundColor = ConsoleColor.White;
					if (Math.Abs(YYY) == 1) Console.ForegroundColor = ConsoleColor.Gray;
					if (Math.Abs(YYY) == 2) Console.ForegroundColor = ConsoleColor.DarkGray;
					
					Console.SetCursorPosition(5+XXX[i],2+i);				
					//Console.WriteLine(XXX[i]);
					Console.Write(logoArray[i]);
					}
					
				
				Console.SetCursorPosition(15,21);				
				Console.WriteLine("Press 'S' to start a path from monkey tester to CEO");	
					
				Console.ForegroundColor = ConsoleColor.DarkGray;	
				Console.SetCursorPosition(34,19);
				Console.WriteLine("by Ven Zell");	
					
				Thread.Sleep(100);
				
				Console.Clear();
				
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey();

					if (key.Key == ConsoleKey.S)
					{
						Console.Clear();
						Game game = new Game();
					}
				}
			}
		}
	}

	class Game
	{	
		int freeSpaces;
		bool gameOver = false;
		// update movement and addiction 
		bool wasMoved = false;
		bool wasCalculated = false;
		// board array
		int[,] array;
		int score = 0;
		Random random;
		int arraySize = 4;		
		// amount of each number type (on game start we have two "2" tiles)
		int[] amountArray = new int[]{0,0,0,0,0,0,0,0,0,0,2}; 	
		// numbers 
		int[] numbersArray = new int[]{2,4,8,16,32,64,128,256,512,1024,2048};	
		
		// table 
		string[] positionArray = new string[]{
			"2048 |      CEO      | ",
			"1024 |    зам CEO    | ",
			" 512 | фин. директор | ",

			" 256 |    тим лид    | ",
			" 128 |   архитектор  | ",
			"  64 |  разработчик  | ",
			"  32 |  джуниор дев  | ",

			"  16 | ведущий тестер| ",
			"   8 | автоматизатор | ",
			"   4 |    тестер     | ",
			"   2 |  манкитестер  | "};
						
		// tile background colours
		ConsoleColor[] colBackArray = new ConsoleColor[] {
			ConsoleColor.DarkGray, 
			ConsoleColor.White,
			ConsoleColor.DarkBlue,
			ConsoleColor.DarkMagenta,
			ConsoleColor.DarkCyan,
			ConsoleColor.DarkGreen,
			ConsoleColor.DarkYellow,
			ConsoleColor.DarkRed,
			ConsoleColor.Green,
			ConsoleColor.Red,
			ConsoleColor.Cyan};	
			
		// tile number colours
		ConsoleColor[] colFrontArray = new ConsoleColor[] {
			ConsoleColor.White, 
			ConsoleColor.Black,
			ConsoleColor.White,
			ConsoleColor.White,
			ConsoleColor.White,
			ConsoleColor.White,
			ConsoleColor.White,
			ConsoleColor.White,
			ConsoleColor.White,
			ConsoleColor.White,
			ConsoleColor.White};
		
		// game initialisation
		public Game()
		{		
			array = new int[arraySize,arraySize];
			random = new Random();
			
			// set all cells to zero 
			for (int i=0; i<arraySize; i++)
			{
				for (int j=0; j<arraySize; j++)
				{
						array[i,j] = 0;
				}
			}
			// start game with several tiles on board
			array[random.Next(0,arraySize-1),random.Next(0,arraySize-1)] = 1;
			array[random.Next(0,arraySize-1),random.Next(0,arraySize-1)] = 1;		
			// start score
			score = 4;
			// print everything
			Print();
	
			bool buttonWasPressed = false;
	
				
			// game loop
			while(!gameOver)
			{
				// keyboard control
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey();

					if (key.Key == ConsoleKey.LeftArrow)
					{
						MoveNumbers("left");
						CalcNumbers("left");
						MoveNumbers("left");
						buttonWasPressed = true;
					}
					else if (key.Key == ConsoleKey.RightArrow)
					{
						MoveNumbers("right");
						CalcNumbers("right");
						MoveNumbers("right");
						buttonWasPressed = true;
					}
					else if (key.Key == ConsoleKey.UpArrow)
					{
						MoveNumbers("up");
						CalcNumbers("up");
						MoveNumbers("up");
						buttonWasPressed = true;
					}
					else if (key.Key == ConsoleKey.DownArrow)
					{
						MoveNumbers("down");
						CalcNumbers("down");
						MoveNumbers("down");
						buttonWasPressed = true;
					}											
				}
				
				// update after button press
				if (buttonWasPressed)
				{							
					if (wasMoved || wasCalculated)
					{
						if (freeSpaces > 0)
						{
							SetNewNumber();
							AmountOfNumbers();
						}						
					}
					
					buttonWasPressed = false;	
					Clear();
					Print();
					
					wasMoved = false; 
					wasCalculated = false;
				}
				
				// calculate amount of free places
				freeSpaces = AmountOfFreeSpaces();
						
				// if there is no free spaces - game over 
				if (freeSpaces == 0)
				{
					gameOver = true;
				}
			}
			
			// game over screen
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			// cross the board
			for (int f=0; f<20; f++)
			{
				Console.SetCursorPosition(40+f,1+f);
				Console.Write(" ");	
				Thread.Sleep(20);				
			}
			for (int f=0; f<20; f++)
			{
				Console.SetCursorPosition(40+f,21-f);
				Console.Write(" ");	
				Thread.Sleep(20);				
			}
			
			// write: GAME OVER 
			Console.SetCursorPosition(45,20);
			Console.Write("           ");
			Console.SetCursorPosition(45,21);
			Console.Write(" GAME OVER ");
			Console.SetCursorPosition(45,22);
			Console.Write(" (R)estart ");
			Console.SetCursorPosition(45,23);
			Console.Write("           ");
			
			
			// option: RESTART
			while(gameOver)
			{
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey();

					if (key.Key == ConsoleKey.R)
					{
					// default colours
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Gray;
					
					Clear();
							
					// start new game
					Game game = new Game();
					}
				}
			}
			Console.ReadLine();
		}
	
		
		// print tiles, score and table method
		public void Print()
		{		
			// coordinates of top left corner of game board
			int X0 = 8;
			int Y0 = 6;
			// offset between tile centres
			int stepVert = 4;
			int stepHor = 6;
		
			for (int i=0; i<arraySize; i++)
			{
				for (int j=0; j<arraySize; j++)
				{
					if (array[i,j] != 0)
					{
						// set colour
						Console.BackgroundColor = colBackArray[array[i,j]-1];
						Console.ForegroundColor = colFrontArray[array[i,j]-1];
						
						// rectangle tile
						for (int k=0; k<3; k++)
						{
							Console.SetCursorPosition(X0 + j*stepHor - 2, Y0 + i*stepVert - 1 + k);
							Console.Write("     ");
						}
						
						// print number
						Console.SetCursorPosition(X0 + j*stepHor - 1, Y0 + i*stepVert);
						Console.Write(numbersArray[array[i,j] - 1]);
						
						// recover default colours
						Console.BackgroundColor = ConsoleColor.Black;
						Console.ForegroundColor = ConsoleColor.Gray;
					}
					else
					{
						// if there is no tile - print a dot
						Console.SetCursorPosition(X0 + j*stepHor, Y0 + i*stepVert);
						Console.Write(".");
					}
				}
			}
			// stats 
			// offset between QA, dev, business 			
			int gap = 0;
			// coordinate of top left corner of the table
			int tableX0 = 40;
			int tableY0 = 2;
			
			// table title
			Console.SetCursorPosition(tableX0, tableY0);
			Console.Write("         должность      количество");
			Console.SetCursorPosition(tableX0, tableY0+1);
			Console.Write("-----+---------------+---------------");
			Console.SetCursorPosition(tableX0, tableY0+2);
			Console.Write("     |               |");
			
			// feel the table 
			for (int i=0; i<11; i++)
			{
				if (i>2) gap = 1;
				if (i>6) gap = 2;
				
				Console.SetCursorPosition(tableX0, tableY0+i+gap+3);
				Console.Write(positionArray[i]);
				
				// collaborator's faces 
				for (int k=0; k<amountArray[i]; k++)
				{
					// collaborator's faces are coloured with chess pattern 
					if (i%2 == 0)
					{
						if (k%2 == 0) Console.ForegroundColor = ConsoleColor.Gray;
						else Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						if (k%2 != 0) Console.ForegroundColor = ConsoleColor.Gray;
						else Console.ForegroundColor = ConsoleColor.White;
					}
					
					// limit number of faces in one line = 9
					if (amountArray[i] < 10)
					{
						// print a funny face
						Console.Write((char)2 + " ");
					}
				}
				Console.ForegroundColor = ConsoleColor.Gray;
			}
			
			// score
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(tableX0+26, tableY0+20);
			Console.Write("score: " + score);
		}		
		
		// clean screen method
		public void Clear()
		{
			Console.Clear();
		}
		
		// tile movement method
		public void MoveNumbers(string _direction)
		{		
			for (int i=0; i<arraySize; i++)
			{
				bool canMove = true;
								
				while(canMove)
				{
					// if nothing was moved in the "while" loop - stop "while" loop
					canMove = false;	
					
					if (_direction == "left")
					{
						for (int j=1; j<arraySize; j++)
						{
							if (array[i,j] != 0 & array[i,j-1] == 0)
							{
								array[i,j-1] = array[i,j];
								array[i,j] = 0;
								canMove = true;
								wasMoved = true;
							}
						}
					}
					
					if (_direction == "right")
					{
						for (int j=arraySize-2; j>-1; j--)
						{
							if (array[i,j] != 0 & array[i,j+1] == 0)
							{
								array[i,j+1] = array[i,j];
								array[i,j] = 0;
								canMove = true;
								wasMoved = true;
							}
						}
					}
	
					if (_direction == "up")
					{
						for (int j=1; j<arraySize; j++)
						{
							if (array[j,i] != 0 & array[j-1,i] == 0)
							{
								array[j-1,i] = array[j,i];
								array[j,i] = 0;
								canMove = true;
								wasMoved = true;
							}
						}
					}
					
					if (_direction == "down")
					{
						for (int j=arraySize-2; j>-1; j--)
						{
							if (array[j,i] != 0 & array[j+1,i] == 0)
							{
								array[j+1,i] = array[j,i];
								array[j,i] = 0;
								canMove = true;
								wasMoved = true;
							}
						}
					}
				}
			}
		}
		
		// addiction of equal nearby numbers method
		public void CalcNumbers(string _direction)
		{			
			for (int i=0; i<arraySize; i++)
			{					
				if (_direction == "left")
				{
					for (int j=1; j<arraySize; j++)
					{
						if (array[i,j] == array[i,j-1] & array[i,j] != 0)
						{
							array[i,j-1] = 0;
							array[i,j] ++;
							wasCalculated = true;
						}
					}
				}
				
				if (_direction == "right")
				{
					for (int j=arraySize-2; j>-1; j--)
					{
						if (array[i,j] == array[i,j+1]  & array[i,j] != 0)
						{
							array[i,j+1] = 0;
							array[i,j] ++;
							wasCalculated = true;
						}
					}
				}
				
				if (_direction == "up")
				{
					for (int j=1; j<arraySize; j++)
					{
						if (array[j,i] == array[j-1,i] & array[j,i] != 0)
						{
							array[j-1,i] = 0;
							array[j,i] ++;
							wasCalculated = true;
						}
					}
				}
				
				if (_direction == "down")
				{
					for (int j=arraySize-2; j>-1; j--)
					{
						if (array[j,i] == array[j+1,i] & array[j,i] != 0)
						{
							array[j+1,i] = 0;
							array[j,i] ++;
							wasCalculated = true;
						}
					}
				}
			}
		}
		
		// calculate amount of free places method
		public int AmountOfFreeSpaces()
		{
			int amount = 0;
			
			for (int i=0; i<arraySize; i++)
			{
				for (int j=0; j<arraySize; j++)
				{
					if (array[i,j] == 0)
					{
						amount ++;
					}
				}
			}
			return amount;
		}
		
		// set new tiles  
		public void SetNewNumber()
		{
			bool numberWasSet = false;			
			
			// while a tile wasn't set
			while (!numberWasSet)
			{
				// choose random place on he board
				int newX = random.Next(0,arraySize);
				int newY = random.Next(0,arraySize);
				
				// if place is free set a tile
				if (array[newX, newY] == 0)
				{	
					// set tile with number "2" (90%) or "4" (10%)
					if (random.Next(0,100) < 90)
					{
						array[newX, newY] = 1;
						score += 2;
					}
					else
					{
						array[newX, newY] = 2;
						score += 4;
					}
					numberWasSet = true;	
				}
			}		
		}
		
		// calculate amount of each number type method
		public void AmountOfNumbers()
		{
			// clear array with result of calculation
			Array.Clear(amountArray,0,amountArray.Length);
			
			for (int i=0; i<arraySize; i++)
			{
				for (int j=0; j<arraySize; j++)
				{
					if (array[i,j] != 0)
					{
						amountArray[array[i,j]-1] ++;
					}
				}
			}
			// revers the array (it's easier to print from top to bottom)
			Array.Reverse(amountArray);
		}
	}
}