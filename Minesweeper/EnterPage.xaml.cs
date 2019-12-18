using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для EnterPage.xaml
    /// </summary>
    public partial class EnterPage : Page
    {
        public EnterPage()
        {
            InitializeComponent();
        }

        private void onClickContinue(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Logic.dataFilePath))
            {
                string[] dataFile = File.ReadAllLines(Logic.dataFilePath);
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        (int, int) defaultSize = (0, 0);
                        switch (int.Parse(dataFile[0].Remove(0, 11)))
                        {
                            case 1:
                                defaultSize = DifficultyChoicePage.Size1;
                                break;
                            case 2:
                                defaultSize = DifficultyChoicePage.Size2;
                                break;
                            case 3:
                                defaultSize = DifficultyChoicePage.Size3;
                                break;
                            default:
                                break;
                        }

                        (window as MainWindow).Width = defaultSize.Item1;
                        (window as MainWindow).Height = defaultSize.Item2;
                        (window as MainWindow).Main.Width = defaultSize.Item1;
                        (window as MainWindow).Main.Height = defaultSize.Item2;
                        (window as MainWindow).Main.Content = new MainPage(int.Parse(dataFile[0].Remove(0, 11)), true) { Width = defaultSize.Item1, Height = defaultSize.Item2 };
                    }
                }
            }
            else
            {
                newGame();
            }
        }

        private void onClickNew(object sender, RoutedEventArgs e)
        {
            newGame();
        }

        private void onClickExit(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Close();
                }
            }
        }

        private void newGame()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Main.Content = new DifficultyChoicePage();
                }
            }
        }
    }
}
