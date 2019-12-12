using System;

namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to MineSweeper!\n");
            Console.WriteLine("Enter field width: ");
            int widthInput = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter field height: ");
            int heightInput = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter number of mines: ");
            int mines = int.Parse(Console.ReadLine());
            Console.WriteLine("Open the first tile, enter number: ");
            int startingTile = int.Parse(Console.ReadLine());

            Logic.createField(widthInput, heightInput, mines, startingTile);
        }
    }
}
