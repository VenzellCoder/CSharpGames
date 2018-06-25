/*
MINESWEEPER - game version by Ven Zell
20/04/2018
*/

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

/*
     size  mines
easy  10    10
med   15    30
hard  25    70

*/

namespace MaApp
{
	// класс кнопок - клеток
	class MyButton : Button
	{
		public int x;
		public int y;
	}
	
	// классоконного приложения 
	class Simple : Form
	{
		int flagSet = 0;
		bool gameOver = false;
		
		// игровое поле
		int size = 10; 
		int[,] arrayType;
		string[,] arrayState;
		MyButton[,] arratButton;
				
		// размер тайлов
		int tileSize = 30;
		
		// количество бомб
		int bombNum = 0;
		int bombNumMax = 10;
		
		// Фразы при выигрыше
		string[] phraseWin = new string[] {"Молодец!", "Умница!", "Так держать!", "Вот это да!", "Поздравляем!"}; 
		
		// конструктор фраз при взрыве
		string[] phrase0 = new string[] {"Бабах!", "БУМ!", "Быдыщь!", "БАХ!"};
		string[] phrase1 = new string[] {"Левую ногу", "Правую ногу", "Ноги", "Большой палец ноги", "Пальцы", "Мизинец", "Уши", "Нос", "Руку", "Руки", "Печёнку", "Силизёнку", "Зубы", "Рёбра", "Сапёрную лапату", "Ботинки", "Сапёрную каску", "Дырявый носок", "Пряжку от ремня"};
		string[] phrase2 = new string[] {"в яме", "на дереве", "на ветке дерева", "в луже", "в кустах", "у воробья в клюве", "у медведя в зубах", "под бревном", "у хомяка за щекой", "в дупле дерева", "у грибника", "в ручье","","","","",""};
		string[] phrase3 = new string[] {"в соседнем лесу", "в ближайшем болоте", "в соседней деревне", "в 10 км от взрыва", "около соседнего города", "у пионерлагеря", "на берегу реки"};
		string[] phrase4 = new string[] {"проходящий мимо", "занесённый в красную книгу", "пьяный", "припадочный", "делавший зарядку неподалёку", "путешествующий", "собирающий грибы", "залипший в телефон", "делающий селфи", "неизвестный"}; 
		string[] phrase5 = new string[] {"почтальон", "медведь", "хобит", "кенгуру", "голубь", "Джастин Бибер", "покемон", "программист", "Гарри Поттер", "отряд октябрят", "взвод солдат", "космодесантник"}; 

		
		public Simple()
		{
					
			// меню бар 1 уровня 
			MainMenu mainMenu = new MainMenu();
			MenuItem opt1 = mainMenu.MenuItems.Add("New game");
			MenuItem opt2 = mainMenu.MenuItems.Add("About");
			
			// меню бар 2 уровня 
			opt1.MenuItems.Add(new MenuItem("Baby", new EventHandler(NewGameBaby)));
			opt1.MenuItems.Add(new MenuItem("Man ", new EventHandler(NewGameMan)));
			opt1.MenuItems.Add(new MenuItem("God ", new EventHandler(NewGameGod)));
			
			opt2.MenuItems.Add(new MenuItem("About", new EventHandler(ShowAbout)));
			
			Menu = mainMenu;
			
				
			// заголовок окна
			Text = "Сапёр v4.0";
			
			// Инициализация игры 
			Initialization();	
		}

