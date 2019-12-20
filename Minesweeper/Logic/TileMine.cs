using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper
{
    public class TileMine : ITileInterface
    {
        public int id { get; set; }

        public int number { get => -1; set { } }
    }
}
