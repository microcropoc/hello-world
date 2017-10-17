using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        //forDisplay
        static int[,] field;
        static Direction curDirect;
        static int X;
        static int Y;
        static bool isExit = false;
        static Timer gameTimer;
        //forInitFood
        static Random randForFood = new Random();
        static Point food;
        static void Main(string[] args)
        {
            CursorVisible = false;
            Y = WindowHeight;
            X = WindowWidth;
            field = new int[Y,X];

            //initSnake
            #region initSnakeAndFood

            Point head = new Point(X / 2, Y / 2);
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

            do
            {
                food = new Point(randForFood.Next(X + 1), randForFood.Next(Y + 1));
            } while (snake.Any(p=>p==food));
            #endregion

            //initDirect
            curDirect = Direction.Left;

            gameTimer = new Timer(gameLoop,null,0,100);

            #region loopReadKey
            do
            {
                switch (ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        if (curDirect != Direction.Down)
                        {
                            curDirect = Direction.Top;
                        }
                        break;
                    case ConsoleKey.S:
                        if (curDirect != Direction.Top)
                        {
                            curDirect = Direction.Down;
                        }
                        break;
                    case ConsoleKey.A:
                        if (curDirect != Direction.Right)
                        {
                            curDirect = Direction.Left;
                        }
                        break;
                    case ConsoleKey.D:
                        if (curDirect != Direction.Left)
                        {
                            curDirect = Direction.Right;
                        }
                        break;
                    case ConsoleKey.Escape:
                        isExit = true;
                        break;
                }
            } while (!isExit);
            #endregion
        }

        static void gameLoop(object o)
        {
            //step head
            switch(curDirect)
            {
                case Direction.Top:
                    snake[0]=new Point(snake[0].X, (snake[0].Y+1)%Y);
                    break;

                case Direction.Down:
                    snake[0] = new Point(snake[0].X, (snake[0].Y - 1)<0?Y-1: (snake[0].Y - 1));
                    break;

                case Direction.Left:
                    snake[0] = new Point((snake[0].X - 1)<0? X-1: (snake[0].X - 1), snake[0].Y);
                    break;

                case Direction.Right:
                    snake[0] = new Point((snake[0].X + 1) % X, snake[0].Y);
                    break;
            }

            if(snake.Skip(1).Any(p=>p==snake[0]))
            {
                //GameOver
                throw new Exception();
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
                //genFood
                do
                {
                    food = new Point(randForFood.Next(X + 1), randForFood.Next(Y + 1));
                } while (snake.Any(p => p == food));
            }

            Display();
        }

        #region DisplayMethods

        /// <summary>
        /// item1->i
        /// item2->j
        /// </summary>
        static List<Tuple<int, int>> histList = new List<Tuple<int, int>>(); 
        static void ClearConAndField()
        {
            var deleteObjs = histList.ToList();
            foreach (var i in deleteObjs)
            {
                SetCursorPosition(i.Item2, i.Item1);
                field[i.Item1, i.Item2] = 0;
                Write(' ');
            }
            histList.RemoveAll(p=>deleteObjs.Any(j=>j.Item1==p.Item1 && j.Item2==p.Item2));
        }

        Point shadowTail;
        int snakeLong;

        static void Display()
        {
             ClearConAndField();
            #region oldClear

            //for (int i = 0; i < Y; i++)
            //{
            //    for (int j = 0; j < X; j++)
            //    {
            //        field[i, j] = 0;
            //    }
            //}

            //Clear();

            #endregion

            #region AddObjToField

            //disSnakeToMatrix
            foreach (var i in snake)
            {
                //разница между координатной сеткой и матрицей
                field[Math.Abs(i.Y - (Y - 1)), i.X] = 1;
            }

            field[Math.Abs(food.Y - (Y - 1)), food.X] = 2;

            #endregion

            //RenderMAtrix
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    switch (field[i,j])
                    {
                        //cellSnake
                        case 1:
                            SetCursorPosition(j, i);
                            Write("#");
                            histList.Add(new Tuple<int, int>(i, j));
                            break;
                        //food
                        case 2:
                            SetCursorPosition(j, i);
                            Write("$");
                            histList.Add(new Tuple<int, int>(i, j));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
