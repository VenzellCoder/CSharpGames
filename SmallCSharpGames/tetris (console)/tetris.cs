/*
TETRIS by Ven Zell
24.03.2018 
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
			// set console window size
			Console.SetWindowSize(Level.windWidth, Level.windHeight);
		
			// set console buffer size
			Console.SetBufferSize(Level.windWidth, Level.windHeight);
					
			// new game 
			Game game = new Game();
		}
	}
	
	static class Level
	{
		// level size/coordinates info
		public static int windWidth = 36; 
		public static int windHeight = 30;
		public static int figureStartX = 4;
		public static int figureStartY = 0;
		public static int horLineX = 20;
		public static int vertLineY = 5;
		public static int scoreX = 22;
		public static int scoreY = 2;
	}
	
	class Game
	{
		// timing
		int stepTimeNormal = 70;
		int stepTime = 0;
		// game progression
		int level = 1;
		int score = 0;
		
		// randomisator
		Random random;
		Figure currentBlock;
		Figure nextBlock;
		
		bool gameOver = false;
				
		// classic blocks
	    int[,,] blockClassicArray = new int[,,]{{{0,1,0,0},
										   {0,1,0,0},
										   {0,1,1,0},
			 							   {0,0,0,0}},
									
										  {{0,1,0,0},
										   {0,1,0,0},
										   {1,1,0,0},
										   {0,0,0,0}},
										  
										  {{0,1,0,0},
										   {0,1,0,0},
										   {0,1,0,0},
										   {0,1,0,0}},
										  
										  {{0,1,0,0},
										   {0,1,1,0},
										   {0,0,1,0},
										   {0,0,0,0}},
										   
										  {{0,0,1,0},
										   {0,1,1,0},
										   {0,1,0,0},
										   {0,0,0,0}},
										   
										  {{0,1,0,0},
										   {0,1,1,0},
										   {0,1,0,0},
										   {0,0,0,0}},
										  
										  {{0,0,0,0},
										   {0,1,1,0},
										   {0,1,1,0},
										   {0,0,0,0}}};
					
		// modern blocks
		int[,,] blockModernArray = new int[,,]{{{0,1,0,0},
										   {0,1,0,0},
										   {0,1,1,0}, 
										   {0,1,0,0}},
									
										  {{1,1,0,0},
										   {0,1,0,0},
										   {0,1,1,0},
										   {0,0,0,0}},
										  
										  {{1,1,1,0},
										   {0,1,0,0},
										   {0,1,0,0},
										   {0,0,0,0}},
										  
										  {{0,1,1,0},
										   {0,1,1,0},
										   {0,0,1,0},
										   {0,0,0,0}},
										   
										  {{0,0,1,0},
										   {0,1,1,0},
										   {0,1,0,0},
										   {0,1,0,0}},

										  {{1,0,0,0},
										   {1,0,0,0},
										   {1,1,1,0},
										   {0,0,0,0}},
										  
										  {{0,1,0,0},
										   {1,1,1,0},
										   {0,1,0,0},
										   {0,0,0,0}},
										  
										  {{1,1,0,0},
										   {0,1,1,0},
										   {0,0,1,1},
										   {0,0,0,0}},
										   
										  {{0,0,0,0},
										   {0,1,1,0},
										   {1,1,0,0},
										   {0,1,0,0}},
										   
										  {{0,0,0,0},
										   {0,1,1,0},
										   {1,1,0,0},
										   {0,1,0,0}},	
										   
										  {{0,0,0,0},
										   {0,1,0,1},
										   {0,1,1,1},
										   {0,0,0,0}}};
		
		// chosen array of blocks 
		int[,,] blocksArray;
		
		// active block
		Figure fig;
		
		// collider
		Collider collider;
		
		
		public Game()
		{	
			blocksArray = blockClassicArray;
		
			// collider controll collisions and destroy full lines 
			collider = new Collider();
			
			// randomisation
			random = new Random();
			
			// create first block
			currentBlock = CreateNewBlock();
			currentBlock.Draw();
			
			// create next block
			nextBlock = CreateNewBlock();
			nextBlock.Move(8,0);
			nextBlock.Draw();
			

			// timing
			int time = 0;
			stepTime = stepTimeNormal;

			//set colour of GUI
			Console.ForegroundColor = ConsoleColor.White;
			
			//horisontal line
			for (int i=0; i<Level.windHeight; i++)
			{
				Console.SetCursorPosition(Level.horLineX,i);
				Console.Write('|');
			}
			
			//vertical line
			for (int i=Level.horLineX+1; i<Level.windWidth; i++)
			{
				Console.SetCursorPosition(i,Level.vertLineY);
				Console.Write('-');
			}
			
			// draw score and level
			DrawScore();
			
			// main game loop
			while(!gameOver)
			{
				// buttons controll 
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey();

					// move left
					if (key.Key == ConsoleKey.LeftArrow)
					{
						if (currentBlock.MoveCollisionCheck(-1))
						{
							currentBlock.Move(-1,0);
							currentBlock.Draw();
						}
					}
					// move right
					if (key.Key == ConsoleKey.RightArrow)
					{
						if (currentBlock.MoveCollisionCheck(+1))
						{
							currentBlock.Move(+1,0);
							currentBlock.Draw();
						}
					}
					// rotate 
					if (key.Key == ConsoleKey.UpArrow)
					{
						currentBlock.Rotate();
						currentBlock.Draw();
					}
					// drop down
					if (key.Key == ConsoleKey.DownArrow)
					{
						stepTime = 1;
					}
				}
				// game step
				if (time % stepTime == 0)
				{	
					// check landing of droped figure
					bool landed = currentBlock.LandingCheck();
					
					if (!landed)
					{
						// block is falling
						currentBlock.Move(0,1);
						currentBlock.Draw();
					}
					else
					{
						// block was landed
						for (int R=0; R<26; R++)
						{
							if (collider.deletFullRow())
							{					
								score += 100*level;
													
								// each 500 points level and speed grow
								if (score % 500 == 0)
								{
									level ++;
									stepTimeNormal -= 10;
									stepTime = stepTimeNormal;
								}
								
								// update score and level text
								DrawScore();
							}
						}
						// return to normal falling speed
						stepTime = stepTimeNormal;
						
						// create first block
						currentBlock = nextBlock;
						currentBlock.Move(-8,0);
						if (currentBlock.LandingCheck())
						{
							gameOver = true;
						}
						
						// create next block
						//nextBlock.Clear();
						nextBlock = CreateNewBlock();
						nextBlock.Move(8,0);
						
						// draw new block after move from "next" window
						currentBlock.Draw();
					}
				}
				// delay
				Thread.Sleep(5);
				time ++;
			}
			
			// game over screen
			// randomly fill screen with symbols 
			char[] chars = new char[]{'@','#','$','%','&','*','/','+','F'};
			int charNum = 0;
			int charNumMax = 200;
			Console.ForegroundColor = ConsoleColor.Red;
			
			while(charNum < charNumMax)
			{
				Console.SetCursorPosition(random.Next(0,Level.windWidth), random.Next(0,Level.windHeight-1));
				Console.Write(chars[random.Next(0,8)]);
				// 10% of writing phrase "game over"
				if (random.Next(0,100) > 90)
				{
					Console.SetCursorPosition(random.Next(0,Level.windWidth-10), random.Next(0,Level.windHeight));
					Console.Write("GAME OVER");
				}
				// delay between symbols
				Thread.Sleep(5);	
				charNum ++;
			}

			// close console window
			Thread.Sleep(2000);
			Environment.Exit(0);
		}
		
		void DrawScore()
		{
			Console.SetCursorPosition(Level.scoreX, Level.vertLineY+2);
			Console.Write("Score: " + score);
			
			Console.SetCursorPosition(Level.scoreX, Level.vertLineY+3);
			Console.Write("Level: " + level);
		}
		
		Figure CreateNewBlock()
		{
			// method returns this new block
			Figure b;
			// random type of new block
			int newBlockType = random.Next(0,blocksArray.GetLength(0));
			// create new block 
			b = new Figure(newBlockType,blocksArray,Level.figureStartX,Level.figureStartY,collider);
		
			return b;
		}
	}
		
	class Dot
	{
		public int x;
		public int y;

		public Dot(int _x, int _y)
		{
			x = _x;
			y = _y; 
		}
		
		// draw dot
		public void Draw(string _col)
		{
			// falling block
			if (_col == "active") Console.BackgroundColor = ConsoleColor.White;
			// bottom platform (draw in debug mode)
			if (_col == "platform") Console.BackgroundColor = ConsoleColor.DarkGray;
			// fallen blocks
			if (_col == "solid") Console.BackgroundColor = ConsoleColor.DarkGray;
			// for cleaning 
			if (_col == "empty") Console.BackgroundColor = ConsoleColor.Black;
			
			for (int i=0; i<2; i++)
			{
				for (int j=0; j<1; j++)
				{
					Console.SetCursorPosition(x*2+i,y*1+j);
					Console.Write(" ");
				}
			}
			// return start colour and position
			Console.BackgroundColor = ConsoleColor.Black;
			Console.SetCursorPosition(0,0);			
		}
		
		// clear dot place
		public void Clear()
		{
			Draw("empty");
		}
	}
	
	class Collider
	{
		// grid for handling of full lines
		public int[,] landedGrid;
		// list contains dots of all lended blocks
		public List<Dot> landedDotsList;
		
		public Collider()
		{
			landedDotsList = new List<Dot>();
			landedGrid = new int[10,30];
			
			// create bottom platform
			for (int i=0; i<Level.horLineX/2; i++)
			{
				Dot dot = new Dot(i,Level.windHeight);
				landedDotsList.Add(dot);
				// Debug draw platform
				//dot.Draw("platform");
			}
		}
		
		// does this dot collide with another one?
		public bool CheckCollision(int _x, int _y)
		{
			foreach(Dot i in landedDotsList)
			{				
				if (i.x == _x & i.y == _y)
				{
					return true;	
				}
			}
			return false;
		}
		
		// find full lines, delete dots, put down others lines
		public bool deletFullRow()
		{
			// a line was deleted
			bool rowWasDelete = false;	
			
			// dots in the line 
			int summRow = 0;

			// go throw all lines 
			for (int j=29; j>1; j--)
			{
				if (rowWasDelete)
				{
					for (int k=0; k<10; k++)
					{
						foreach(Dot dot in landedDotsList)
						{				
							if (dot.x == k & dot.y == j)
							{
								landedGrid[k,j] = 0;
								landedGrid[k,j+1] = 1;
								
								dot.Clear();
								dot.y ++;
								dot.Draw("solid");
								break;
							}
						}
					}
				}
				
				if (!rowWasDelete)
				{
					summRow = 0;
					
					for (int i=0; i<10; i++)
					{
						summRow += landedGrid[i,j];
					}
					// it is a full line 
					if (summRow == 10)
					{
						rowWasDelete = true;
						
						for (int k=0; k<10; k++)
						{
							landedGrid[k,j] = 0;
							
							foreach(Dot dot in landedDotsList)
							{				
								if (dot.x == k & dot.y == j)
								{
									dot.Clear();
									
									landedDotsList.Remove(dot);
									break;
								}
							}
						}
					}
				}

			}
			return rowWasDelete;
		}	
	}
	
	class Figure
	{
		public int x;
		public int y;
		// list for dots in a block
		List<Dot> dotList;
		// list for block rotation 
		List<Dot> dotTempList;
		bool landed = false;
		Collider collider;
		int type;

		//blocks array
		int[,,] fig;

	
				
		public Figure(int _type, int[,,] _fig, int _x, int _y, Collider _collider)
		{
			type = _type;
			fig = _fig;
			x = _x;
			y = _y;
			collider = _collider;
			
			Random random = new Random();

			dotList = new List<Dot>();
			dotTempList = new List<Dot>();
			
			dotList = DotInitialisation(fig);
		}
		
		// array to list convertation
		public List<Dot> DotInitialisation(int[,,] _array)
		{
			List<Dot> listReturn = new List<Dot>();
			
			for (int i=0; i<4; i++)
			{
				for (int j=0; j<4; j++)
				{
					if (_array[type,i,j] == 1)
					{
						Dot dot = new Dot(x+i,y+j);
						listReturn.Add(dot);
					}
				}
			}
			return listReturn;
		}
		
		// draw a block
		public void Draw()
		{
			foreach(Dot i in dotList)
			{
				i.Draw("active");
			}
		}
		
		// check if there is a landed dot in a certain place (for e.g. under dot when it falls)
		public bool Collision(List<Dot> _dotList, int _deltaX, int _deltaY)
		{
			bool wasCollide = false;
			
			foreach (Dot figDot in dotList)
			{
				foreach(Dot landDot in collider.landedDotsList)
				{	
					// dots coordinates + offset
					if (figDot.x+_deltaX == landDot.x & figDot.y+_deltaY == landDot.y)
					{
						wasCollide = true;
						break;
					}
				}
			}			
			return wasCollide;		
		}
		
		
		// check if there is a land or lended dot under active block
		public bool LandingCheck()
		{		
			// there is obstacle under active block
			if (Collision(dotList, 0, +1))
			{
				landed = true;
			}
			
			// block was lended
			if (landed)
			{
				foreach (Dot i in dotList)
				{
					// now dots of active block are lended dots
					collider.landedDotsList.Add(i);
					// update level grid
					collider.landedGrid[i.x,i.y] = 1;
					// change colour of landed block
					i.Draw("solid");
				}
			}
			return landed;
		}
		
		// check obstacles on the left and right
		public bool MoveCollisionCheck(int _deltaX)
		{		
			// you can move the block left and right
			bool canMove = true;

			foreach (Dot figDot in dotList)
			{
				foreach(Dot landDot in collider.landedDotsList)
				{				
					// there is an obstacle
					if (figDot.x + _deltaX == landDot.x & figDot.y == landDot.y)
					{
						canMove = false;
						break;
					}
				}
				// block can't go beyond the borders
				if ((figDot.x + _deltaX < 0) || (figDot.x + _deltaX > 9))
				{
					canMove = false;
					break;
				}
			}
			return canMove;
		}

		// move block
		public void Move(int _deltaX, int _deltaY)
		{
			x += _deltaX;
			y += _deltaY;
			
			// move and redraw each dot in the block
			foreach (Dot i in dotList)
			{
				i.Clear();
				i.x += _deltaX;
				i.y += _deltaY;
				i.Draw("active");
			}
		}
		
		// block rotation
		public void Rotate()
		{
			// array after rotation
			int[,,] figAfterRot = new int[20,4,4];
			
			for (int i=0; i<4; ++i)
			{
				for (int j=0; j<4; ++j)
				{
					// math magic rotates array
					figAfterRot[type,i,j] = fig[type, 4-j-1, i];
				}					
			}
			// list of dots in rotated block
			dotTempList = DotInitialisation(figAfterRot);
			
			// check if rotation is successfull
			// if block meats obstacle or go beyond the borders = false
			// standard = true
			bool rotIsSucsesfull = true;
			
			foreach (Dot figDot in dotTempList)
			{
				if ((figDot.x < 0) || (figDot.x > 9))
					{
						rotIsSucsesfull = false;
						break;
					}
				foreach(Dot landDot in collider.landedDotsList)
				{				
					if ((figDot.x == landDot.x & figDot.y == landDot.y))
					{
						rotIsSucsesfull = false;
						break;
					}
				}
			}			
			
			// if rotation is successfull acces all changes
			if (rotIsSucsesfull)
			{			
				// change block
				fig = figAfterRot;
				
				// clear all dots
					foreach (Dot i in dotList)
					{
						i.Clear();
					}
				// clear dot list
				dotList.Clear();
				
				// add add new dots in list
				foreach (Dot i in dotTempList)
					{
						dotList.Add(i);
					}
				// clear buffer list
				dotTempList.Clear();
			}	
			
			else
			{
				// clear buffer list
				dotTempList.Clear();
			}
		}	
	}
}