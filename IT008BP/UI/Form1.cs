using DATA;
using LOGIC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
            this.UpdateStyles();
        }

    }

    public partial class maingame : Form
    {
        private BufferedPanel board;
        private FlowLayoutPanel blockPanel;
        private PictureBox preview1, preview2, preview3;
        private Label scorePanel;
        private Label highScorePanel;
        private int currScore = 0;
        private int highScore = 0;
        private DATA.DataHelper dataHelper;

        private const int rows = 8;
        private const int cols = 8;

        private float cellSize => board.ClientSize.Width / (float)cols;

        private List<Point> highlightCells = new List<Point>();
        bool highlightValid = false;

        private MainBoard gameLogic;
        private BlockData blockData;
        private Block draggingBlock = null;
        private int draggingBlockSourceIndex = -1;

        private readonly Color PreviewBlockColor = Color.FromArgb(255, 248, 220); 
        private readonly Color BoardBlockColor = Color.FromArgb(218, 170, 40); 
        private readonly Color BlockBorderColor = Color.DarkGoldenrod;


        private Block block;
        private BlockData[] previewBlocks = new BlockData[3];
        private Storage storage = new Storage();

        private Bitmap backBuffer;
        private Graphics bufferG;

        private Timer waveTimer;
        private int waveIndex = 0;
        private List<List<Point>> waveLayers = new List<List<Point>>();
        private bool isWaveClearing = false;

        bool bgmOn = true;
        bool sfxOn = true;


        public maingame()
        {
            InitializeComponent();
            
            UpdateStyles();

            gameLogic = new MainBoard();
            blockData = new BlockData("J");
            block = new Block();
            SetupUI();

            waveTimer = new Timer();
            waveTimer.Interval = 40; 
            waveTimer.Tick += WaveTick;
            dataHelper = new DATA.DataHelper("gameScore.db");
            highScore = dataHelper.GetHighscore();

            try
            {
                AudioManager.PlayBGM("BGM");

            }
            catch { }

            this.FormClosed += (s, e) =>
            {
                try
                {
                    AudioManager.Stop("BGM");
                }
                catch { }
            };

        }
        private void InitBuffer()
        {
            backBuffer?.Dispose();
            bufferG?.Dispose();

            backBuffer = new Bitmap(board.Width, board.Height);
            bufferG = Graphics.FromImage(backBuffer);

            bufferG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            bufferG.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
        }

        public void SetupUI()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(62, 87, 153);
            this.ClientSize = new Size(900, 600);

            //===Board===
            board = new BufferedPanel
            {
                Size = new Size(500, 500),
                Location = new Point(18, 40),
                BackColor = Color.FromArgb(34, 49, 81)
            };
            this.Controls.Add(board);

            board.Paint += PictureBoxBoard_Paint;
            board.Resize += (s, e) => board.Invalidate();
            board.MouseMove += board_MouseMove;
            board.MouseClick += board_MouseClick;
            board.MouseLeave += (s, e) =>
            {
                highlightCells.Clear();
                board.Invalidate();
            };

            //===Score Panel===
            scorePanel = new Label
            {
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(board.Right + 120, board.Top + 120),
                ForeColor = Color.White,
                AutoSize = true,
                Padding = new Padding(10),
                Text = "Score: 0"
            };
            this.Controls.Add(scorePanel);

            highScorePanel = new Label
            {
                Font = new Font("Segoe UI", 25, FontStyle.Bold),
                Location = new Point(board.Left,board.Bottom),
                ForeColor = Color.White,
                AutoSize = true,
                Padding = new Padding(0),
                Text = $"Highest Score: {highScore}"
            };
            this.Controls.Add(highScorePanel);

            //===Previews===
            blockPanel = new FlowLayoutPanel
            {
                Size = new Size(300, 150),
                Location = new Point(board.Right + 20, board.Top),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(5),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(45, 63, 104)
            };
            this.Controls.Add(blockPanel);

            preview1 = CreatePreviewBox();
            preview2 = CreatePreviewBox();
            preview3 = CreatePreviewBox();

            blockPanel.Controls.Add(preview1);
            blockPanel.Controls.Add(preview2);
            blockPanel.Controls.Add(preview3);
            GeneratePreviewBlocks();

            board.Resize += (s, e) =>
            {
                InitBuffer();
                board.Invalidate();
            };

            PictureBox btnBGM = new PictureBox
            {
                Size = new Size (75, 75),
                Location = new Point(board.Right + 170, scorePanel.Bottom + 300),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Cursor = Cursors.Hand,
                Image = AudioManager.IsBGMMuted()
                    ? Properties.Resources.music_off
                    : Properties.Resources.music_on
            };

            PictureBox btnSFX = new PictureBox
            {
                Size = new Size(75, 75),
                Location = new Point(board.Right + 270, scorePanel.Bottom + 300),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Cursor = Cursors.Hand,
                Image = AudioManager.IsSFXMuted()
                    ? Properties.Resources.mute
                    : Properties.Resources.unmute
            };

            this.Controls.Add(btnBGM);
            this.Controls.Add(btnSFX);

            bool bgmOn = true;
            bool sfxOn = true;

            btnBGM.Click += (s, e) =>
            {
                AudioManager.ToggleBGM();
                btnBGM.Image = AudioManager.IsBGMMuted()
                    ? Properties.Resources.music_off
                    : Properties.Resources.music_on;
            };

            btnSFX.Click += (s, e) =>
            {
                AudioManager.ToggleSFX();
                btnSFX.Image = AudioManager.IsSFXMuted()
                    ? Properties.Resources.mute
                    : Properties.Resources.unmute;
            };

            InitBuffer();

        }

        private void StartWaveClear()
        {
            waveLayers.Clear();
            waveIndex = 0;
            isWaveClearing = true;
            AudioManager.Play("score");
            foreach (int r in gameLogic.CheckRowsFull())
            {
                for (int x = 0; x < cols; x++)
                {
                    if (waveLayers.Count <= x)
                        waveLayers.Add(new List<Point>());

                    waveLayers[x].Add(new Point(x, r));
                }
            }
            int offset = waveLayers.Count;
            foreach (int c in gameLogic.CheckColumnsFull())
            {
                for (int y = 0; y < rows; y++)
                {
                    int idx = offset + y;
                    if (waveLayers.Count <= idx)
                        waveLayers.Add(new List<Point>());

                    waveLayers[idx].Add(new Point(c, y));
                }
            }

            waveTimer.Start();
        }


        private void WaveTick(object sender, EventArgs e)
        {
            waveIndex++;
            if (waveIndex >= waveLayers.Count)
            {
                waveTimer.Stop();
                isWaveClearing = false;

                gameLogic.Clear(); 
                waveLayers.Clear();
                board.Invalidate();
                if (IsAllClear()) {
                    HandleAllClear();
                }
                if (gameOver())
                {
                    waveTimer.Stop();
                    waveTimer.Dispose();
                    MessageBox.Show($"Điểm số cuối cùng: {currScore}","GAME OVER",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    if (currScore > highScore)
                        dataHelper.UpdateHighscore(currScore);

                    this.Close();
                }
                return;
            }
            board.Invalidate();
        }


        public void GeneratePreviewBlocks()
        {
            for (int i = 0; i < 3; i++)
                previewBlocks[i] = storage.GetRandomBlock(); 

            preview1.Invalidate();
            preview2.Invalidate();
            preview3.Invalidate();
        }

        private void board_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (draggingBlock == null) return;

                int col = (int)(e.X / cellSize);
                int row = (int)(e.Y / cellSize);

                draggingBlock.X = col;
                draggingBlock.Y = row;

                if (gameLogic.Can_Place(draggingBlock))
                {
                    //===PLACE===
                    gameLogic.Place(draggingBlock);
                    UpdateScore(10);

                    //===CLEAR ROW/COL===
                    int rowsCleared = gameLogic.CheckRowsFull().Count;
                    int colsCleared = gameLogic.CheckColumnsFull().Count;

                    if (rowsCleared > 0 || colsCleared > 0)
                    {
                        if (rowsCleared > 0 || colsCleared > 0)
                        {
                            UpdateScore((rowsCleared + colsCleared) * 20);
                            StartWaveClear();
                        }


                        int totalClear = rowsCleared + colsCleared;
                        UpdateScore(totalClear * 20);
                    }

                    draggingBlock = null;
                    draggingBlockSourceIndex = -1; 
                    highlightCells.Clear();
                    board.Invalidate();
                    RefreshPreviewSlots();
                    if (isWaveClearing)
                        return;
                    if (!isWaveClearing && gameOver())
                    {
                        waveTimer.Stop();
                        waveTimer.Dispose();

                        MessageBox.Show($"Điểm số cuối cùng: {currScore}", "GAME OVER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (currScore > highScore)
                            dataHelper.UpdateHighscore(currScore);
                        
                        AudioManager.Play("gameover");

                        this.Close();
                    }
                }
                else
                {
                    // không đặt được => âm thanh hoặc feedback
                    System.Media.SystemSounds.Beep.Play();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (draggingBlock != null && draggingBlockSourceIndex != -1)
                {
                   
                    previewBlocks[draggingBlockSourceIndex] = draggingBlock.data;
                    blockPanel.Controls[draggingBlockSourceIndex].Invalidate();
                }
                draggingBlock = null;
                draggingBlockSourceIndex = -1;
                highlightCells.Clear();
                board.Invalidate();
            }
        }

        private void RefreshPreviewSlots()
        {
            bool allEmpty = true;
            for (int i = 0; i < 3; i++)
            {
                if (previewBlocks[i] != null)
                {
                    allEmpty = false;
                    break;
                }
            }
            if (!allEmpty)
                return;
            AudioManager.Play("create"); 
            for (int i = 0; i < 3; i++)
            {

                previewBlocks[i] = storage.GetRandomBlock(); 
                blockPanel.Controls[i].Invalidate();
            }
        }
        private void HandleAllClear()
        {
            UpdateScore(100);
            AudioManager.Play("AllClear");
            FlashBoard();
        }
        private int flashCount = 0;
        private Timer flashTimer;

        private void FlashBoard()
        {
            flashCount = 0;
            flashTimer = new Timer();
            flashTimer.Interval = 60;

            flashTimer.Tick += (s, e) =>
            {
                board.BackColor = (flashCount % 2 == 0)
                    ? Color.White
                    : Color.FromArgb(34, 49, 81);

                board.Invalidate();
                flashCount++;

                if (flashCount >= 6)
                {
                    flashTimer.Stop();
                    flashTimer.Dispose();
                    board.BackColor = Color.FromArgb(34, 49, 81);
                }
            };

            flashTimer.Start();
        }


        private bool IsAllClear()
        {
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                    if (gameLogic.Grid[y, x])
                        return false;

            return true;
        }


        public void UpdateScore(int points)
        {
            currScore += points;
            scorePanel.Text = $"Score: {currScore}";
            
        }

        private PictureBox CreatePreviewBox()
        {
            var preview = new PictureBox
            {
                Size = new Size(90, 90),
                Margin = new Padding(10),
                BackColor = Color.FromArgb(78, 109, 179),
                BorderStyle = BorderStyle.FixedSingle
            };
            preview.Paint += Preview_Paint;
            preview.MouseDown += PreviewMouseDown;
            return preview;
        }

        private void Preview_Paint(object sender, PaintEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            int idx = blockPanel.Controls.IndexOf(box);

            if (idx < 0 || idx > 2) return;

            BlockData data = previewBlocks[idx];
            if (data == null) 
            {
                e.Graphics.Clear(box.BackColor);
                return;
            }

            var g = e.Graphics;
            g.Clear(box.BackColor);


            const int PREVIEW_GRID = 3; 

            float padding = box.Width * 0.15f;
            float drawSize = box.Width - padding * 2;
            float cell = drawSize / PREVIEW_GRID;
            float offsetX = (box.Width - data.width * cell) / 2f;
            float offsetY = (box.Height - data.height * cell) / 2f;



            using (Brush b = new SolidBrush(PreviewBlockColor))
            using (Pen p = new Pen(BlockBorderColor, 2))
            {
                for (int i = 0; i < data.height; i++)
                {
                    for (int j = 0; j < data.width; j++)
                    {
                        if (data.Grid[i, j])
                        {
                            float x = j * cell + offsetX;
                            float y = i * cell + offsetY;

                            g.FillRectangle(b, x, y, cell, cell);
                            g.DrawRectangle(p, x, y, cell, cell);
                        }
                    }
                }
            }
        }

        private void PictureBoxBoard_Paint(object sender, PaintEventArgs e)
        {
            if (backBuffer == null) return;

            DrawBoardToBuffer();
            e.Graphics.DrawImageUnscaled(backBuffer, 0, 0);
        }


        // ===HIGHLIGHT===

        public void HighlightBlock(Block blockpreview)
        {
            highlightCells.Clear();

            if (blockpreview == null || blockpreview.data == null)
            {
                highlightValid = false;
                return;
            }

            highlightValid = gameLogic.Can_Place(blockpreview);

            for (int i = 0; i < blockpreview.data.height; i++)
            {
                for (int j = 0; j < blockpreview.data.width; j++)
                {
                    if (blockpreview.data.Grid[i, j])
                    {
                        int boardX = blockpreview.X + j;
                        int boardY = blockpreview.Y + i;

                        if (boardX >= 0 && boardX < cols &&
                            boardY >= 0 && boardY < rows)
                        {
                            highlightCells.Add(new Point(boardX, boardY));
                        }
                    }
                }
            }

        }
        private void DrawBoardToBuffer()
        {
            bufferG.Clear(board.BackColor);

            float cw = cellSize;
            float ch = cellSize;



            using (Brush b = new SolidBrush(BoardBlockColor))
            {
                for (int y = 0; y < rows; y++)
                    for (int x = 0; x < cols; x++)
                        if (gameLogic.Grid[y, x])
                            bufferG.FillRectangle(b, x * cw, y * ch, cw, ch);
            }
            using (Pen p = new Pen(BlockBorderColor, 2))
            {
                for (int y = 0; y < rows; y++)
                    for (int x = 0; x < cols; x++)
                        if (gameLogic.Grid[y, x])
                            bufferG.DrawRectangle(
                                p,
                                x * cw,
                                y * ch,
                                cw,
                                ch
                            );
            }
            using (Brush b = new SolidBrush(
                highlightValid ?
                Color.FromArgb(120, 0, 255, 0) :
                Color.FromArgb(120, 255, 0, 0)))
            {
                foreach (var cell in highlightCells)
                    bufferG.FillRectangle(b, cell.X * cw, cell.Y * ch, cw, ch);
            }
            if (isWaveClearing)
            {
               
                using (Brush b = new SolidBrush(Color.FromArgb(180, Color.White)))
                {
                    for (int i = 0; i <= waveIndex && i < waveLayers.Count; i++)
                    {
                        foreach (var p in waveLayers[i])
                        {
                            if (gameLogic.Grid[p.Y, p.X])
                            {
                                bufferG.FillRectangle(
                                    b,
                                    p.X * cellSize,
                                    p.Y * cellSize,
                                    cellSize,
                                    cellSize
                                );
                            }

                        }
                    }
                }
            }
            using (var pen = new Pen(Color.LightGray, 1))
            {
                for (int x = 0; x <= cols; x++)
                    bufferG.DrawLine(pen, x * cw, 0, x * cw, board.Height);

                for (int y = 0; y <= rows; y++)
                    bufferG.DrawLine(pen, 0, y * ch, board.Width, y * ch);
            }
        }

        private void board_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingBlock == null) return;

            int col = (int)(e.X / cellSize);
            int row = (int)(e.Y / cellSize);
            if (draggingBlock.X == col && draggingBlock.Y == row)
                return;

            draggingBlock.X = col;
            draggingBlock.Y = row;

            HighlightBlock(draggingBlock);
            board.Invalidate();
        }

        private BlockData CloneBlockData(BlockData src)
        {
            if (src == null) return null;

            BlockData copy = new BlockData(src.Type_name);
            copy.width = src.width;
            copy.height = src.height;
            copy.Rotation_Index = src.Rotation_Index;

            copy.Grid = new bool[src.height, src.width];
            for (int i = 0; i < src.height; i++)
                for (int j = 0; j < src.width; j++)
                    copy.Grid[i, j] = src.Grid[i, j];

            return copy;
        }

        public bool Can_Place_Anywhere(BlockData data)
        {
            Block tmp = new Block();
            tmp.data = data;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    tmp.X = x;
                    tmp.Y = y;

                    if (gameLogic.Can_Place(tmp))
                        return true;
                }
            }
            return false;
        }

        private bool gameOver()
        {

            foreach (var pb in previewBlocks)
            {
                if (pb == null) continue;

                if (Can_Place_Anywhere(pb))
                    return false;
            }

            return true;
        }

        private void PreviewMouseDown(object sender, MouseEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            int idx = blockPanel.Controls.IndexOf(box);
            if (idx < 0 || idx > 2) return;
            if (previewBlocks[idx] == null || draggingBlock != null) return;

            if (e.Button == MouseButtons.Left)
            {
                draggingBlockSourceIndex = idx;
                var dataCopy = CloneBlockData(previewBlocks[idx]);

                draggingBlock = new Block();
                draggingBlock.data = dataCopy;
                draggingBlock.X = -1;
                draggingBlock.Y = -1;

                previewBlocks[idx] = null;
                box.Invalidate();

                board_MouseMove(board, new MouseEventArgs(MouseButtons.None, 0, Cursor.Position.X, Cursor.Position.Y, 0));
            }

            else if (e.Button == MouseButtons.Right)
            {
                return;
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            waveTimer?.Stop();
            waveTimer?.Dispose();
            bufferG?.Dispose();
            backBuffer?.Dispose();
            base.OnFormClosed(e);
        }

    }
}