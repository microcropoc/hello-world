﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Console;

namespace ConsoleSn
{
    public struct Point : IEquatable<Point>
    {
        private readonly int _x;
        private readonly int _y;

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public override int GetHashCode()
        {
            return _x ^ _y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;

            return Equals((Point)obj);
        }

        public bool Equals(Point other)
        {
            if (_x != other._x)
                return false;

            return _y == other._y;
        }

        public static bool operator ==(Point point1, Point point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return !point1.Equals(point2);
        }
    }
    enum Direction { Left,Right,Top,Down}
    class Program
    {
        static List<Point> snake;
        static Direction CurDirect
        {
            get
            {
                fixKeyRead = false;
                return _curDirect;
            }
            set
            {
                fixKeyRead = true;
                _curDirect = value;
            }
        }
        static bool fixKeyRead;
        static Direction _curDirect;
        static int maxX;
        static int maxY;
        static Random randForFood = new Random();
        static Point food;
        static int speedSnake;
        static bool isExit;

        static void initSetting(string[] args)
        {
            speedSnake = 70;
            int width = 0;
            int height = 0;
            if (args.Length >= 2)
            {
                int.TryParse(args[0], out width);
                int.TryParse(args[1], out height);
            }
            if (height > 0)
                if (LargestWindowHeight < height)
                    WindowHeight = LargestWindowHeight;
                else
                    WindowHeight = height;
            if (width > 0)
                if (LargestWindowWidth < width)
                    WindowWidth = LargestWindowWidth;
                else
                    WindowWidth = width;
            CursorVisible = false;
        }
        static void Main(string[] args)
        {
            initSetting(args);
            maxY = WindowHeight;
            maxX = WindowWidth;

            #region initSnakeAndFood

            Point head = new Point(maxX / 2, maxY / 2);
            snake = new List<Point>()
            {
                head,
                new Point(head.X + 1, head.Y),
                new Point(head.X + 2, head.Y),
                new Point(head.X + 3, head.Y),
                new Point(head.X + 4, head.Y),
                new Point(head.X + 5, head.Y),
                new Point(head.X + 6, head.Y),
                new Point(head.X + 7, head.Y),
                new Point(head.X + 8, head.Y),
                new Point(head.X + 9, head.Y)
            };

            GenFood();

            #endregion

            initDisplay();
            //initDirect
            CurDirect = Direction.Left;

            using (var gameTimer = new Timer(gameLoop, null, 0, speedSnake))
            {
                #region loopReadKey
                do
                {
                    if (!fixKeyRead)
                        switch (ReadKey(true).Key)
                        {
                            case ConsoleKey.W:
                            case ConsoleKey.UpArrow:
                                if (CurDirect != Direction.Down)
                                {
                                    CurDirect = Direction.Top;
                                }
                                else
                                {
                                    CurDirect = Direction.Down;
                                }
                                break;
                            case ConsoleKey.S:
                            case ConsoleKey.DownArrow:
                                if (CurDirect != Direction.Top)
                                {
                                    CurDirect = Direction.Down;
                                }
                                else
                                {
                                    CurDirect = Direction.Top;
                                }
                                break;
                            case ConsoleKey.A:
                            case ConsoleKey.LeftArrow:
                                if (CurDirect != Direction.Right)
                                {
                                    CurDirect = Direction.Left;
                                }
                                else
                                {
                                    CurDirect = Direction.Right;
                                }
                                break;
                            case ConsoleKey.D:
                            case ConsoleKey.RightArrow:
                                if (CurDirect != Direction.Left)
                                {
                                    CurDirect = Direction.Right;
                                }
                                else
                                {
                                    CurDirect = Direction.Left;
                                }
                                break;
                        }

                } while (!isExit);
                #endregion
            }
            displayExit();
            ReadKey(true);
        }

        static void gameLoop(object o)
        {
            //step head
            switch(CurDirect)
            {
                case Direction.Top:
                    snake[0]=new Point(snake[0].X, (snake[0].Y+1)%maxY);
                    break;

                case Direction.Down:
                    snake[0] = new Point(snake[0].X, (snake[0].Y - 1)<0?maxY-1: (snake[0].Y - 1));
                    break;

                case Direction.Left:
                    snake[0] = new Point((snake[0].X - 1)<0? maxX-1: (snake[0].X - 1), snake[0].Y);
                    break;

                case Direction.Right:
                    snake[0] = new Point((snake[0].X + 1) % maxX, snake[0].Y);
                    break;
            }

            if(snake.Skip(1).Any(p=>p==snake[0]))
            {
                //GameOver
                isExit = true;
            }

            //))
            Point shadowTail = snake[snake.Count - 1];

            //stepsOtherSnakeCell
            for (int i = snake.Count - 1; i > 0 ; i--)
            {
                snake[i] = snake[i - 1];
            }

            if(snake[0]==food)
            {
                snake.Add(shadowTail);
                GenFood();
            }

            Display();
        }

        public static void GenFood()
        {
            //genFood
            do
            {
                food = new Point(randForFood.Next(maxX), randForFood.Next(maxY));
            } while (snake.Any(p => p == food));
        }

        #region DisplayMethods

        static Point lastFood;
        static Point shadowTail;

        static void displayExit()
        {
            Clear();
            WriteLine("Score: "+snake.Count);
        }

        static void initDisplay()
        {
            //init lastFood with unreal position
            lastFood = new Point(maxX,maxY);
            shadowTail = snake[snake.Count - 1];
        }

        static void Display()
        {
            //write head
            var head = snake[0];
            SetCursorPosition(head.X, Math.Abs(head.Y - (maxY - 1)));
            ForegroundColor = ConsoleColor.DarkGreen;
            Write("#");

            if (lastFood == food)
            {
                SetCursorPosition(shadowTail.X, Math.Abs(shadowTail.Y - (maxY - 1)));
                ForegroundColor = ConsoleColor.DarkGray;
                Write('*');
            }
            else
            {
                lastFood = food;
                SetCursorPosition(food.X, Math.Abs(food.Y - (maxY - 1)));
                ForegroundColor = ConsoleColor.Green;
                Write("$");
            }
            shadowTail = snake[snake.Count - 1];
        }

        #endregion
    }
}
