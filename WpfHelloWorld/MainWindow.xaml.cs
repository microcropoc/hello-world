using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int N = 20;
        Point[,] masElement= new Point[N,N];
        long time = 0;

        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            //init timer
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick ;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        //Вызывается через заданный интервал
        private void Timer_Tick(object sender, EventArgs e)
        {
            time = time + 1;
            txtTime.Text = time.ToString();
        }

        void Display()
        {
            myCanvas.Children.Clear();

            for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
            {

                var ell = new Ellipse()
                {
                    Fill = Brushes.Indigo,
                    Height = 20,
                    Width = 20
                };

                Canvas.SetTop(ell, masElement[i,j].Y);
                Canvas.SetLeft(ell, masElement[i,j].X);

                myCanvas.Children.Add(ell);

            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    masElement[i, j].X = i * 20;
                    masElement[i, j].Y = j * 30;
                }
            Display();
            timer.Start();
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
