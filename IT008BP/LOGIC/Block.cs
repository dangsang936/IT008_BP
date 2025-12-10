using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC
{
    // lớp này chỉ định 1 block cụ thể đang hiện trên màn hình
    public class Block
    {
        public int X {get; set;}
        public int Y {get; set;}
        public BlockData data { get; set; }
        Block()
        {
            //mặc định X,Y có tọa độ -1 tức là block đang ở ngoài, chưa đăt vào bảng
            int X = -1; 
            int Y = -1;
        }
    }
}
