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

            PictureBox btnSetting = new PictureBox();
            btnSetting.Image = Properties.Resources.setting;
            btnSetting.SizeMode = PictureBoxSizeMode.Zoom;
            btnSetting.Cursor = Cursors.Hand;
            btnSetting.Size = new Size(320, 128); 
            btnSetting.BackColor = Color.Transparent;
            this.Controls.Add(btnSetting);

            PictureBox btnExit = new PictureBox();
            btnExit.Image = Properties.Resources.quit;
            btnExit.SizeMode = PictureBoxSizeMode.Zoom;
            btnExit.Cursor = Cursors.Hand;
            btnExit.Size = new Size(320, 128); 
            btnExit.BackColor = Color.Transparent;
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);

            Action positionControls = () =>
            {
                
                int logoWidth = (int)(this.ClientSize.Width * 0.6);
                logoWidth = Math.Max(240, Math.Min(logoWidth, 720));
                int logoHeight = logoWidth / 3;
                logo.Size = new Size(logoWidth, logoHeight);

                int btnWidth = (int)(this.ClientSize.Width * 0.4); 
                btnWidth = Math.Max(240, Math.Min(btnWidth, 420)); 
                int btnHeight = (int)(btnWidth * 0.4); 
                btnPlay.Size = new Size(btnWidth, btnHeight);
                btnSetting.Size = new Size(btnWidth, btnHeight);
                btnExit.Size = new Size(btnWidth, btnHeight);

                int centerXButtons = (this.ClientSize.Width - btnWidth) / 2;


                int spacing = Math.Max(6, btnHeight / 12);
                int totalButtonsHeight = 3 * btnHeight + 2 * spacing;
                int minTopBelowLogo = logo.Bounds.Bottom + 20;
                int startY = Math.Max(minTopBelowLogo, (this.ClientSize.Height - totalButtonsHeight) / 2);

                btnPlay.Location = new Point(centerXButtons, startY);
                btnSetting.Location = new Point(centerXButtons, startY + btnHeight + spacing);
                btnExit.Location = new Point(centerXButtons, startY + 2 * (btnHeight + spacing));

                int centerXLogo = (this.ClientSize.Width - logo.Width) / 2;
                int logoY = Math.Max(20, startY - logo.Height - 20);
                logo.Location = new Point(centerXLogo, logoY);
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
            
            AudioManager.Play("click");
            fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 20;
            fadeOutTimer.Tick += FadeOutTimer_Tick;
            fadeOutTimer.Start();
        }
        private Timer fadeExitTimer;

        private void BtnExit_Click(object sender, EventArgs e)
        {
            
            AudioManager.Play("click");
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
                    AudioManager.Play("click");
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
