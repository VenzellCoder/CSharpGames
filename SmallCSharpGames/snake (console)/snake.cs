/*
TROUBLE VOIDER - snake game by Ven Zell - console version
14/01/2018
*/


/* ИДЕИ ДЛЯ РЕФАКТОРИНГА:
- реализацию заставки очистки экрана вынести в отдельный метод
- данные для настройки размера экрана вынести в статический класс, убрать "волшебные" числа
- объединить три конструктора line в один
- создавать объект класса Random только один раз для лучшего рандома
*/

using System;
using System.Collections.Generic;
using System.Threading;

namespace HelloWorld
{
	class MyClass
	{		
		static void Main()
		{
		// set console windows size
		Console.SetWindowSize(100,50);
		
		// main menu
		MainMenu mainMenu = new MainMenu();
		
		// start drawing main menu logo
		mainMenu.DrawLogo();
		
		Console.ReadLine();
		}
	}

	class MainMenu
	{
		public MainMenu()
		{
			// console windows size
			int windWidth = 100;
			int windHeight = 50;
			Console.SetBufferSize(windWidth, windHeight);
			
			
			// INTRO
			// list of lines
			List<Snake> lineList = new List<Snake>();
			
			// dot grid move from left
			Snake line1 = new Snake(2,'.', -10, 5+0, 15, 270, "dark gray",0,2);
			Snake line2 = new Snake(2,'.', -11, 5+1, 15, 270, "dark gray",0,2);
			// dot grid move from right
			Snake line3 = new Snake(2,'.', 110, 5+0, 15, 270, "gray",180,2);
			Snake line4 = new Snake(2,'.', 111, 5+1, 15, 270, "gray",180,2);
			
			// add lines to list
			lineList.Add(line1);
			lineList.Add(line2);
			lineList.Add(line3);
			lineList.Add(line4);
			
			// while logo isn't drawn
			bool logoFinished = false;
			while(!logoFinished)
			{
				// move lines
				foreach(Snake i in lineList)
				{
				i.Slide();
				}	
				
				// if lines meet
				if (line1.headDot.x > 105)
				{				
					// logo flashing
					LogoFlashing(0, 150);
					// logo is finished
					logoFinished = true;
				}
				// delay
				Thread.Sleep(20);
				
			}
			
			// logo pulse index
			int pulsarIndex = 0;
			
			// main menu control
			while(true)
			{
				// main menu text
				Console.ForegroundColor = ConsoleColor.White;
				Console.SetCursorPosition(40, 40);
				Console.Write("(P)lay        (E)xit");
				
				// speed of logo flashing
				pulsarIndex = (1-pulsarIndex);
				int timeFlash = 100;
				int timeFlashPause = timeFlash+4*timeFlash*pulsarIndex;
				
				// loog is flashing
				LogoFlashing(timeFlashPause, timeFlash);
				
				// wait for a key press
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey();
					
					// hide written symbols
					Console.ForegroundColor = ConsoleColor.Black;
					
					// option: TUTORIAL
					if (key.Key == ConsoleKey.T)
					{
						// only in DLC
					}
					// option: PLAY GAME
					else if (key.Key == ConsoleKey.P)
					{						
						// clean screen with effects
						Snake outLine1 = new Snake(true,'*', -10, 0, windHeight-1, 270, "dark gray",0,1);
						Snake outLine2 = new Snake(true,' ', -100, 0, windHeight-1, 270, "gray",0,1);						
						do 
						{
						outLine1.Slide();
						outLine2.Slide();
						}	
						while (outLine2.headDot.x < 200);
						
						// preparations for trash grabber
						outLine1 = null;
						outLine2 = null;
						
						// start new game
						Game game = new Game(windWidth, windHeight);
					}
					// option: EXIT
					else if (key.Key == ConsoleKey.E)
					{
						// clean screen with effects
						Snake outLine1 = new Snake(true,'*', -10, 0, windHeight-1, 270, "dark gray",0,1);
						Snake outLine2 = new Snake(true,' ', -100, 0, windHeight-1, 270, "gray",0,1);
						do 
						{
						outLine1.Slide();
						outLine2.Slide();
						}	
						while (outLine2.headDot.x < 200);
						
						// write good bay text
						Console.ForegroundColor = ConsoleColor.White;
						Console.SetCursorPosition(45, 25);
						Console.Write("Good bye!");
						
						// close console in 1 second
						Thread.Sleep(1000);
						Environment.Exit(0);
					}
				}
			}
		}
		
