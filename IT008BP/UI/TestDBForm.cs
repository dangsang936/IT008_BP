using System;
using System.Windows.Forms;
using DATA; 

namespace UI
{
    public partial class TestDBForm : Form
    {
        private DataHelper db;

        public TestDBForm()
        {
            InitializeComponent();
            db = new DataHelper("game_data.json");
            lblHighscore.Text = "Highscore: " + db.GetHighscore();
        }

        private void btnSaveScore_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtScore.Text, out int score))
            {
                db.UpdateHighscore(score);
                lblHighscore.Text = "Highscore: " + db.GetHighscore();
                MessageBox.Show("Saved!");
            }
            else
            {
                MessageBox.Show("Điểm phải là số nguyên!");
            }
        }
    }
}
