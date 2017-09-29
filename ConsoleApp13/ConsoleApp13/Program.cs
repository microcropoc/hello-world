using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp13
{
#region struct
    public struct Energy //структура
    {
        public double E { get; set; }


        public Energy(double e) // конструктор
        {
            E = e;

        }

    }
    public struct Position //структура
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Position(double x, double y) // конструктор
        {
            X = x;
            Y = y;
        }


    }
    public struct Velocity //структура
    {
        public double VX { get; set; }
        public double VY { get; set; }

        public Velocity(double vx, double vy) // конструктор
        {
            VX = vx;
            VY = vy;
        }

    }
    public struct Acceleratio //структура
    {
        public double AX { get; set; }
        public double AY { get; set; }

        public Acceleratio(double ax, double ay) // конструктор
        {
            AX = ax;
            AY = ay;
        }

    }
    #endregion

    class Program
    {
        const int N = 15;
        static Position[,] massPosition;
        static double x = 20;
        static double y = 20;
        static Velocity[,] massVelocity;
        static double vx;
        static double vy;
        static Acceleratio[,] massacceleratio;
        static double ax;
        static double ay;
        static Energy[] massEnergy;
        static double e;

        static void Main(string[] args)
        {
            InitmasPosition(out massPosition);
            InitmasVelocity(out massVelocity);
            InitmasAcceleratio(out massacceleratio);
            Console.ReadKey();
        }

        public static void InitmasPosition(out Position[,] m)
        {
            m = new Position[N,N];
            int x0 = 20;
            int y0 = 20;
            double b = 24;
            m[0, 0] = new Position(20, 20);

            for (int i = 0; i < N; i++)

                for (int j = 0; j < N; j++)
                {
                    m[i, j].X = x0 + i * b;        //иксы
                    m[i, j].Y = y0 + j * b;       // игреки
                }
        }

        public static void InitmasVelocity(out Velocity[,] m)
        {
            m = new Velocity[N, N];
            int v0 = 160;
            Random rand = new Random();

            for (int i = 0; i < N; i++)

                for (int j = 0; j < N; j++)
                {
                    m[i, j].VX = v0 * rand.NextDouble();        
                    m[i, j].VY = v0 * rand.NextDouble();       
                }
        }
        public static void InitmasAcceleratio(out Acceleratio[,] m)
        {
            m = new Acceleratio[N, N];
            int a0 = 160;
            Random rand = new Random();

            for (int i = 0; i < N; i++)

                for (int j = 0; j < N; j++)
                {
                    m[i, j].AX = a0 * rand.NextDouble();        
                    m[i, j].AY = a0 * rand.NextDouble();       
                }
        }
        public static void Verlet(Position[,] signaturaP , Velocity[,] signaturaV, Acceleratio[,] sugnaturaA )
        {
            var deltaT = 20;
            for (int i = 0; i < N; i++)
                for (int j = 0; i <N; j++)
                {

                 Position newPos = signaturaP[i,j] + signaturaV[i,j]
                }
                
            
        }



    }
}
