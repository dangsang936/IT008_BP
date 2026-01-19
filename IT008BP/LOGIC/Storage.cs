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

        // KHAI BÁO đối tượng Random MỘT LẦN duy nhất
        private Random rand;

        public Storage()
        {
            // KHỞI TẠO đối tượng Random MỘT LẦN duy nhất
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
            Rate.Add("dot", 8); // 1x1: Cần thiết nhất để sửa sai
            Rate.Add("I2", 8); // 1x2: Rất dễ đặt
            Rate.Add("O", 8); // 2x2: Dễ tạo cụm vuông

            // --- NHÓM 2: PHỔ THÔNG ---
            Rate.Add("I3", 9); // 1x3: Dùng dọn hàng tốt
            Rate.Add("T", 9); // Chữ T
            Rate.Add("L", 9); // Chữ L
            Rate.Add("J", 9); // Chữ J

            // --- NHÓM 3: KHÓ CHỊU  ---
            Rate.Add("S", 7); // Zic-zac khó chịu
            Rate.Add("Z", 7); // Zic-zac ngược
            Rate.Add("I4", 6); // Thanh dài 4 ô 

            // --- NHÓM 4: KHỐI KHỔNG LỒ  ---
            Rate.Add("L3", 5); // Chữ L to (3x3)
            Rate.Add("J3", 5); // Chữ J to (3x3)
            Rate.Add("I5", 5); // Thanh dài 5 
            Rate.Add("O3", 5); // Khối vuông 3x3
        }

        public BlockData GetRandomBlock()
        {
            string name = "dot";
            // SỬ DỤNG đối tượng 'rand' đã được khởi tạo MỘT LẦN
            int cur_rate = 1;
            int random_num = rand.Next(100) + 1; // khởi tạo 1 số random từ 1 -> 100
            foreach (var tmp in Rate)
            {
                cur_rate += tmp.Value;
                if (cur_rate > random_num)
                {
                    name = tmp.Key; // lấy tên của nhóm block
                    break;
                }
            }

            // khởi tạo 1 góc độ quay ngẫu nhiên 0 --> 3 tương đương với 0 -> 270 độ
            int random_rotation = rand.Next(4);
            int rot = Rotation_Array[random_rotation];

            return Shapes[(name, rot)];
        }

        //hàm thêm 15 khối vào bag
        public void Add_To_Bag()
        {
            for (int i = 0; i < 15; i++)
            {
                Bag.Enqueue(GetRandomBlock());
            }
        }
        public bool check_Bag_Empty()
        {
            return Bag.Count == 0;
        }
    }
}