		// draw big logo "TROUBLE AVOIDER"
		public void DrawLogo()
		{
			// logo top right corner
			int logoX0 = 25;
			int logoY0 = 12;
			
			// write: "TROUBLE"
			Console.SetCursorPosition(logoX0, logoY0+0);
			Console.Write("                                                  ");
			Console.SetCursorPosition(logoX0, logoY0+1);
			Console.Write("  OOOOO  OOOO    OOO   O   O  OOOO   O      OOOOO ");
			Console.SetCursorPosition(logoX0, logoY0+2);
			Console.Write("    O    O   O  O   O  O   O  O   O  O      O     ");
			Console.SetCursorPosition(logoX0, logoY0+3);
			Console.Write("    O    OOOO   O   O  O   O  OOOO   O      OOO   ");
			Console.SetCursorPosition(logoX0, logoY0+4);
			Console.Write("    O    O   O  O   O  O   O  O   O  O      O     ");
			Console.SetCursorPosition(logoX0, logoY0+5);
			Console.Write("    O    O   O   OOO    OOO   OOOO   OOOOO  OOOOO ");
			Console.SetCursorPosition(logoX0, logoY0+6);
			Console.Write("                                                  ");
			
			// write: "AVOIDER"
			Console.SetCursorPosition(logoX0, logoY0+7);
			Console.Write("   OOO   O   O   OOO    OOO   OOOO   OOOOO  OOOO  ");
			Console.SetCursorPosition(logoX0, logoY0+8);
			Console.Write("  O   O  O   O  O   O    O    O   O  O      O   O ");
			Console.SetCursorPosition(logoX0, logoY0+9);
			Console.Write("  OOOOO  O   O  O   O    O    O   O  OOO    OOOO  ");
			Console.SetCursorPosition(logoX0, logoY0+10);
			Console.Write("  O   O   O O   O   O    O    O   O  O      O   O ");
			Console.SetCursorPosition(logoX0, logoY0+11);
			Console.Write("  O   O    O     OOO    OOO   OOOO   OOOOO  O   O ");
			Console.SetCursorPosition(logoX0, logoY0+12);
			Console.Write("                                                  ");
			// write: "Snake game by Ven Zell"
			Console.SetCursorPosition(logoX0, logoY0+13);
			Console.Write("               Snake game by Ven Zell              ");
			Console.SetCursorPosition(logoX0, logoY0+14);
			Console.Write("                                                  ");
		}
		                            
