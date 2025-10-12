//test xong audio rồi, chạy khá là ổn, hiện tại chỉ có các hàm để gọi ra âm thanh, còn gọi khi nào thì phụ thuộc vào logic game
// pickup: tiếng khi nhặt 1 block lên
//put: tiếng khi đặt block xuống
//score: loop tiếng này đến khi đến score cuối cùng khi game over
//nghĩa là khi game over,lưu số điểm cuối cùng, nó sẽ loop tiếng score này từ 0 đến khi nào score cuối cùng hiện ra thì dừng lại
//create: tiếng này khi game bắt đầu (cho animation các block trôi qua grid từ dưới lên trên)
//gameover: kẹt block thì xuất hiện 1 lần
//all clear: clear hết block thì xuất hiện 1 lần
//click: tiếng khi bấm vào các elements ví dụ như nút start game, retry, tắt âm, mở âm,....
//khi clear được 1 hàng thì sẽ bắt đầu từ combo1 đến combo8, combo8 là max, dù có nhảy thêm bao nhiêu combo nữa thì vẫn dùng sound combo8.
//nếu đặt quá 5 block mới thì combo sẽ bị hủy, bắt đầu lại từ combo1

using System;
using System.Windows.Forms;

namespace UI
{
    public partial class TestAudioManagerForm : Form
    {
        private Button btnNextCombo;
        private string[] comboList = new string[]
        {
            "combo1", "combo2", "combo3", "combo4",
            "combo5", "combo6", "combo7", "combo8"
        };

        private int currentComboIndex = 0;

        public TestAudioManagerForm()
        {
            InitializeComponent();
            InitializeControls();
            LoadComboSounds();
        }

        private void InitializeControls()
        {
            btnNextCombo = new Button();
            btnNextCombo.Text = "Phát Combo tiếp theo";
            btnNextCombo.Location = new System.Drawing.Point(20, 20);
            btnNextCombo.Size = new System.Drawing.Size(200, 40);
            btnNextCombo.Click += (s, e) => PlayNextCombo();
            this.Controls.Add(btnNextCombo);
        }

        private void LoadComboSounds()
        {
            AudioManager.LoadFolder("Sounds/SFX");
        }

        private void PlayNextCombo()
        {
            if (currentComboIndex >= comboList.Length)
            {
                btnNextCombo.Enabled = false;
                MessageBox.Show("Đã phát hết tất cả combo!");
                return;
            }

            string combo = comboList[currentComboIndex];
            AudioManager.Play(combo);
            currentComboIndex++;
        }
    }
}
