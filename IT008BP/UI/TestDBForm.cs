using System;
using System.Linq;
using System.Windows.Forms;
using DATA; 

namespace UI
{
    public class TestDBForm : Form
    {
        private DataHelper helper;

        private TextBox txtUsername;
        private Button btnCreateUser;
        private TextBox txtScore;
        private Button btnAddScore;
        private TextBox txtUserID;
        private Button btnViewUser;
        private ListBox lstLeaderboard;
        private ListBox lstUserScores;
        private Label lblCurrentUser;

        private int currentUserId = -1;

        public TestDBForm()
        {
            helper = new DataHelper("BlockBlastData.json");
            InitializeComp();
        }

        private void InitializeComp()
        {
            // Username
            txtUsername = new TextBox { Left = 20, Top = 20, Width = 150 };
            btnCreateUser = new Button { Left = 180, Top = 18, Text = "Tạo User" };
            btnCreateUser.Click += BtnCreateUser_Click;

            // Score
            txtScore = new TextBox { Left = 20, Top = 60, Width = 150 };
            btnAddScore = new Button { Left = 180, Top = 58, Text = "Thêm Score" };
            btnAddScore.Click += BtnAddScore_Click;

            // UserID để xem info
            txtUserID = new TextBox { Left = 20, Top = 100, Width = 150 };
            btnViewUser = new Button { Left = 180, Top = 98, Text = "Xem User" };
            btnViewUser.Click += BtnViewUser_Click;

            // Label hiện user hiện tại
            lblCurrentUser = new Label { Left = 20, Top = 130, Width = 300, Text = "Chưa chọn user" };

            // ListBox Leaderboard
            lstLeaderboard = new ListBox { Left = 20, Top = 160, Width = 300, Height = 120 };

            // ListBox điểm của user
            lstUserScores = new ListBox { Left = 20, Top = 290, Width = 300, Height = 120 };

            // Add controls vào form
            this.Controls.Add(txtUsername);
            this.Controls.Add(btnCreateUser);
            this.Controls.Add(txtScore);
            this.Controls.Add(btnAddScore);
            this.Controls.Add(txtUserID);
            this.Controls.Add(btnViewUser);
            this.Controls.Add(lblCurrentUser);
            this.Controls.Add(lstLeaderboard);
            this.Controls.Add(lstUserScores);

            // Form
            this.Text = "Test Database Form";
            this.Width = 360;
            this.Height = 460;
        }

        private void BtnCreateUser_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Nhập username!");
                return;
            }

            try
            {
                currentUserId = helper.CreateUser(username);
                lblCurrentUser.Text = $"Current User: {username} (ID: {currentUserId})";
                UpdateLeaderboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddScore_Click(object sender, EventArgs e)
        {
            if (currentUserId == -1)
            {
                MessageBox.Show("Chưa tạo user!");
                return;
            }

            if (!int.TryParse(txtScore.Text.Trim(), out int score))
            {
                MessageBox.Show("Nhập số điểm hợp lệ!");
                return;
            }

            try
            {
                helper.AddScore(currentUserId, score);
                var user = helper.GetUserInfo(currentUserId);
                lblCurrentUser.Text = $"Current User: {user.Username} | Highscore: {user.Highscore}";
                UpdateLeaderboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnViewUser_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUserID.Text.Trim(), out int userId))
            {
                MessageBox.Show("Nhập UserID hợp lệ!");
                return;
        }

            try
        {
                var user = helper.GetUserInfo(userId);
                lblCurrentUser.Text = $"User: {user.Username} | Highscore: {user.Highscore}";
                lstUserScores.Items.Clear();
                foreach (var score in user.Scores)
            {
                    lstUserScores.Items.Add($"ScoreID: {score.ScoreID} | Score: {score.Score} | Ngày: {score.PlayDate}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

        private void UpdateLeaderboard()
        {
            lstLeaderboard.Items.Clear();
            var top = helper.GetLeaderboard();
            foreach (var p in top)
            {
                lstLeaderboard.Items.Add($"ID: {p.UserID} | {p.Username} | Highscore: {p.Highscore}");
            }
        }
    }
}