		// logo flashing method
		public void LogoFlashing(int _timeFlashPause, int _timeFlash)
		{
				Thread.Sleep(_timeFlashPause);
				Console.ForegroundColor = ConsoleColor.White;
				DrawLogo();
				Thread.Sleep(_timeFlash);
				Console.ForegroundColor = ConsoleColor.Gray;
				DrawLogo();
				Thread.Sleep(_timeFlash);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				DrawLogo();
				Thread.Sleep(_timeFlash);
				Console.ForegroundColor = ConsoleColor.Gray;
				DrawLogo();
				Thread.Sleep(_timeFlash);
				Console.ForegroundColor = ConsoleColor.White;
				DrawLogo();
		}
	}	
		
	class Game
	{
	int windWidth;
	int windHeight;
	bool paused = false;
				
		public Game(int _windWidth, int _windHeight)
		{
			// windows console size
			windWidth = _windWidth;
			windHeight = _windHeight;
						
			// time
			int time = 0;
								
			// collision detector				
			Collider collider = new Collider();
			
			// food spawner
			FoodSpawner foodSpawner = new FoodSpawner(collider);
			
			ProblemSpawner problemSpawner = new ProblemSpawner(collider);
			
			// the snake
			Snake myLine = new Snake('O', 0, 5, 5, 0, "white", collider);
			 		 
			bool gameOver = false;
			bool buttonWasPressed = false;
			
			// GAME LOOP
			while(!gameOver)
			{
				// snake control
				if (Console.KeyAvailable & !buttonWasPressed)
				{
					ConsoleKeyInfo key = Console.ReadKey();
									
					if (!paused)
					{
						if (key.Key == ConsoleKey.LeftArrow & myLine.direction != 0)
						{
							myLine.direction = 180;
							buttonWasPressed = true;
						}
						else if (key.Key == ConsoleKey.RightArrow & myLine.direction != 180)
						{
							myLine.direction = 0;
							buttonWasPressed = true;
						}
						else if (key.Key == ConsoleKey.UpArrow & myLine.direction != 270)
						{
							myLine.direction = 90;
							buttonWasPressed = true;
						}
						else if (key.Key == ConsoleKey.DownArrow & myLine.direction != 90)
						{
							myLine.direction = 270;
							buttonWasPressed = true;
						}
						
						if (key.Key == ConsoleKey.P)
						{
							paused = true;
							DrawPauseMenu();
						}
					}
					else
					{
						if (key.Key == ConsoleKey.P)
						{
							paused = false;
							ClearPauseMenu();
						}	
					}						
				}
				
				if (!paused)
				{
					// delay, add time
					Thread.Sleep(5);
					time ++;
					
					// snake make a step
					if (time % myLine.speed == 0)
					{					
						myLine.Move();
						buttonWasPressed = false;
						myLine.Draw();
						collider.collision(myLine);
					}
					
					// game over condition
					if (myLine.HP == 0)
					{
						gameOver = true;
						myLine.EpicChangeColor("dark gray", 'x');
					}

					
					// food spawn condition
					if (time % foodSpawner.speed == 0)
					{
						foodSpawner.spawnNewTester(windWidth, windHeight);
					}
					
					// blocks spawn condition
					if (time % problemSpawner.speed == 0)
					{
					problemSpawner.spawnNewProblem(windWidth, windHeight);
					}
					
					// write score in the top left corner
					Console.SetCursorPosition(0,0);
					Console.ForegroundColor = ConsoleColor.White;
					int num = myLine.dotList.Count-5; // start snake size = 5
					Console.Write("score: " + num);
					
					// hide written letters
					Console.ForegroundColor = ConsoleColor.Black;
				}
			}
			
			// GAME OVER MENU
			int xGameOver = 41;
			int yGameOver = 20;			
			// draw white rectangle in the centre 
			Console.SetCursorPosition(xGameOver,yGameOver);
			Console.BackgroundColor = ConsoleColor.White;
			for (int i = 0; i<11; i++)
			{
				Console.SetCursorPosition(xGameOver,yGameOver+i);
				Console.Write("                   ");
			}
			
			// write : "ТЫ ПРОИГРАЛ" // нужно сделать выбор языка  
			Console.ForegroundColor = ConsoleColor.Black;
			Console.SetCursorPosition(xGameOver+4,yGameOver+1);
			Console.Write("ТЫ ПРОИГРАЛ");
			// write: "причина" 
			Console.SetCursorPosition(xGameOver+6,yGameOver+3);
			Console.Write("причина:");
			// write: problem name  
			Console.SetCursorPosition(xGameOver+10-myLine.problemName.Length/2,yGameOver+4);
			Console.Write(myLine.problemName);
			// write: "счёт: "  
			Console.SetCursorPosition(xGameOver+6,yGameOver+6);
			Console.Write("счёт: " + (myLine.lenght-5));
			// write: "(R)estart  (M)ain"  
			Console.SetCursorPosition(xGameOver,yGameOver+9);
			Console.Write(" (R)estart  (M)ain");
			
			// clear score text in the top left corner 
			Console.SetCursorPosition(0,0);
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write("               ");
			
			
			
			while(true)
			{
				// wait for a key press 
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey();
					// option: RESTART
					if (key.Key == ConsoleKey.R)
					{						
						// clean screen with effect
						Snake outLine1 = new Snake(true,'*', -10, 0, windHeight-1, 270, "dark gray",0,1);
						Snake outLine2 = new Snake(true,' ', -100, 0, windHeight-1, 270, "gray",0,1);
						
						do 
						{
						outLine1.Slide();
						outLine2.Slide();
						}	
						while (outLine2.headDot.x < 200);
						
						// preporations for trash grabber
						outLine1 = null;
						outLine2 = null;
						
						// start new game
						Game game = new Game(windWidth, windHeight);
					}
					// option: RESTART
					else if (key.Key == ConsoleKey.M)
					{
						// Заставка очистки экрана
						Snake outLine1 = new Snake(true,'*', -10, 0, windHeight-1, 270, "dark gray",0,1);
						Snake outLine2 = new Snake(true,' ', -100, 0, windHeight-1, 270, "gray",0,1);
						
						// Двигаем линии
						do 
						{
						outLine1.Slide();
						outLine2.Slide();
						}	
						while (outLine2.headDot.x < 200);
						
						
						// Главное меню
						MainMenu mainMenu = new MainMenu();
						mainMenu.DrawLogo();
					}
				}
			}
		}
		
		// turn ON pause menu
		public void DrawPauseMenu()
		{
			Console.SetCursorPosition(40,0);
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Write("      ПАУЗА     ");
		}
		
		// turn OFF pause menu
		public void ClearPauseMenu()
		{
			Console.SetCursorPosition(0,0);
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("                                                                       ");
		}
	}
	
	class Dot
	{
		public int x;
		public int y;
		public char sym;
		public string color;
		public int feedValue;
		public string problemName;
		
		// initialization
		public Dot(int _x, int _y, char _sym, string _color, int _feedValue=0, string _problemName = " ")
		{
			x = _x;
			y = _y;
			sym = _sym;
			color = _color;
			feedValue = _feedValue;
			problemName = _problemName;
		}

	
		public void Move(int offset, int direction)
		{			
			if (direction == 0)
			{
				x += offset;
			}
			else if (direction == 180)
			{
				x -= offset;
			}
			else if (direction == 90)
			{
				y -= offset;
			}
			else if (direction == 270)
			{
				y += offset;
			}
		}
		
		public void Draw()
		{
			if (x > 0 & x < 100 & y > 0 & y < 50)
			{
				if (color == "white") Console.ForegroundColor = ConsoleColor.White;
				if (color == "red") Console.ForegroundColor = ConsoleColor.Red;
				if (color == "green") Console.ForegroundColor = ConsoleColor.Green;
				if (color == "yellow") Console.ForegroundColor = ConsoleColor.Yellow;
				if (color == "gray") Console.ForegroundColor = ConsoleColor.Gray;
				if (color == "dark gray") Console.ForegroundColor = ConsoleColor.DarkGray;
				
				Console.SetCursorPosition(x,y);
				Console.Write(sym);
			}
		}
		
		public void Clear()
		{
			if (x > 0 & x < 100 & y > 0 & y < 50)
			{
				Console.SetCursorPosition(x,y);
				Console.Write(' ');
			}
		}
	}
	
	// line consists of dots
	class Line
	{
		public List<Dot> dotList;
		public int lenght;
		public int direction;
		
		public void Draw()
		{
			foreach(Dot i in dotList)
			{
				i.Draw();			
			}
		}
		
		// change symbols of line's dots
		public void ChangeSym(char newSym)
		{
			foreach(Dot i in dotList)
			{
				i.sym = newSym;			
			}
		}	
	}
	
	class Snake : Line
	{
		// start speed = 10, max speed = 1
		public int speed = 10; 
		public int aimLenght;
		Dot tailDot;
		public Dot headDot;
		public int slideDirection;
		public int slideSpeed;
		Collider collider;
		public int HP = 1;
		public string problemName;
		

		// standard line of 1 symbol 
		public Snake(char _sym, int _x0, int _y0, int _lenght, int _direction, string _color, Collider _collider)
		{
			direction = _direction;
			lenght = _lenght;
			aimLenght = _lenght;
			collider = _collider;
			
			dotList = new List<Dot>();
			
			for (int i=0; i<lenght; i++)
			{
				Dot newDot = new Dot(_x0, _y0, _sym, _color);
				newDot.Move(i,direction);
				dotList.Add(newDot);
			}
		}
		

		
		// standard line of 2 symbols 
		public Snake(int _gap, char _sym, int _x0, int _y0, int _lenght, int _direction, string _color, int _slideDirection, int _slideSpeed)
		{
			direction = _direction;
			lenght = _lenght;
			aimLenght = _lenght;
			slideDirection = _slideDirection;
			slideSpeed = _slideSpeed;
			
			dotList = new List<Dot>();
			
			for (int i=0; i<lenght; i++)
			{
				
				Dot newDot = new Dot(_x0, _y0, _sym, _color);
				newDot.Move(i*_gap,direction);
				dotList.Add(newDot);
				
			}
		}
		
		// standard line of 2 symbol type, diagonal(!!!)
		public Snake(bool _diag, char _sym, int _x0, int _y0, int _lenght, int _direction, string _color, int _slideDirection, int _slideSpeed)
		{
			direction = _direction;
			lenght = _lenght;
			aimLenght = _lenght;
			slideDirection = _slideDirection;
			slideSpeed = _slideSpeed;
			
			dotList = new List<Dot>();
			
			for (int i=0; i<lenght; i++)
			{
				
				Dot newDot = new Dot(_x0, _y0, _sym, _color);
				newDot.Move(i,180);
				newDot.Move(i,270);
				dotList.Add(newDot);
				
			}
		}
		
		public void Move()
		{
			// find head
			headDot = dotList[lenght-1];
			
			// find tail, delete it if doesn't eat
			if (lenght == aimLenght)
			{
				tailDot = dotList[0];
				tailDot.Clear();
				dotList.Remove(tailDot);
			}
			else
			{
				lenght++;
			}

			// now this dot is a new head
			Dot newHeadDot = new Dot(headDot.x,headDot.y,headDot.sym,headDot.color);
			newHeadDot.Move(1,direction);
			dotList.Add(newHeadDot);
			headDot = newHeadDot;
			
			
			// if snake is out of window frame
			if (headDot.x > 99)
			{
				headDot.x = 1;
			}
			if (headDot.x < 1)
			{
				headDot.x = 99;
			}
			if (headDot.y > 49)
			{
				headDot.y = 1;
			}
			if (headDot.y < 1)
			{
				headDot.y = 49;
			}
		}
		
		public void Slide()
		{
			// find the head 
			headDot = dotList[lenght-1];
			
			foreach(Dot i in dotList)
			{
				//i.Clear();
				i.Move(slideSpeed,slideDirection);	
				i.Draw();
			}
		}
		
		// the longer snake the faster it is
		public void ChangeSpeed()
		{
			if (lenght > 10)speed = 9;
			if (lenght > 20)speed = 8;
			if (lenght > 30)speed = 7;
			if (lenght > 40)speed = 6;
			if (lenght > 50)speed = 5;
			if (lenght > 60)speed = 4;
			if (lenght > 80)speed = 3;
			if (lenght > 100)speed = 2;
			if (lenght > 130)speed = 1;
		}
		
		// after snake death it's dots change colour and symbol one after one
		public void EpicChangeColor(string _newColor, char _newSym)
		{
			dotList.Reverse();
			
			foreach(Dot i in dotList)
			{
				i.sym = _newSym;
				i.color = _newColor;
				i.Draw();
				Thread.Sleep(25);
			}	
		}
	}
	
	class Collider
	{
		public List<Dot> problemList;
		public List<Dot> foodList;
		
		public Collider()
		{
			problemList = new List<Dot>();
			foodList = new List<Dot>();
		}
		
		public void collision(Snake _line)
		{		
			// collision check "snake-food"
			foreach (Dot i in foodList)
			{
				if (_line.headDot.x == i.x & _line.headDot.y == i.y)
				{
					_line.aimLenght += i.feedValue;

					// may be it's time to change game speed
					_line.ChangeSpeed();
					
					// remove food from list
					foodList.Remove(i);
					
					break;
				}
			}
			
			// collision check "snake-block"
			foreach (Dot i in problemList)
			{
				if (_line.headDot.x == i.x & _line.headDot.y == i.y)
				{
					_line.problemName = i.problemName;
					_line.HP = 0;				
				}
			}
				
			// collision check "snake-snake"
			// remove snake head from list otherwise it will collide with itself
				_line.dotList.Remove(_line.headDot);
			
			foreach(Dot i in _line.dotList)
			{
				// collide with itself
				if (_line.headDot.x == i.x & _line.headDot.y == i.y)
				{
					_line.problemName = "ты сам";
					_line.HP = 0;				
				}
			}
			// add head back
			_line.dotList.Add(_line.headDot);
		}
	}
	
	class FoodSpawner
	{
		public int speed = 100;
		Collider collider;
		
		public FoodSpawner(Collider _collider)
		{
			collider = _collider;
		}
		
		public void spawnNewTester(int xMax, int yMax)
		{
			Random random = new Random();
			int x = random.Next(0+1,xMax-1);
			int y = random.Next(0+1,yMax-1);
			
			// food value
			// 1 - small and common, 9 - big and rare
			int type = 1;
			int typeRandom = random.Next(0,100);
			if (typeRandom > 60) type = 2;
			if (typeRandom > 75) type = 3;
			if (typeRandom > 80) type = 4;
			if (typeRandom > 85) type = 5;
			if (typeRandom > 95) type = 7;
			if (typeRandom > 98) type = 9;
					
			// dot needs a symbol
			// convertation "int-->char"
			int i = 48+type;
			char sym = (char)i;
			
			// create new food
			Dot food = new Dot(x,y,sym,"gray",type);	
			collider.foodList.Add(food);	
			food.Draw();			
		}	
	}
	
	class ProblemSpawner
	{
		public int speed = 200;
		Collider collider;
		string[] hollyDays = new string[]{"долги",
										"стресс",
										"работа",
										"кредит",
										"отношения",
										"дети",
										"хамы",
										"алкоголизм",
										"увольнение",
										"кризис",
										"депрессия",
										"кариес",
										"невезение",
										"агрессия",
										"апатия",
										"развод",
										"дефолт",
										"пустота"};
		int vertical;
		
		public ProblemSpawner(Collider _collider)
		{
			collider = _collider;
		}
		
		public void spawnNewProblem(int xMax, int yMax)
		{
			Random random = new Random();
			// randomly choose problem name for block
			int holIndex = random.Next(0, hollyDays.Length-1);
			string text = hollyDays[holIndex];
			// block coordinate correction, otherwise window boundaries will cut words			
			int x = random.Next(0+1,xMax-1-text.Length);
			int y = random.Next(0+1,yMax-1-text.Length);
			vertical = random.Next(0,2);
			
			
			
			for (int i=0; i<text.Length; i++) 
			{
				//string stringLetter = text[i];
				//char charLetter = stringLetter[0];
				Dot block = new Dot(x+i*vertical,y+i*(1-vertical),text[i],"dark gray", 0, text);	
				collider.problemList.Add(block);	
				
				block.Draw();
			}
		}	
	}	
}
