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
        // hàm đặt block vào vị trí x,y của bảng
        public void Place(Block block, int x, int y)
        {
            for(int i = 0; i < block.height;i++)
                for(int j = 0; j < block.width;j++)
                    Board[i + x,j +  y] = true;
        }
    }
}
