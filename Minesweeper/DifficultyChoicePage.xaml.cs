using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MineSweeper
{
    /// <summary>
    /// Логика взаимодействия для DifficultyChoicePage.xaml
    /// </summary>
    public partial class DifficultyChoicePage : Page
    {
        public static (int, int) defaultSize = (350, 450);
        public static (int, int) Size1 = (350, 400);
        public static (int, int) Size2 = (600, 650);
        public static (int, int) Size3 = (850, 900);
        public DifficultyChoicePage()
        {
            InitializeComponent();
        }

        private void onClickDifficulty(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name.Remove(0, 1) == "1")
            {
                openPage(1, Size1.Item1, Size1.Item2);
            }
            if (((Button)sender).Name.Remove(0, 1) == "2")
            {
                openPage(2, Size2.Item1, Size2.Item2);
            }
            if (((Button)sender).Name.Remove(0, 1) == "3")
            {
                openPage(3, Size3.Item1, Size3.Item2);
            }
        }

        private void onClickBack(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Width = defaultSize.Item1;
                    (window as MainWindow).Height = defaultSize.Item2;
                    (window as MainWindow).Main.Width = defaultSize.Item1;
                    (window as MainWindow).Main.Height = defaultSize.Item2;
                    (window as MainWindow).Main.Content = new EnterPage() { Width = defaultSize.Item1, Height = defaultSize.Item2 };
                }
            }
        }

        private void openPage(int num, int width, int height)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Width = width;
                    (window as MainWindow).Height = height;
                    (window as MainWindow).Main.Width = width;
                    (window as MainWindow).Main.Height = height;
                    (window as MainWindow).Main.Content = new MainPage(num, false) { Width = width, Height = height };
                }
            }
        }

    }
}
