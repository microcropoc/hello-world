﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp13
{
#region struct

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

        public static Position operator *(Position p,double d)
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
    //public struct Velocity //структура
    //{
    //    public double VX { get; set; }
    //    public double VY { get; set; }

    //    public Velocity(double vx, double vy) // конструктор
    //    {
    //        VX = vx;
    //        VY = vy;
    //    }

    //}
    //public struct Acceleratio //структура
    //{
    //    public double AX { get; set; }
    //    public double AY { get; set; }

    //    public Acceleratio(double ax, double ay) // конструктор
    //    {
    //        AX = ax;
    //        AY = ay;
    //    }

    //}
    #endregion

    class Program
    {
        const int N = 15;
        const int Lx= 640;
        const int Ly= 480;
        static Position[,] massPosition;
        static double x = 20;
        static double y = 20;
        static Position[,] massVelocity;
        static double vx;
        static double vy;
        static Position[,] massacceleratio;
        static double ax;
        static double ay;
        static double[] massEnergy;
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

        public static void InitmasVelocity(out Position[,] m)
        {
            m = new Position[N, N];
            int v0 = 160;
            Random rand = new Random();

            for (int i = 0; i < N; i++)

                for (int j = 0; j < N; j++)
                {
                    m[i, j].X = v0 * rand.NextDouble();        
                    m[i, j].Y = v0 * rand.NextDouble();       
                }
        }
        public static void InitmasAcceleratio(out Position[,] m)
        {
            m = new Position[N, N];
            int a0 = 160;
            Random rand = new Random();

            for (int i = 0; i < N; i++)

                for (int j = 0; j < N; j++)
                {
                    m[i, j].X = a0 * rand.NextDouble();        
                    m[i, j].Y = a0 * rand.NextDouble();       
                }
        }

        public static void Accel(Position[,] maspos, Position[,] masaccel)
        {

         for(i=0, i<N-1,i++)
         for(j=(i+1), i<N,j++)
         {
            Position d = maspos[i,j]- maspos[j,i];
            Separation(ref d);
            var r= Math.Sqrt(Math.Pow(d.X,2)+Math.Pow(d.Y,2));
            double f;
            double pot;
            Force(r,out f,out pot)
            masaccel[i,j]=

         }
          
        
        }
        public static void Force(double r,out double f, out double pot)
        {
            double g= 24*(1/r)*(Math.Pow(r,6)*(2*(Pow(r,6)-1)));
            f=g/r;
            pot=4*(Math.Pow(r,6)*(Math.Pow(r,6)-1));

        }
        public static void Separation(ref Position d)
        {
            if (Math.Abs(d) > 0.5*Lx)
            {
                d.X = d.X - Math.Sign(d.X)*Lx:
            }
            if (Math.Abs(d) > 0.5*Ly)
            {
                d.Y = d.Y - Math.Sign(d.Y)*Ly:
            }
        }

        public static void Verlet(Position[,] poss , Position[,] vels, Position[,] accels)
        {
            var deltaT = 20;
            for (int i = 0; i < N; i++)
            for (int j = 0; i <N; j++)
            {
                poss[i, j] = poss[i, j] + vels[i, j] * deltaT+(0.5*accels[i, j]*(deltaT * deltaT));
            }
            for (int i = 0; i < N; i++)
            for (int j = 0; i < N; j++)
            {
                vels[i, j] = vels[i, j] + (0.5 * accels[i, j] * deltaT);
            }
            InitmasAcceleratio(out massacceleratio);
            for (int i = 0; i < N; i++)
            for (int j = 0; i < N; j++)
            {
                poss[i, j] = poss[i, j] + vels[i, j] * deltaT + (0.5 * accels[i, j] * (deltaT * deltaT));
            }
        }

        public static void Periodic(Position pos,int Lx,int Ly)
        {

        }



    }
}
