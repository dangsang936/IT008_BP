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
        public Dictionary<(string, int), BlockData> Shapes { get; set; }

        //lưu trữ tổng tỉ lệ cho từng loại block
        //key: tên khối, value: tỉ lệ
        public Dictionary<string, int> Rate { get; set; }

        public Queue<BlockData> Bag { get; set; }

        // khai báo đối tượng Random
        private Random rand;

        public Storage()
        {
            // khởi tạo đối tượng Random
            rand = new Random();

            Bag = new Queue<BlockData>();

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
            // --- NHÓM 1: CỨU HỘ & DỄ  ---
            Rate.Add("dot", 2); // 1x1: Cần thiết nhất để sửa sai
            Rate.Add("I2", 2); // 1x2: Rất dễ đặt
            Rate.Add("O", 2); // 2x2: Dễ tạo cụm vuông

            // --- NHÓM 2: PHỔ THÔNG ---
            Rate.Add("I3", 3); // 1x3: Dùng dọn hàng tốt
            Rate.Add("T", 3); // Chữ T
            Rate.Add("L", 3); // Chữ L
            Rate.Add("J", 3); // Chữ J

            // --- NHÓM 3: KHÓ CHỊU  ---
            Rate.Add("S", 2); // Zic-zac khó chịu
            Rate.Add("Z", 3); // Zic-zac ngược
            Rate.Add("I4", 3); // Thanh dài 4 ô 

            // --- NHÓM 4: KHỐI KHỔNG LỒ  ---
            Rate.Add("L3", 1); // Chữ L to (3x3)
            Rate.Add("J3", 1); // Chữ J to (3x3)
            Rate.Add("I5", 1); // Thanh dài 5 
            Rate.Add("O3", 1); // Khối vuông 3x3
        }

        public void Add_To_Bag()
        {
            //Tạo một danh sách tạm thời (cái túi)
            List<string> temp_bag = new List<string>();

            //Đổ khối vào túi dựa trên tỉ lệ đã định nghĩa
            foreach (var item in Rate)
            {
                int count = item.Value;
                for (int i = 0; i < count; i++)
                {
                    temp_bag.Add(item.Key);
                }
            }

            //Tráo đổi thứ tự túi bằng thuật toán Fisher-Yates
            int n = temp_bag.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                string value = temp_bag[k];
                temp_bag[k] = temp_bag[n];
                temp_bag[n] = value;
            }

            //Đưa các khối từ túi đã tráo vào hàng đợi Bag của game
            foreach (string name in temp_bag)
            {
                int random_rotation = rand.Next(4);
                int rot = Rotation_Array[random_rotation];
                if (Shapes.ContainsKey((name, rot)))
                {
                    Bag.Enqueue(Shapes[(name, rot)]);
                }
            }
        }
        public bool check_Bag_Empty()
        {
            return Bag.Count == 0;
        }
    }
}