		void Initialization()
		{
			// массив содержания клеток - 100 = бомба, 1...8 = числа 
			arrayType = new int[size,size];
			// массив состояния клеток - lose/open/flag
			arrayState = new string[size,size];
			// массив клеток - кнопок
			arratButton = new MyButton[size,size];

			// установка размера окна 
			Size = new Size(size*tileSize+15,size*tileSize+55);
			CenterToScreen();
			
			// поставим бомбы
			Random random = new Random();
					
			while(bombNum < bombNumMax)
			{
				int newX = random.Next(0,size-1);
				int newY = random.Next(0, size-1);
				
				if (arrayType[newX, newY] != 100)
				{
					arrayType[newX, newY] = 100;
					bombNum ++;
				}
			}
			
			// посчитаем, чколько бомб рядом с каждой клеткой
			for (int i=0; i< arrayType.GetLength(0); i++)
			{
				for (int j=0; j< arrayType.GetLength(1); j++)
				{
					if (arrayType[i,j] != 100)
					{
					
						// слева
						if (i>0 && arrayType[i-1,j] == 100)
						{
							arrayType[i,j] ++;
						}
						// справа
						if (i<size-2 && arrayType[i+1,j] == 100)
						{
							arrayType[i,j] ++;
						}
						// сверху
						if (j>0 && arrayType[i,j-1] == 100)
						{
							arrayType[i,j] ++;
						}
						// снизу
						if (j<size-2 && arrayType[i,j+1] == 100)
						{
							arrayType[i,j] ++;
						}
						
						// слева-сверху
						if (i>0 && j>0 && arrayType[i-1,j-1] == 100)
						{
							arrayType[i,j] ++;
						}
						// справа-сверху
						if (i<size-2 && j>0 && arrayType[i+1,j-1] == 100)
						{
							arrayType[i,j] ++;
						}
						// справа-внизу
						if (i<size-2 && j<size-2 && arrayType[i+1,j+1] == 100)
						{
							arrayType[i,j] ++;
						}
						// слева-внизу
						if (i>0 && j<size-2 && arrayType[i-1,j+1] == 100)
						{
							arrayType[i,j] ++;
						}
					}
				}
			}
			
			// отрисовка кнопок
			for (int i=0; i<size; i++)
			{
				for (int j=0; j<size; j++)
				{
					// новые клетки заркты 
					arrayState[i,j] = "close";
					// клетка является кнопкой
					MyButton button = new MyButton();
					button.x = i;
					button.y = j;
					// задание координат кнопки 
					button.Location = new Point(0+tileSize*i,0+tileSize*j);
					// задание размера кнопки 
					button.Size = new Size (tileSize,tileSize);
					Controls.Add(button);
					arratButton[i,j] = button;
					// отрисовать кнопку 
					Draw(i,j);
					// подписать кнопку на событие нажатия мышью
					button.MouseDown += new MouseEventHandler(OnClick);
				}
			}
		}
		// вызываемые методы при нажатии кнопок в меню-баре 
		// новая игра, сложность = ребёнок
		void NewGameBaby(object sender, EventArgs e)
		{
			flagSet = 0;
			gameOver = false;
			bombNum = 0;
			Controls.Clear();			
			size = 10;
			bombNumMax = 10;
			Initialization();
		}
		// новая игра, сложность = мужик
		void NewGameMan(object sender, EventArgs e)
		{
			flagSet = 0;
			gameOver = false;
			bombNum = 0;
			Controls.Clear();			
			size = 15;
			bombNumMax = 30;
			Initialization();
		}
		// новая игра, сложность = Бог
		void NewGameGod(object sender, EventArgs e)
		{
			flagSet = 0;
			gameOver = false;
			bombNum = 0;
			Controls.Clear();			
			size = 20;
			bombNumMax = 70;
			Initialization();
		}
		// информация об игре 
		void ShowAbout(object sender, EventArgs e)
		{
			MessageBox.Show("Game by Ven Zell. Enjoy!");
		}
		
		// отрисовка клеток
		void Draw(int _i, int _j)
		{
			if (arrayState[_i,_j] == "close")
			{
				arratButton[_i,_j].BackColor = Color.DarkGray;
				arratButton[_i,_j].Image = null;
			}
			
			if (arrayState[_i,_j] == "flag")
			{
				arratButton[_i,_j].BackColor = Color.DarkGray;
				arratButton[_i,_j].Image = Image.FromFile(".\\source\\flag.bmp");
			}
			
			if (arrayState[_i,_j] == "open")
			{
				arratButton[_i,_j].BackColor = Color.White;
				arratButton[_i,_j].Image = null;
				
				// попал на бомбу
				if (arrayType[_i,_j] == 100)
				{
					// game over 
					gameOver = true;
					
					// нарисаовать бомбу
					arratButton[_i,_j].Image = Image.FromFile(".\\source\\bomb_red.bmp");
					
					// открыть все клетки
					OpenAllTiles();
					
					// сформируем фразу при проигрыше
					Random r = new Random();
					int f0 = r.Next(0,phrase0.GetLength(0));
					int f1 = r.Next(0,phrase1.GetLength(0));
					int f2 = r.Next(0,phrase2.GetLength(0));
					int f3 = r.Next(0,phrase3.GetLength(0));
					int f4 = r.Next(0,phrase4.GetLength(0));
					int f5 = r.Next(0,phrase5.GetLength(0));
					// показать сформированную фразу
					MessageBox.Show(phrase0[f0] + " " + phrase1[f1] + " нашли " + phrase2[f2] + " " + phrase3[f3] + ". Кроме сапёра пострадал " + phrase4[f4] + " " + phrase5[f5] + ".");
				}
				else
				{
					// попали на клетку с цифрой
					if (arrayType[_i,_j] > 0)
					{
						// рисуем цифры разного цвета
						if (arrayType[_i,_j] == 1) arratButton[_i,_j].ForeColor = Color.DarkBlue;
						if (arrayType[_i,_j] == 2) arratButton[_i,_j].ForeColor = Color.Green;
						if (arrayType[_i,_j] == 3) arratButton[_i,_j].ForeColor = Color.Red;
						//if (arrayType[_i,_j] == 4) arratButton[_i,_j].ForeColor = Color.DarkPurple;
						
						string myStr = arrayType[_i,_j].ToString();
						arratButton[_i,_j].Text = myStr;
						arratButton[_i,_j].Font = new Font(arratButton[_i,_j].Font.Name, 15, FontStyle.Bold);
					}
				}
			}
		}
		
