using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfHelloWorld
{
    public struct Position //структура
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Position(double x, double y) // конструктор
        {
            X = x;
            Y = y;
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Position operator *(Position p, double d)
        {
            return new Position(p.X * d, p.Y * d);
        }

        public static Position operator *(double d, Position p)
        {
            return new Position(p.X * d, p.Y * d);
        }
        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.X - p2.X, p1.Y - p2.Y);
        }
    }
    public partial class MainWindow : Window
    {
        long time = 0;
        DispatcherTimer timer;
        const int N = 9;
        //const int Lx = 640;
       // const int Ly = 480;
        static Position[] massPosition;
        static double x = 20;
        static double y = 20;
        static Position[] massVelocity;
        static double vx;
        static double vy;
        static Position[] massacceleratio;
        static double ax;
        static double ay;
        static double[] massEnergy;
        static double pe;
        public MainWindow()
        {
            InitializeComponent();
            InitmasPosition(out massPosition);
            InitmasVelocity(out massVelocity);
            InitmasAcceleratio(out massacceleratio);
            //init timer
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            Display();
        }
        

        //Вызывается через заданный интервал
        private void Timer_Tick(object sender, EventArgs e)
        {
           // Stopwatch stopWatch = new Stopwatch();
           // stopWatch.Start();
            Verlet(massPosition, massVelocity, massacceleratio);
            Display();
            time = time + 1;
            txtTime.Text = time.ToString();
           // stopWatch.Stop();
           // var diagTime = stopWatch.ElapsedMilliseconds;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        Brush[] masBrsh = new[] 
        {
            Brushes.Black,
            Brushes.Indigo,
            Brushes.Red,
            Brushes.Green,
            Brushes.HotPink,
            Brushes.Khaki,
            Brushes.Blue,
            Brushes.Orange,
            Brushes.Violet
        };

        void Display()
        {
            myCanvas.Children.Clear();
            for (int i = 0; i < N; i++)
            {
                var ell = new Ellipse()
                {
                    Fill = masBrsh[i],
                    Height = 3*(i+1),
                    Width = 3*(i+1)
                };
                Canvas.SetTop(ell, massPosition[i].Y);
                Canvas.SetLeft(ell, massPosition[i].X);
                myCanvas.Children.Add(ell);
            }
        }
        

        public static void InitmasPosition(out Position[] m)
        {
            m = new Position[N];
            int x0 = 20;
            int y0 = 20;
            double b = 20;
            m[0] = new Position(20, 20);

            int dem =(int) Math.Sqrt(N);
            int j = 0;
            int k = 0;
            for (int i = 0; i < N; i++)
            {
                if(i%dem==0)
                {
                    j++;
                    k = 0;
                }
                m[i].X = x0 + k * b;        //иксы
                m[i].Y = y0 + j * b;       // игреки
                k++;
            }            
        }

        public void InitmasVelocity(out Position[] m)
        {
            m = new Position[N];
            int v0 = 1;
            Random rand = new Random();

            for (int i = 0; i < N; i++)
            {
                m[i].X = v0 * rand.NextDouble();
                m[i].Y = v0 * rand.NextDouble();
            }
        }
        public void InitmasAcceleratio(out Position[] m)
        {
            m = new Position[N];
            int a0 = 2;
            Random rand = new Random();
            for (int i = 0; i < N; i++)
            {
                m[i].X = a0 * rand.NextDouble();
                m[i].Y = a0 * rand.NextDouble();
            }
        }

        public void Accel(Position[] maspos, Position[] masaccel, ref double pe)
        {
            for (int i = 0; i < N - 1; i++)
                for (int j = (i + 1); j < N; j++)
                {
                    Position d = maspos[i] - maspos[j];
                    Separation(ref d);
                    var r = Math.Sqrt(Math.Pow(d.X, 2) + Math.Pow(d.Y, 2));
                    double f;
                    double pot;
                    Force(r, out f, out pot);
                    masaccel[i] = masaccel[i] + f * d;
                    masaccel[i] = masaccel[i] + f * d;
                    pe += pot;
                }
        }
        public  void Force(double r, out double f, out double pot)
        {
            double g = 24 * (1 / r) * (Math.Pow(r, 6) * (2 * (Math.Pow(r, 6) - 1)));
            f = g / r;
            pot = 4 * (Math.Pow(r, 6) * (Math.Pow(r, 6) - 1));

        }
        public  void Separation(ref Position d)
        {
            if (Math.Abs(d.X) > 0.5 * (int)myCanvas.ActualWidth)
            {
                d.X = d.X - Math.Sign(d.X) * (int)myCanvas.ActualWidth;
            }
            if (Math.Abs(d.Y) > 0.5 * (int)myCanvas.ActualHeight)
            {
                d.Y = d.Y - Math.Sign(d.Y) * (int)myCanvas.ActualHeight;
            }
        }

        public  void Verlet(Position[] poss, Position[] vels, Position[] accels)
        {
            var deltaT = 0.001;
            for (int i = 0; i < N; i++)
            {
                poss[i] = poss[i] + vels[i] * deltaT + (0.5 * accels[i] * (deltaT * deltaT));
                Periodic(ref poss[i],(int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight);
            }
            for (int i = 0; i < N; i++)
            {
                vels[i] = vels[i] + (0.5 * accels[i] * deltaT);
            }
            double pe = 0;
            Accel(poss, accels, ref pe);
            for (int i = 0; i < N; i++)
            {
                vels[i] = vels[i] + (0.5 * accels[i] * deltaT);

                double ke = 0;
                ke = ke + 0.5 * (Math.Pow(vels[i].X, 2) + Math.Pow(vels[i].Y, 2));
            }

        }

        public void Periodic(ref Position pos, int Lx, int Ly)
        {
            //bool flag = false;
            //do
            //{
            //    flag = false;
            //    if (pos.X >= 640)
            //    {
            //        pos.X = pos.X - 640;
            //        flag = true;
            //    }
            //    if (pos.Y >= 480)
            //    {
            //        pos.Y = pos.Y - 480;
            //        flag = true;
            //    }
            //    if (pos.X < 0)
            //    {
            //        pos.X = pos.X + 640;
            //        flag = true;
            //    }
            //    if (pos.Y < 0)
            //    {
            //        pos.Y = pos.Y + 480;
            //        flag = true;
            //    }
            //} while (flag);

            pos.X = Math.Abs(((pos.X * 100) % (Lx * 100)) / 100);
            pos.Y = Math.Abs(((pos.Y * 100) % (Ly * 100)) / 100);

            //pos.X = pos.X % 600;
            //pos.Y = pos.Y % 600;
        }


        #region oldmainwindow
        //public partial class MainWindow : Window
        //{
        //    const int N = 20;
        //    Point[,] masElement = new Point[N, N];
        //    long time = 0;

        //    DispatcherTimer timer;

        //    public MainWindow()
        //    {
        //        InitializeComponent();
        //        //init timer
        //        timer = new DispatcherTimer();
        //        timer.Tick += Timer_Tick;
        //        timer.Interval = new TimeSpan(0, 0, 1);
        //    }


        //    //Вызывается через заданный интервал
        //    private void Timer_Tick(object sender, EventArgs e)
        //    {
        //        time = time + 1;
        //        txtTime.Text = time.ToString();
        //    }

        //    void Display()
        //    {
        //        myCanvas.Children.Clear();

        //        for (int i = 0; i < N; i++)
        //            for (int j = 0; j < N; j++)
        //            {

        //                var ell = new Ellipse()
        //                {
        //                    Fill = Brushes.Indigo,
        //                    Height = 20,
        //                    Width = 20
        //                };

        //                Canvas.SetTop(ell, masElement[i, j].Y);
        //                Canvas.SetLeft(ell, masElement[i, j].X);

        //                myCanvas.Children.Add(ell);

        //            }
        //    }

        //    private void btnOK_Click(object sender, RoutedEventArgs e)
        //    {
        //        for (int i = 0; i < N; i++)
        //            for (int j = 0; j < N; j++)
        //            {
        //                masElement[i, j].X = i * 20;
        //                masElement[i, j].Y = j * 30;
        //            }
        //        Display();
        //        timer.Start();
        //    }


        //    private void btnStart_Click(object sender, RoutedEventArgs e)
        //    {
        //        timer.Start();
        //    }

        //    private void btnStop_Click(object sender, RoutedEventArgs e)
        //    {
        //        timer.Stop();
        //    }
        //}
        #endregion
    }
}

