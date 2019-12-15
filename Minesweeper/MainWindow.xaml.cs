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

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Logic.mainMenu();

            updateTextures();


        }

        private void onLeftClick(object sender, RoutedEventArgs e)
        {

            Logic.onClickLeft(int.Parse(((Button)sender).Name.Remove(0, 1)));

            updateTextures();
        }

        private void onRightClick(object sender, MouseButtonEventArgs e)
        {
            Logic.onClickRight(int.Parse(((Button)sender).Name.Remove(0, 1)));

            updateTextures();
        }


        private void setTexture(Button button, string textureName)
        {
            button.Content = new Image() { Source = new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\", @"Textures\", textureName + ".png"))) };
        }
        private void updateTextures()
        {
            if (File.Exists(Logic.dataFilePath))
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
                            button.Content = '*';
                        }
                    }
                    else
                    {
                        if (tile.isFlagged)
                        {
                            setTexture(button, "flag");
                        }
                        else
                        {
                            setTexture(button, "tile");
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
        }
    }
}