		// нажатие на клетку
		void OnClick(object sender, MouseEventArgs e)
		{    
			if (!gameOver)
			{
				MyButton b = sender as MyButton;
					
				// правая кнопка мыши
				if (e.Button == MouseButtons.Right)
				{
					switch(arrayState[b.x, b.y])
					{
						case "close":				
						arrayState[b.x, b.y] = "flag";
						Draw(b.x, b.y);
						flagSet ++;
						break;
						
						case "flag":				
						arrayState[b.x, b.y] = "close";
						Draw(b.x, b.y);
						flagSet --;
						break;
					}
					
					// если кол-во бомб == кол-во поставленных флагов, проверить, не выйграли ли 
					if (flagSet == bombNumMax)
					{
						CheckWin();
					}
				}
				
				// левая кнопка мыши
				if (e.Button == MouseButtons.Left)
				{
					if (arrayState[b.x, b.y] == "close" || arrayState[b.x, b.y] == "flag")
					{
						Open(b.x, b.y, true);
						
					}
				}
			}			
		}
		
		// открытие клетки. РЕКУРСИВНЫЙ метод. 
		void Open(int _i, int _j, bool _eterate)
		{
			if (arrayState[_i, _j] == "close" || arrayState[_i, _j] == "flag")
			{
				arrayState[_i, _j] = "open";
				Draw(_i, _j);
				
				if (arrayType[_i, _j] != 0)
						{
							_eterate = false;
						}
				
				if (_eterate)
				{
					// меньше правой границы и справа
					if (_i < size-1 && arrayState[_i+1, _j] == "close")
					{
						Open(_i+1, _j, _eterate);
					}
					// больше левой границы и слева
					if (_i > 0 && arrayState[_i-1, _j] == "close")
					{	
						Open(_i-1, _j, _eterate);
					}
					// меньше нижней границы и снизу
					if (_j < size-1 && arrayState[_i, _j+1] == "close")
					{
						Open(_i, _j+1, _eterate);
					}
					// больше верхней границы и сверху
					if (_j > 0 && arrayState[_i, _j-1] == "close")
					{	
						Open(_i, _j-1, _eterate);
					}
					
					// меньше правой границы и справа и меньше нижней границы и снизу
					if (_i < size-1 && _j < size-1 && arrayState[_i+1, _j+1] == "close")
					{
						Open(_i+1, _j+1, _eterate);
					}
					// больше левой границы и слева и больше верхней границы и сверху
					if (_i > 0 && _j > 0 && arrayState[_i-1, _j-1] == "close")
					{	
						Open(_i-1, _j-1, _eterate);
					}
					// больше левой границы и слева и меньше нижней границы и снизу
					if (_i > 0 && _j < size-1 && arrayState[_i-1, _j+1] == "close")
					{
						Open(_i-1, _j+1, _eterate);
					}
					// меньше правой границы и справа и больше верхней границы и сверху
					if (_i < size-1 && _j > 0 && arrayState[_i+1, _j-1] == "close")
					{	
						Open(_i+1, _j-1, _eterate);
					}
				}
			}
		}
		
		// открыть все клетки 
		void OpenAllTiles()
		{
			for (int i=0; i<size; i++)
			{
				for (int j=0; j<size; j++)
				{
					if (arrayState[i, j] == "flag" && arrayType[i, j] == 100)
					{
						arratButton[i,j].Image = Image.FromFile(".\\source\\bomb_green.bmp");
					}
					if (arrayState[i, j] == "close" && arrayType[i, j] == 100)
					{
						arratButton[i,j].Image = Image.FromFile(".\\source\\bomb_red.bmp");
					}
					if (arrayState[i, j] == "flag" && arrayType[i, j] != 100)
					{
						arratButton[i,j].BackColor = Color.White;
						arratButton[i,j].Image = Image.FromFile(".\\source\\flag_false.bmp");
					}
					if (arrayState[i, j] == "close" && arrayType[i, j] != 100)
					{
						//arrayState[i, j] = "open";
						//arrayType[i, j] = 0;
						//Draw(i,j);
					}
				}
			}
		}
		
		// метод проверки условия победы 
		void CheckWin()
		{
			// проверяем, правильно ли стоят все флаги 
			bool everethingIsCorrect = true;
			
			for (int i=0; i<size; i++)
			{
				for (int j=0; j<size; j++)
				{
					// если флаг стоит на клетке, где нет бомбы 
					if ((arrayState[i, j] == "flag" && arrayType[i, j] != 100) || (arrayState[i, j] == "close" && arrayType[i, j] == 100))
					{
						everethingIsCorrect = false;
						break;
					}
				}
			}
					
			// если всё корректно, выбирается победная фразу, показывается сообщение 
			if (everethingIsCorrect)
			{
				gameOver = true;
				Random r = new Random();
				int w = r.Next(0,phraseWin.GetLength(0));
				MessageBox.Show(phraseWin[w]);
			}
		}
		
		// точка входа 
		static void Main()
		{
			Application.Run(new Simple());
		}
	}
}



