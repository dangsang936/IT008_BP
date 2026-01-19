using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public class gameoverOverlay : Panel
    {
        PictureBox gameover;
        PictureBox btnRetry;
        PictureBox btnMenu;
        Label quoteLabel;
        public event Action RetryClicked;
        public event Action MenuClicked;
        static readonly string[] QuotesBeatHighScore =
    {
        "You just got lucky this time, I dare you to try again!",
        "CoNGratuLaTiON!!!",
        "It's easy mode, don't be too excited!",
        "Well-Played! Wanna beat the highscore again?",
        "Damn, you are getting better and better o.o",
        "System Overclocked! Luckily for you!",
        "You are unstoppable! Keep it up!",
    };
        static readonly string[] QuotesNotBeatHightScore =
    {
        "Nice try. My grandma could do better, and she’s not even playing!",
        "You only lose when you stop trying.",
        "Every defeat makes you stronger.",
        "ERROR 404: Skill not found!!!",
        "Try again. You’re closer than you think.",
        "Even the best players fail sometimes. Give it another try",
        "Does your brain require a firmware update?",
        "That was a great performance... for a beginner",
        "Go touch some grass bro..."
    };


        static readonly Random rng = new Random();

        public gameoverOverlay(int currscore, int highscore)
        {
            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;
            DoubleBuffered = true;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true
            );

            // GAME OVER
            gameover = new PictureBox
            {
                Image = Properties.Resources.gameover,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            //QUOTE
            
            quoteLabel = new Label
            {
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Font = new Font("Segoe UI", 14f, FontStyle.Italic),
            };
            quoteLabel.Text = currscore >= highscore ?
                QuotesBeatHighScore[rng.Next(QuotesBeatHighScore.Length)] :
                QuotesNotBeatHightScore[rng.Next(QuotesNotBeatHightScore.Length)];

            // RETRY
            btnRetry = new PictureBox
                {
                    Image = Properties.Resources.retry,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Cursor = Cursors.Hand,
                    BackColor = Color.Transparent
                };

            // MENU
            btnMenu = new PictureBox
            {
                Image = Properties.Resources.menu,
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };

            Controls.Add(gameover);
            Controls.Add(btnRetry);
            Controls.Add(btnMenu);
            Controls.Add(quoteLabel);

            btnRetry.Click += (s, e) => RetryClicked?.Invoke();
            btnMenu.Click += (s, e) => MenuClicked?.Invoke();

            Resize += (s, e) => PositionControls();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Brush b = new SolidBrush(Color.FromArgb(140, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(b, ClientRectangle);
            }
        }

        void PositionControls()
        {
            int w = Width;
            int h = Height;

            int logoW = Clamp((int)(w * 0.82), 480, 860);
            int logoH = logoW / 3;
            gameover.Size = new Size(logoW, logoH);

            int btnW = Clamp((int)(w * 0.30), 220, 360);
            int btnH = (int)(btnW * 0.38);
            btnRetry.Size = btnMenu.Size = new Size(btnW, btnH);

            int quoteMaxWidth = Clamp((int)(w * 0.70), 400, 900);
            int quoteHeight = 80;

            int spaceLogoQuote = 10;
            int spaceQuoteBtn = 18;
            int spaceBtn = 24;

            int totalHeight =
                logoH +
                spaceLogoQuote +
                quoteHeight +
                spaceQuoteBtn +
                btnH;

            int startY = (h - totalHeight) / 2;

            // GAME OVER
            gameover.Location = new Point(
                (w - logoW) / 2,
                startY
            );

            // QUOTE
            quoteLabel.Size = new Size(quoteMaxWidth, quoteHeight);
            quoteLabel.Location = new Point(
                (w - quoteMaxWidth) / 2,
                gameover.Bottom + spaceLogoQuote
            );

            // BUTTONS
            int totalBtnW = btnW * 2 + spaceBtn;
            int startX = (w - totalBtnW) / 2;
            int btnY = quoteLabel.Bottom + spaceQuoteBtn;

            btnRetry.Location = new Point(startX, btnY);
            btnMenu.Location = new Point(startX + btnW + spaceBtn, btnY);
        }


        int Clamp(int v, int min, int max)
            => v < min ? min : (v > max ? max : v);
    }
}
