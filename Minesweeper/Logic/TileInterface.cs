using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper
{
    public interface ITileInterface
    {
        public int id { get; set; }

        public int number { get; set; }
    }
}
