using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC
{
    public class MainBord
    {
        public bool[,] Board { get; }
        public MainBord()
        {
            Board = new bool[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Board[i, j] = false;
        }       
        public bool Place(Block block, int x, int y)
        {
            if (block == null) return false;
            int rows = Board.GetLength(0);
            int cols = Board.GetLength(1);

            if (x < 0 || y < 0 || x + block.height > rows || y + block.width > cols)
                return false;

            for (int i = 0; i < block.height; i++)
                for (int j = 0; j < block.width; j++)
                    if (block.grid[i, j] && Board[i + x, j + y])
                        return false;

            for (int i = 0; i < block.height; i++)
                for (int j = 0; j < block.width; j++)
                    if (block.grid[i, j])
                        Board[i + x, j + y] = true;

            return true;
        }
    }
}
