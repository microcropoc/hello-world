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
using static System.Math;

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
        const int D = 20;
        long time = 0;
        DispatcherTimer timer;
        const int N = 144;
        const int Lx = 280;
        const int Ly = 280;
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
        static double Pe;
        static double Ke;
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



        void Display()
        {
            myCanvas.Children.Clear();
            for (int i = 0; i < N; i++)
            {
                var ell = new Ellipse()
                {
                    Fill = Brushes.Indigo,
                    Height = D,
                    Width = D
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

            int dem = (int)Math.Sqrt(N);
            int j = 0;
            int k = 0;
            for (int i = 0; i < N; i++)
            {
                if (i % dem == 0)
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
            int v0 = 160;
            Random rand = new Random();

            for (int i = 0; i < N; i++)
            {
                m[i].X = v0 * (2 * rand.NextDouble() - 1);
                m[i].Y = v0 * (2 * rand.NextDouble() - 2);
            }
        }
        public void InitmasAcceleratio(out Position[] m)
        {
            m = new Position[N];
            int a0 = 450;
            Random rand = new Random();
            for (int i = 0; i < N; i++)
            {
                m[i].X = a0 * (2 * rand.NextDouble() - 1);
                m[i].Y = a0 * (2 * rand.NextDouble() - 1);
            }
        }

        public void Accel(Position[] maspos, Position[] masaccel, ref double pe)
        {
            double ap = 24;
            double dp = 22;
            for (int i = 0; i < N - 1; i++)
            {
                Position force = new Position();
                for (int j = 0; j < N; j++)
                {
                    if (i != j)
                    {
                        Position d = maspos[i] - maspos[j];
                        Separation(ref d);
                        var r = Math.Pow(d.X, 2) + Math.Pow(d.Y, 2);
                        force.X = force.X - 4 * dp * (6 * Math.Pow(ap, 6)) * d.X / (Pow(r, 4) - 12 * d.X * Math.Pow(ap, 12) / Pow(d.X, 7));
                        force.Y = force.Y - 4 * dp * (6 * Math.Pow(ap, 6)) * d.Y / (Pow(r, 4) - 12 * d.Y * Math.Pow(ap, 12) / Pow(d.Y, 7));

                        //double f;
                        //double pot;
                        //Force(r, out f, out pot);
                        //masaccel[i] = masaccel[i] + f * d;
                        //masaccel[i] = masaccel[i] - f * d;
                        //masaccel[j] = masaccel[j] + f * d;
                        //masaccel[j] = masaccel[j] - f * d;
                        //pe += pot;
                    }
                }
                massacceleratio[i].X = force.X;
                massacceleratio[i].Y = force.Y;
            }
        }
        //public void Force(double r, out double f, out double pot)
        //{
        //    double g = 24 * (1 / r) * (Math.Pow(r, 6) * (2 * (Math.Pow(r, 6) - 1)));
        //    f = g / r;
        //    pot = 4 * (Math.Pow(r, 6) * (Math.Pow(r, 6) - 1));

        //}
        public void Separation(ref Position d)
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

        public void Verlet(Position[] poss, Position[] vels, Position[] accels)
        {
            var deltaT = 0.0000000000007;
            for (int i = 0; i < N; i++)
            {
                poss[i] = poss[i] + vels[i] * deltaT + (0.5 * accels[i] * (deltaT * deltaT));


                for (int k = (i + 1); k < N; k++)
                {
                    double diff;
                    if (CheckCollision(ref poss[i], ref poss[k], out diff))
                    {
                        ResolveCollision(ref poss[i], ref poss[k], diff);
                    }
                }
                Periodic(ref poss[i], Lx, Ly);
            }
            for (int i = 0; i < N; i++)
            {
                vels[i] = vels[i] + (0.5 * accels[i] * deltaT);
            }
            double pe = 10;
            Accel(poss, accels, ref pe);
            for (int i = 0; i < N; i++)
            {
                vels[i] = vels[i] + (0.5 * accels[i] * deltaT);

                double ke = 0;
                ke = ke + 0.5 * (Math.Pow(vels[i].X, 2) + Math.Pow(vels[i].Y, 2));
                Ke = ke/10000;
            }
            txtKE.Text = Ke.ToString();
        }

        public void Periodic(ref Position pos, int Lx, int Ly)
        {
            if (pos.X > Lx)
            {
                pos.X = pos.X - Lx;
            }
            if (pos.X < 0)
            {
                pos.X = pos.X + Lx;
            }
            if (pos.Y < 0)
            {
                pos.Y = pos.Y + Lx;
            }
            if (pos.Y > Ly)
            {
                pos.Y = pos.Y - Lx;
            }
        }
        public bool CheckCollision(ref Position p1, ref Position p2, out double diff)
        {


            diff = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)); //   d = √((хА – хВ)2 + (уА – уВ)2),

            if (diff < D)
            {
                return true;
            }

            return false;
        }

        public void ResolveCollision(ref Position p1, ref Position p2, double diff)
        {

            Vector vecP1 = new Vector(p1.X, p1.Y) - new Vector(p2.X, p2.Y);
            Vector vecP2 = new Vector(p2.X, p2.Y) - new Vector(p1.X, p1.Y);
            vecP1.Normalize();
            vecP2.Normalize();
            p1.X += diff / 2 * vecP1.X;
            p1.Y += diff / 2 * vecP1.Y;
            p2.X += diff / 2 * vecP2.X;
            p2.Y += diff / 2 * vecP2.Y;
        }
    }
}