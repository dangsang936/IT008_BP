using System.Windows.Forms;

namespace UI
{
    partial class TestDBForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblHighscore;
        private TextBox txtScore;
        private Button btnSaveScore;
        private Button btnLoad;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.lblHighscore = new Label();
            this.txtScore = new TextBox();
            this.btnSaveScore = new Button();

            // lblHighscore
            this.lblHighscore.AutoSize = true;
            this.lblHighscore.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblHighscore.Location = new System.Drawing.Point(30, 20);
            this.lblHighscore.Text = "Highscore: 0";

            // txtScore
            this.txtScore.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtScore.Location = new System.Drawing.Point(30, 60);
            this.txtScore.Size = new System.Drawing.Size(150, 29);

            // btnSaveScore
            this.btnSaveScore.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnSaveScore.Location = new System.Drawing.Point(30, 100);
            this.btnSaveScore.Size = new System.Drawing.Size(150, 40);
            this.btnSaveScore.Text = "Save Score";
            this.btnSaveScore.Click += new System.EventHandler(this.btnSaveScore_Click);

            // Thêm controls
            this.Controls.Add(this.lblHighscore);
            this.Controls.Add(this.txtScore);
            this.Controls.Add(this.btnSaveScore);
            this.Controls.Add(this.btnLoad);

            // Form
            this.ClientSize = new System.Drawing.Size(360, 170);
            this.Text = "Test DBForm ";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
