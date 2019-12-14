using System;
namespace MineSweeper
{
    public class Tile
    {
        public int id { get; set; }
        public int number { get; set; }
        public bool isOpen { get; set; }
        public bool isFlagged { get; set; }
    }
}
