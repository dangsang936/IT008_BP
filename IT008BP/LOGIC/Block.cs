using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC
{
    public class Block
    {
        public bool[,] grid;
        public int height;
        public int width;
        string Type_name;
        public void Create_Block(string name)
        {
            Type_name = name;
            switch (Type_name)
            {
                case "3 Verticle Line Block":
                    height = 3;
                    width = 1;
                    grid = new bool[3, 1] { { true }, { true }, { true } };
                    break;
                case "4 Verticle Line Block":
                    height = 4;
                    width = 1;
                    grid = new bool[4, 1] { { true }, { true }, { true }, { true } };
                    break;
                case "3 Horizontal Line Block":
                    height = 1;
                    width = 3;
                    grid = new bool[1, 3] { { true, true, true } };
                    break;
                case "4 Horizontal Line Block":
                    height = 1;
                    width = 4;
                    grid = new bool[1, 4] { { true, true, true, true } };
                    break;
                case "L Type":
                    height = 3;
                    width = 2;
                    grid = new bool[3, 2] { { true, false }, { true, false }, { true, true } };
                    break;
                case "J Type":
                    height = 3;
                    width = 2;
                    grid = new bool[3, 2] { { false, true }, { false, true }, { true, true } };
                    break;
                case "Long L Type":
                    height = 3;
                    width = 3;
                    grid = new bool[3, 3] { { true, false, false }, { true, false, false }, { true, true, true } };
                    break;
                case "Long J Type":
                    height = 3;
                    width = 3;
                    grid = new bool[3, 3] { { false, false, true }, { false, false, true }, { true, true, true } };
                    break;
                case "F Type":
                    height = 3;
                    width = 2;
                    grid = new bool[3, 2] { { true, true }, { true, false }, { true, false } };
                    break;
                case "7 Type":
                    height = 3;
                    width = 2;
                    grid = new bool[3, 2] { { true, true }, { false, true }, { false, true } };
                    break;
                case "Long F Type":
                    height = 3;
                    width = 3;
                    grid = new bool[3, 3] { { true, true, true }, { true, false, false }, { true, false, false } };
                    break;
                case "Long 7 Type":
                    height = 3;
                    width = 3;
                    grid = new bool[3, 3] { { true, true, true }, { false, false, true }, { false, false, true } };
                    break;
                case "Z Type":
                    height = 2;
                    width = 3;
                    grid = new bool[2, 3] { { true, true, false }, { false, true, true } };
                    break;
                case "S Type":
                    height = 2;
                    width = 3;
                    grid = new bool[2, 3] { { false, true, true }, { true, true, false } };
                    break;
                case "O Type":
                    height = 2;
                    width = 2;
                    grid = new bool[2, 2] { { true, true }, { true, true } };
                    break;
                case "Big O Type":
                    height = 3;
                    width = 3;
                    grid = new bool[3, 3] { { true, true, true }, { true, true, true }, { true, true, true } };
                    break;
                case "4 Type":
                    height = 3;
                    width = 2;
                    grid = new bool[3, 2] { { true, false }, { true, true }, { false, true } };
                    break;
                case "Reverse 4 Type":
                    height = 3;
                    width = 2;
                    grid = new bool[3, 2] { { false, true }, { true, true }, { true, false } };
                    break;
                case "T Type":
                    height = 2;
                    width = 3;
                    grid = new bool[2, 3] { { true, true, true }, { false, true, false } };
                    break;
                case "Reverse T Type":
                    height = 2;
                    width = 3;
                    grid = new bool[2, 3] { { false, true, false }, { true, true, true } };
                    break;
                case "7 Up Type":
                    height = 2;
                    width = 3;
                    grid = new bool[2, 3] { { true, true, true }, { true, false, false } };
                    break;
                case "F Up Type":
                    height = 2;
                    width = 3;
                    grid = new bool[2, 3] { { true, true, true }, { false, false, true } };
                    break;
                case "F Down Type":
                    height = 2;
                    width = 3;
                    grid = new bool[2, 3] { { true, false, false }, { true, true, true } };
                    break;
                case "7 Down Type":
                    height = 2;
                    width = 3;
                    grid = new bool[2, 3] { { false, false, true }, { true, true, true } };
                    break;

            }
        }
        public bool Can_Place(MainBoard board, int x, int y)
        {
            if (board == null || board.Board == null) return false;
            int rows = board.Board.GetLength(0);
            int cols = board.Board.GetLength(1);

            if (x < 0 || y < 0 || x + height > rows || y + width > cols) return false;

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if (grid[i, j] && board.Board[i + x, j + y])
                        return false;
            return true;
        }
        
    }
}
