using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board
{
    public class MainBord
    {
        private bool[8, 8] Board;
        public Board => Board;
        public MainBord()
        {
            Board = new bool[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Board[i, j] = false;
        }
    }
}
