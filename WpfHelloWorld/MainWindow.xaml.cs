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

namespace WpfHelloWorld
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int N = 50;
        Point[,] masElement= new Point[N,N];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
            {
                masElement[i, j].X = i * 20;
                masElement[i, j].Y = j * 30;
            }
            Display();
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
    }
}
