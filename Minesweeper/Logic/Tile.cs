using System;
namespace MineSweeper
{
    public class Tile : TileInterface
    {
        public int number { get; set; }
        public bool isOpen { get; set; }
        public bool isFlagged { get; set; }
    }
}
