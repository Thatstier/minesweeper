using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace MineSweeper
{
    /// <summary>
    /// Логика взаимодействия для StartingPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(int difficulty, bool isContinued)
        {
            InitializeComponent();

            createButtonsGrid(difficulty);

            if (isContinued == false)
            {
                Logic.restart();
            }
            Logic.mainMenu(difficulty);

            setTexture(restartButton, "face1");
            MinesIndicator.Text = Logic.mines.ToString();
            updateTextures();
        }

        private void onLeftClick(object sender, RoutedEventArgs e)
        {
            if (Logic.gameState == 1)
            {
                Logic.onClickLeft(int.Parse(((Button)sender).Name.Remove(0, 1)));
            }
            else
            {
                Logic.restart();
            }
            updateTextures();
        }

        private void onRightClick(object sender, MouseButtonEventArgs e)
        {
            if (Logic.gameState == 1)
            {
                Logic.onClickRight(int.Parse(((Button)sender).Name.Remove(0, 1)));
                updateTextures();
            }
        }

        private void onMouseDown(object sender, MouseButtonEventArgs e)
        {
            setTexture(restartButton, "face4");
        }

        private void restart(object sender, RoutedEventArgs e)
        {
            Logic.restart();
            updateTextures();
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).Width = DifficultyChoicePage.defaultSize.Item1;
                    (window as MainWindow).Height = DifficultyChoicePage.defaultSize.Item2;
                    (window as MainWindow).Main.Width = DifficultyChoicePage.defaultSize.Item1;
                    (window as MainWindow).Main.Height = DifficultyChoicePage.defaultSize.Item2;
                    (window as MainWindow).Main.Content = new EnterPage() { Width = DifficultyChoicePage.defaultSize.Item1, Height = DifficultyChoicePage.defaultSize.Item2 };
                }
            }
        }

        public void createButtonsGrid(int difficulty)
        {
            

            Button AddButton(int name, int row, int column, Grid parent)
            {
                Button button = new Button();
                button.Name = "B" + name;
                button.Content = "";
                button.Click += onLeftClick;
                button.MouseRightButtonUp += onRightClick;
                button.MouseDown += onMouseDown;
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.VerticalAlignment = VerticalAlignment.Top;
                button.Width = 30.0;
                button.Height = 30.0;

                parent.Children.Add(button);
                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);
                return button;
            }

            int side = 0;

            switch (difficulty)
            {
                case 1:
                    side = 8;
                    break;
                case 2:
                    side = 16;
                    break;
                case 3:
                    side = 24;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < side; i++)
            {
                ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                ButtonsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }
            for (int i = 0; i < side * side; i++)
            {
                AddButton(i, i / side, i % side, ButtonsGrid);
            }
        }


        private void setTexture(Button button, string textureName)
        {
            button.Content = new Image() { Source = new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\", @"Textures\", textureName + ".png"))) };
        }

        private void updateTextures()
        {
            int minesCount = Logic.mines;
            if (File.Exists(Logic.dataFilePath) || Logic.inGame == true)
            {
                foreach (Button button in ButtonsGrid.Children)
                {
                    Tile tile = Logic.tilesList[ButtonsGrid.Children.IndexOf(button)];
                    if (tile.isOpen == true)
                    {
                        if (tile.number != -1)
                        {
                            setTexture(button, tile.number.ToString());
                        }
                        else
                        {
                            setTexture(restartButton, "face4");
                            setTexture(button, "mine_fatal");
                        }
                    }
                    else
                    {
                        if (tile.isFlagged)
                        {
                            setTexture(button, "flag");
                            minesCount--;
                        }
                        else
                        {
                            setTexture(button, "tile");
                        }

                        if (File.Exists(Logic.dataFilePath) == false && Logic.inGame == true)
                        {
                            if (tile.number != -1 && tile.isFlagged == true)
                            {
                                setTexture(button, "mine_false");
                            }
                            if (tile.number == -1 && tile.isFlagged == false)
                            {
                                setTexture(button, "mine");
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Button button in ButtonsGrid.Children)
                {
                    setTexture(button, "tile");
                }
            }

            if (minesCount >= 0)
            {
                MinesIndicator.Text = minesCount.ToString();
            }
            else
            {
                MinesIndicator.Text = "0";
            }

            setTexture(restartButton, "face" + Logic.gameState);
        }
    }
}

