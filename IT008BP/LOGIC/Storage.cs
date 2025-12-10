using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC
{
    //lớp này để làm kho chứa tất cả các loại block và túi có sẵn  
    public class Storage
    {
        // tất cả các tên của loại khối quay 0 độ
        public string[] Type_Name = { "dot", "I2", "I3", "I4", "I5", "O", "O3", "T", "L", "L3", "J", "J3", "S", "Z" };

        // tất cả các góc quay
        public int[] Rotation_Array = { 0, 90, 180, 270 };

        // lưu trữ tất cả các khối với tên và góc quay tương ứng
        // key: (tên khối, góc quay), value: dữ liệu khối
        public Dictionary<(string,int), BlockData> Shapes {  get; set; }

        //lưu trữ tổng tỉ lệ cho từng loại block
        //key: tên khối, value: tỉ lệ
        public Dictionary<string, int> Rate {  get; set; }

        public Queue<BlockData> Bag {  get; set; }

        public Storage()
        {
            //Khởi tạo các khối trong nhà máy khối
            Shapes = new Dictionary<(string, int), BlockData>();
            foreach (string name in Type_Name)
            {               
                for (int i = 0; i < Rotation_Array.Length; i++)
                {
                    BlockData block = new BlockData(name);
                    block.Rotate(Rotation_Array[i]);
                    Shapes.Add((name, Rotation_Array[i]), block);                   
                }

            }

            //Khởi tạo tỉ lệ xuất hiện cho từng loại khối
            Rate = new Dictionary<string, int>();
            // --- NHÓM 1: CỨU HỘ & DỄ (Xuất hiện nhiều nhất) ---
            Rate.Add("dot", 18); // 1x1: Cần thiết nhất để sửa sai
            Rate.Add("I2", 15); // 1x2: Rất dễ đặt
            Rate.Add("O", 14); // 2x2: Dễ tạo cụm vuông

            // --- NHÓM 2: PHỔ THÔNG (Xuất hiện vừa phải) ---
            Rate.Add("I3", 12); // 1x3: Dùng dọn hàng tốt
            Rate.Add("T", 10); // Chữ T
            Rate.Add("L", 10); // Chữ L
            Rate.Add("J", 10); // Chữ J

            // --- NHÓM 3: KHÓ CHỊU (Xuất hiện ít hơn) ---
            Rate.Add("S", 8); // Zic-zac khó chịu
            Rate.Add("Z", 8); // Zic-zac ngược
            Rate.Add("I4", 6); // Thanh dài 4 ô (Tetris bar)

            // --- NHÓM 4: KHỐI KHỔNG LỒ (Hiếm - Để thử thách) ---
            Rate.Add("L3", 5); // Chữ L to (3x3)
            Rate.Add("J3", 5); // Chữ J to (3x3)
            Rate.Add("I5", 3); // Thanh dài 5 ô (Rất khó đặt)
            Rate.Add("O3", 2); // Khối vuông 3x3 (Sát thủ bàn chơi)
        }

        public BlockData GetRandomBlock()
        {
            string name="dot";
            Random r = new Random();
            int cur_rate = 1;
            int random_num = r.Next(100) + 1; // khởi tạo 1 số random từ 1 -> 100
            foreach (var tmp in Rate)
            {
                cur_rate += tmp.Value;
                if(cur_rate > random_num)
                {
                    name = tmp.Key; // lấy tên của nhóm block
                    break;
                }    
            }

            // khởi tạo 1 góc độ quay ngẫu nhiên 0 --> 3 tương đương với 0 -> 270 độ
            int random_rotation = r.Next(4);
            return Shapes[(name,random_rotation * 4)];
        }

        //hàm thêm 15 khối vào bag
        public void Add_To_Bag()
        {
            for (int i = 0; i < 15; i++)
            {
                Bag.Enqueue(GetRandomBlock());
            }
        }
    }
}
