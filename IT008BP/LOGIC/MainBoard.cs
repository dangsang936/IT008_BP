using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC
{
    // lớp này là bảng chính
    public class MainBoard
    {
        public bool[,] Grid { get; set; }
        
        public MainBoard()
        {
            Grid = new bool[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Grid[i, j] = false;
        }

        // hàm kiểm tra có thể đặt không
        public bool Can_Place(Block block)
        {
            int x = block.X;
            int y = block.Y;
            if (x < 0 || y < 0 || x > 7 || y > 7)
                return false;
            else if (x + block.data.width > 8
                || x + block.data.width < 0 
                || y + block.data.height > 8 
                || y + block.data.height < 0)
                return false;
            for (int i = 0; i < block.data.height; i++)
                for (int j = 0; j < block.data.width; j++)
                    if (block.data.Grid[i, j] && Grid[y + i, x + j])
                        return false;                  
            return true;
        }

        // hàm đặt block vào bảng
        public bool Place(Block block)
        {
            int x = block.X;
            int y = block.Y;
            if (!Can_Place(block)) 
                return false;
            for (int i = 0; i < block.data.height; i++)
                for(int j = 0; j < block.data.width ; j++)
                    if (block.data.Grid[i, j])
                        Grid[y + i, x + j] = true;
            return true;
        }

        // hàm kiểm tra đầy hàng và trả về 1 mảng các hàng đã đầy
        public List<int> CheckRowsFull()
        {
            List<int> answer = new List<int>();
            for (int i = 0;i < 8; i++)
                for(int j = 0; j < 8 ; j++)
                {
                    if (!Grid[i, j])
                        break;
                    if (j==7)
                        answer.Add(i);
                }    
            return answer;
        }

        // hàm kiểm tra các cột đã đầy và trả về 1 mảng các cột đã đầy
        public List<int> CheckColumnsFull()
        {
            List<int> answer = new List<int>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (!Grid[j, i])
                        break;
                    if (j == 7)
                        answer.Add(i);                   
                }
            return answer;
        }

        // hàm xóa nhiều hàng, nhiều cột đã đầy
        public void Clear()
        {
            List<int> collumns = CheckColumnsFull();
            List<int> rows = CheckRowsFull();
            // xóa cột
            foreach(int collumn in collumns)
                for(int i = 0; i < 8 ; i++)
                    Grid[i,collumn] = false;

            // xóa hàng
            foreach (int row in rows)
                for(int i = 0; i < 8; i ++)
                    Grid[row,i] = false;
        }
    }
}
