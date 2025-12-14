using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC
{
    // Lưu trữ dữ liệu của 1 khối
    public class BlockData
    {
        public bool[,] Grid;
        public int height;
        public int width;
        public string Type_name;
        public int Rotation_Index;  //các góc quay 0, 90, 180, 270 độ

        public BlockData(string name)
        {
            Type_name = name;
            this.Rotation_Index = 0;
            switch (name)
            {
                case "dot":
                    Grid = new bool[,] { { true } };
                    height = 1;
                    width = 1;
                    break;
                case "I2":
                    Grid = new bool[,] { { true },{ true } };
                    height = 2;
                    width = 1;
                    break;
                case "I3":
                    Grid = new bool[,] { { true }, { true }, { true } };
                    height = 3;
                    width = 1;
                    break;
                case "I4":
                    Grid = new bool[,] { { true }, { true }, { true }, { true } };
                    height = 4;
                    width = 1;
                    break;
                case "I5":
                    Grid = new bool[,] { { true }, { true }, { true }, { true }, { true } };
                    height = 5;
                    width = 1;
                    break;
                case "O":
                    Grid = new bool[,] { { true, true }, { true, true } };
                    height = 2;
                    width = 2;
                    break;
                case "O3":
                    Grid = new bool[,] { { true, true, true }, { true, true, true }, { true, true, true } };
                    height = 3;
                    width = 3;
                    break;                
                case "T":
                    Grid = new bool[,] { { false, true, false }, { true, true, true } };
                    height = 2;
                    width = 3;
                    break;
                case "L":
                    Grid = new bool[,] { { true, false }, { true, false }, { true, true } };
                    height = 3;
                    width = 2;
                    break;
                case "L3":
                    Grid = new bool[,] { { true, false, false }, { true, false, false }, { true, true, true } };
                    height = 3;
                    width = 3;
                    break;
                case "J":
                    Grid = new bool[,] { { false, true }, { false, true }, { true, true } };
                    height = 3;
                    width = 2;
                    break;
                case "J3":
                    Grid = new bool[,] { { false, false, true }, { false, false, true }, { true, true, true } };
                    height = 3;
                    width = 3;
                    break;
                case "S":
                    Grid = new bool[,] { { false, true, true }, { true, true, false } };
                    height = 2;
                    width = 3;
                    break;
                case "Z":
                    Grid = new bool[,] { { true, true, false }, { false, true, true } };
                    height = 2;
                    width = 3;
                    break;
                default:
                    throw new ArgumentException("Invalid block type");
            }
        }

        //hàm để xoay khối
        public void Rotate(int idx)
        {
            Rotation_Index = idx ;
            if (Rotation_Index == 0)
                return;
            else if (Rotation_Index == 90)
            {
                bool[,] newGrid = new bool[width, height];
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        newGrid[j, height - 1 - i] = Grid[i, j];
                Grid = newGrid;
                int temp = height;
                height = width;
                width = temp;

            }
            else if (Rotation_Index == 180)
            {
                bool[,] newGrid = new bool[height, width];
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        newGrid[height - 1 - i, width - 1 - j] = Grid[i, j];
                Grid = newGrid;
            }
            else if (Rotation_Index == 270)
            {
                bool[,] newGrid = new bool[width, height];
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        newGrid[width - 1 - j, i] = Grid[i, j];
                Grid = newGrid;
                int temp = height;
                height = width;
                width = temp;
            }
            else
                throw new Exception();
        }
    }
}
