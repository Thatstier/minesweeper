using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

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

            Main.Content = new EnterPage();
        }

    }
}
