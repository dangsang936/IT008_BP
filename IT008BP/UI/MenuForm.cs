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
            this.Activated += (s, e) =>
            {
                AudioManager.PlayBGM("menu");
            };

            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(23, 86, 191);
            this.ClientSize = new Size(800, 600);
            this.DoubleBuffered = true;
            this.BackgroundImage = Properties.Resources.bg;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            PictureBox logo = new PictureBox();
            logo.Image = Properties.Resources.logo;
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Size = new Size(648, 216);
            logo.Cursor = Cursors.Hand;
            logo.BackColor = Color.Transparent;
            this.Controls.Add(logo);

            PictureBox btnPlay = new PictureBox();
            btnPlay.Image = Properties.Resources.play;
            btnPlay.SizeMode = PictureBoxSizeMode.Zoom;
            btnPlay.Size = new Size(320, 128);
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.BackColor = Color.Transparent;
            btnPlay.Click += BtnPlay_Click;
            this.Controls.Add(btnPlay);

            /*PictureBox btnSetting = new PictureBox();
            btnSetting.Image = Properties.Resources.setting;
            btnSetting.SizeMode = PictureBoxSizeMode.Zoom;
            btnSetting.Cursor = Cursors.Hand;
            btnSetting.Size = new Size(320, 128); 
            btnSetting.BackColor = Color.Transparent;
            this.Controls.Add(btnSetting);*/

            PictureBox btnExit = new PictureBox();
            btnExit.Image = Properties.Resources.quit;
            btnExit.SizeMode = PictureBoxSizeMode.Zoom;
            btnExit.Cursor = Cursors.Hand;
            btnExit.Size = new Size(320, 128); 
            btnExit.BackColor = Color.Transparent;
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);
            int Clamp(int value, int min, int max)
            {
                if (value < min) return min;
                if (value > max) return max;
                return value;
            }

            Action positionControls = () =>
            {
                int formW = this.ClientSize.Width;
                int formH = this.ClientSize.Height;

                /* ===== LOGO (RẤT TO) ===== */
                int logoWidth = (int)(formW * 0.82);
                logoWidth = Clamp(logoWidth, 480, 860);
                int logoHeight = logoWidth / 3;

                logo.Size = new Size(logoWidth, logoHeight);

                /* ===== BUTTONS (PLAY = QUIT) ===== */
                int btnWidth = (int)(formW * 0.45);
                btnWidth = Clamp(btnWidth, 300, 480);
                int btnHeight = (int)(btnWidth * 0.38);

                btnPlay.Size = new Size(btnWidth, btnHeight);
                btnExit.Size = new Size(btnWidth, btnHeight);

                int spaceLogoBtn = 35;
                int spaceBtn = 20;

                int totalHeight =
                    logoHeight +
                    spaceLogoBtn +
                    btnHeight +
                    spaceBtn +
                    btnHeight;

                int startY = (formH - totalHeight) / 2;

                logo.Location = new Point(
                    (formW - logoWidth) / 2,
                    startY
                );

                btnPlay.Location = new Point(
                    (formW - btnWidth) / 2,
                    logo.Bottom + spaceLogoBtn
                );

                btnExit.Location = new Point(
                    (formW - btnWidth) / 2,
                    btnPlay.Bottom + spaceBtn
                );
            };


            this.Load += (s, e) => positionControls();
            this.Resize += (s, e) => positionControls();

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
            
            AudioManager.Play("create");
            fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 20;
            fadeOutTimer.Tick += FadeOutTimer_Tick;
            fadeOutTimer.Start();
        }
        private Timer fadeExitTimer;

        private void BtnExit_Click(object sender, EventArgs e)
        {
            
            AudioManager.Play("create");
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
                maingame game = new maingame();
                game.FormClosed += (s2, e2) =>
                {
                    AudioManager.Play("create");
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
