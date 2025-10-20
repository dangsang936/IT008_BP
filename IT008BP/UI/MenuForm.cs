using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class MenuForm : Form
    {
        Timer fadeOutTimer;

        public MenuForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(25, 25, 35); 
            this.ClientSize = new Size(800, 600); 
            this.DoubleBuffered = true;

            Label lblTitle = new Label
            {
                Text = "BLOCK BLAST",
                Font = new Font("Impact", 36, FontStyle.Bold),
                ForeColor = Color.DeepSkyBlue,
                AutoSize = true,
                Location = new Point((this.ClientSize.Width / 2) - 200, 80)
            };
            this.Controls.Add(lblTitle);

            PictureBox btnPlay = new PictureBox();
            btnPlay.Image = Properties.Resources.play;
            btnPlay.SizeMode = PictureBoxSizeMode.Zoom;
            btnPlay.Size = new Size(200, 80);
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.BackColor = Color.Transparent;
            btnPlay.Click += BtnPlay_Click;
            this.Controls.Add(btnPlay);

            PictureBox btnSetting = new PictureBox();
            btnSetting.Image = Properties.Resources.setting;
            btnSetting.SizeMode = PictureBoxSizeMode.Zoom;
            btnSetting.Cursor = Cursors.Hand;
            btnSetting.Size = new Size(200, 80);
            btnSetting.BackColor = Color.Transparent;
            this.Controls.Add(btnSetting);

            PictureBox btnExit = new PictureBox();
            btnExit.Image = Properties.Resources.quit;
            btnExit.SizeMode = PictureBoxSizeMode.Zoom;
            btnExit.Cursor = Cursors.Hand;
            btnExit.Size = new Size(200, 80);
            btnExit.BackColor = Color.Transparent;
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);

            this.Load += (s, e) =>
            {
                lblTitle.Location = new Point(
                    (this.ClientSize.Width - lblTitle.Width) / 2,
                    80
                );
                int centerX = (this.ClientSize.Width - btnPlay.Width) / 2;
                btnPlay.Location = new Point(centerX, 220);
                btnSetting.Location = new Point(centerX, 320);
                btnExit.Location = new Point(centerX, 420);
            };
            this.Opacity = 0;
            Timer fadeIn = new Timer();
            fadeIn.Interval = 20;
            fadeIn.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    fadeIn.Stop();
            };
            fadeIn.Start();

        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 20; 
            fadeOutTimer.Tick += FadeOutTimer_Tick;
            fadeOutTimer.Start();
        }
        private Timer fadeExitTimer;

        private void BtnExit_Click(object sender, EventArgs e)
        {
            fadeExitTimer = new Timer();
            fadeExitTimer.Interval = 20; 
            fadeExitTimer.Tick += FadeExitTimer_Tick;
            fadeExitTimer.Start();
        }

        private void FadeExitTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.05;
            }
            else
            {
                fadeExitTimer.Stop();
                Application.Exit();
            }
        }

        private void FadeOutTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.05; 
            }
            else
            {
                fadeOutTimer.Stop();
                Form1 game = new Form1();
                game.FormClosed += (s2, e2) =>
                {
                    this.Opacity = 0; 
                    this.Show();
                    Timer fadeInTimer = new Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s3, e3) =>
                    {
                        if (this.Opacity < 1)
                            this.Opacity += 0.05;
                        else
                            fadeInTimer.Stop();
                    };
                    fadeInTimer.Start();
                };

                game.Show();
                this.Hide();
            }
        }
    }
}
