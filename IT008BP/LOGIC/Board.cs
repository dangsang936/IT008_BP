using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOGIC
{
    public class MainBord
    {
        public bool[,] Board { get; set; }
        
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
            for (int i = 0; i < block.height; i++)
                for (int j = 0; j < block.width; j++)
                    Board[i + x, j + y] = true;
        }
        // hàm kiểm tra đầy hàng, trả về mảng các hàng đầy, nếu không có hàng nào đầy trả về mảng rỗng
        public int[] Check_full_rows(params int[] rows)
        {
            bool[] found = new bool[rows.Length];
            for (int idx = 0; idx < rows.Length; idx++)
            {
                int row = rows[idx];
                found[idx] = true;
                for (int i = 0; i < 8; i++)
                {
                    if (Board[row, i] == false)
                    {
                        found[idx] = false;
                        break;
                    }
                }
            }
            return found.Select((value, index) => new { value, index })
                        .Where(x => x.value)
                        .Select(x => rows[x.index])
                        .ToArray();
        }
        //hàm xóa nhiều dòng 
        public void clear_rows(int[] rows)
        {
            if (rows.Length == 0) return;
            foreach (int row in rows)
                for (int i = 0; i < 8; i++)
                    Board[row,i] = false;
        }
        // hàm check nhiều cột đầy chưa, trả về các cột đã đầy
        public int[] Check_full_cols(int[] cols)
        {
           bool[] found = new bool[cols.Length];
            for (int idx = 0; idx < cols.Length; idx++)
            {
                int col = cols[idx];
                found[idx] = true;
                for (int i = 0; i < 8; i++)
                {
                    if (Board[i, col] == false)
                    {
                        found[idx] = false;
                        break;
                    }
                }
            }
            return found.Select((value, index) => new { value, index })
                        .Where(x => x.value)
                        .Select(x => cols[x.index])
                        .ToArray();
        }

        //hàm clear nhiều cột
        public void clear_cols(int[] cols)
        {
            if (cols.Length == 0) return;
            foreach (int col in cols)
                for (int i = 0; i < 8; i++)
                    Board[i,col] = false;
        }
    }
}
