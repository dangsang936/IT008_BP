using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LOGIC;

namespace UI
{
    public partial class Form1 : Form
    {
        private DoubleBufferedPanel board;
        private DoubleBufferedPanel nextPreview;
        private Label lblScore;
        private MainBoard mainBoard;

        private Block currentBlock;
        private Block nextBlock;
        static Random rand = new Random();

        private int currentBlockIndex = 0;
        private int nextBlockIndex = 0;
        private int score = 0;

        private readonly string[] blockTypes = new[]
        {
            "3 Verticle Line Block","4 Verticle Line Block","3 Horizontal Line Block","4 Horizontal Line Block",
            "L Type","J Type","Long L Type","Long J Type","F Type","7 Type","Long F Type","Long 7 Type",
            "Z Type","S Type","O Type","Big O Type","4 Type","Reverse 4 Type",
            "T Type","Reverse T Type","7 Up Type","F Up Type","F Down Type","7 Down Type"
        };

        private int hoverRow = -1;
        private int hoverCol = -1;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(62, 87, 153);
            this.ClientSize = new Size(900, 600);
            this.DoubleBuffered = true;

            mainBoard = new MainBoard();

            board = new DoubleBufferedPanel { BackColor = this.BackColor };
            nextPreview = new DoubleBufferedPanel { BackColor = this.BackColor };
            lblScore = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = "Score: 0"
            };

            board.Paint += Board_Paint;
            board.MouseClick += Board_MouseClick;
            board.MouseMove += Board_MouseMove;

            nextPreview.Paint += NextPreview_Paint;

            this.Controls.Add(board);
            this.Controls.Add(nextPreview);
            this.Controls.Add(lblScore);

            if (!HasAnyPlacementForAnyBlock())
            {
                MessageBox.Show(this, "No initial placement available. Board will reset.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetBoard();
            }

            TryInitCurrentAndNext();

            this.Load += (s, e) => PositionBoard();
            this.Resize += (s, e) => PositionBoard();
        }

        private bool TryInitCurrentAndNext()
        {
            var cur = FindAnyPlaceableBlock();
            if (cur == null) return false;
            currentBlockIndex = cur.Item1;
            currentBlock = cur.Item2;

            var nxt = FindAnyPlaceableBlock();
            if (nxt == null)
            {
                nextBlock = null;
                nextBlockIndex = -1;
                board?.Invalidate();
                nextPreview?.Invalidate();
                return true;
            }

            nextBlockIndex = nxt.Item1;
            nextBlock = nxt.Item2;

            board?.Invalidate();
            nextPreview?.Invalidate();
            return true;
        }

        private Tuple<int, Block> FindAnyPlaceableBlock()
        {
            int len = blockTypes.Length;
            int start = rand.Next(len);
            for (int i = 0; i < len; i++)
            {
                int idx = (start + i) % len;
                var b = new Block();
                b.Create_Block(blockTypes[idx]);
                if (HasAnyPlacementForBlock(b))
                    return Tuple.Create(idx, b);
            }
            return null;
        }

        private void SpawnNextBlockAsCurrent()
        {
            if (nextBlock == null)
            {
                var any = FindAnyPlaceableBlock();
                if (any == null)
                {
                    var res = MessageBox.Show(this, "No available placements remaining. Restart board?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        ResetBoard();
                        return;
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
                currentBlockIndex = any.Item1;
                currentBlock = any.Item2;
            }
            else
            {
                currentBlockIndex = nextBlockIndex;
                currentBlock = nextBlock;
            }

            var nxt = FindAnyPlaceableBlock();
            if (nxt == null)
            {
                var res = MessageBox.Show(this, "Thua cmmr", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (res == DialogResult.Yes)
                {
                    ResetBoard();
                    return;
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            nextBlockIndex = nxt.Item1;
            nextBlock = nxt.Item2;
        }

        private bool HasAnyPlacementForBlock(Block block)
        {
            if (block == null || mainBoard == null || mainBoard.Board == null) return false;
            int rows = mainBoard.Board.GetLength(0);
            int cols = mainBoard.Board.GetLength(1);

            if (block.height <= 0 || block.width <= 0) return false;
            if (block.height > rows || block.width > cols) return false;
            bool anyOccupied = false;
            for (int r = 0; r < rows && !anyOccupied; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (mainBoard.Board[r, c])
                    {
                        anyOccupied = true;
                        break;
                    }
                }
            }
            if (!anyOccupied)
            {
                return true;
            }

            for (int r = 0; r <= rows - block.height; r++)
            {
                for (int c = 0; c <= cols - block.width; c++)
                {
                    if (block.Can_Place(mainBoard, r, c))
                        return true;
                }
            }
            return false;
        }

        private bool HasAnyPlacementForAnyBlock()
        {
            if (mainBoard == null || mainBoard.Board == null) return false;
            foreach (var type in blockTypes)
            {
                var b = new Block();
                b.Create_Block(type);
                if (HasAnyPlacementForBlock(b))
                    return true;
            }
            return false;
        }

        private void ResetBoard()
        {
            mainBoard = new MainBoard();
            score = 0;
            UpdateScoreLabel();
            TryInitCurrentAndNext();
            board?.Invalidate();
            nextPreview?.Invalidate();
        }

        private void UpdateScoreLabel()
        {
            lblScore.Text = $"Score: {score}";
        }

        private void PositionBoard()
        {
            const int marginHorizontal = 40;
            const int marginVertical = 40;
            int availableWidth = Math.Max(0, this.ClientSize.Width - marginHorizontal);
            int availableHeight = Math.Max(0, this.ClientSize.Height - marginVertical);

            int size = Math.Min(availableWidth - 200, availableHeight);
            size = Math.Max(240, Math.Min(size, 720));

            board.Size = new Size(size, size);
            var boardX = (this.ClientSize.Width - board.Width) / 2 - 80;
            var boardY = (this.ClientSize.Height - board.Height) / 2;
            board.Location = new Point(boardX, boardY);

            nextPreview.Size = new Size(140, 140);
            nextPreview.Location = new Point(board.Right + 20, board.Top);

            lblScore.Location = new Point(nextPreview.Left, nextPreview.Bottom + 10);

            board.BringToFront();
            board.Invalidate();
            nextPreview.Invalidate();
        }

        private void Board_Paint(object sender, PaintEventArgs e)
        {
            if (mainBoard == null || mainBoard.Board == null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int rows = mainBoard.Board.GetLength(0);
            int cols = mainBoard.Board.GetLength(1);
            float cellW = (float)board.ClientSize.Width / cols;
            float cellH = (float)board.ClientSize.Height / rows;

            using (var occupiedBrush = new SolidBrush(Color.FromArgb(200, 255, 200, 0)))
            using (var gridPen = new Pen(Color.FromArgb(120, 0, 0, 0)))
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (mainBoard.Board[r, c])
                        {
                            g.FillRectangle(occupiedBrush, c * cellW, r * cellH, cellW, cellH);
                        }
                    }
                }

                if (currentBlock != null && hoverRow >= 0 && hoverCol >= 0)
                {
                    bool fits = (hoverRow + currentBlock.height <= rows) && (hoverCol + currentBlock.width <= cols);
                    bool canPlace = false;
                    if (fits) canPlace = currentBlock.Can_Place(mainBoard, hoverRow, hoverCol);

                    Color previewColor = canPlace ? Color.FromArgb(180, 50, 200, 50) : Color.FromArgb(140, 200, 50, 50);
                    using (var previewBrush = new SolidBrush(previewColor))
                    using (var previewPen = new Pen(Color.FromArgb(200, 0, 0, 0)))
                    {
                        for (int r = 0; r < currentBlock.height; r++)
                        {
                            for (int c = 0; c < currentBlock.width; c++)
                            {
                                if (!currentBlock.grid[r, c]) continue;
                                float x = (hoverCol + c) * cellW;
                                float y = (hoverRow + r) * cellH;
                                var rect = new RectangleF(x + 1, y + 1, cellW - 2, cellH - 2);
                                try
                                {
                                    string keyBase = blockTypes[currentBlockIndex].Replace(" ", "_").Replace("-", "_");
                                    var obj = Properties.Resources.ResourceManager.GetObject(keyBase);
                                    if (obj is System.Drawing.Image img)
                                        g.DrawImage(img, rect);
                                    else
                                        g.FillRectangle(previewBrush, rect);
                                }
                                catch
                                {
                                    g.FillRectangle(previewBrush, rect);
                                }

                                g.DrawRectangle(previewPen, Rectangle.Round(rect));
                            }
                        }
                    }
                }

                for (int r = 0; r <= rows; r++)
                    g.DrawLine(gridPen, 0, r * cellH, board.ClientSize.Width, r * cellH);
                for (int c = 0; c <= cols; c++)
                    g.DrawLine(gridPen, c * cellW, 0, c * cellW, board.ClientSize.Height);
            }
        }

        private void NextPreview_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(nextPreview.BackColor);
            if (nextBlock == null) return;
            int cellSize = 24;
            int w = nextBlock.width;
            int h = nextBlock.height;
            int totalW = w * cellSize;
            int totalH = h * cellSize;
            int startX = (nextPreview.ClientSize.Width - totalW) / 2;
            int startY = (nextPreview.ClientSize.Height - totalH) / 2;

            using (var brush = new SolidBrush(Color.FromArgb(200, 255, 200, 0)))
            using (var pen = new Pen(Color.FromArgb(120, 0, 0, 0)))
            {
                for (int r = 0; r < h; r++)
                {
                    for (int c = 0; c < w; c++)
                    {
                        if (!nextBlock.grid[r, c]) continue;
                        var rect = new Rectangle(startX + c * cellSize, startY + r * cellSize, cellSize - 2, cellSize - 2);
                        try
                        {
                            string keyBase = blockTypes[nextBlockIndex].Replace(" ", "_").Replace("-", "_");
                            var obj = Properties.Resources.ResourceManager.GetObject(keyBase);
                            if (obj is System.Drawing.Image img)
                                g.DrawImage(img, rect);
                            else
                                g.FillRectangle(brush, rect);
                        }
                        catch
                        {
                            g.FillRectangle(brush, rect);
                        }
                        g.DrawRectangle(pen, rect);
                    }
                }
            }
        }

        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            if (mainBoard == null || mainBoard.Board == null) return;
            int rows = mainBoard.Board.GetLength(0);
            int cols = mainBoard.Board.GetLength(1);

            int col = (int)(e.X * cols / (float)board.ClientSize.Width);
            int row = (int)(e.Y * rows / (float)board.ClientSize.Height);

            if (row != hoverRow || col != hoverCol)
            {
                hoverRow = row;
                hoverCol = col;
                board.Invalidate();
            }
        }

        private void Board_MouseClick(object sender, MouseEventArgs e)
        {
            if (mainBoard == null || mainBoard.Board == null) return;

            int rows = mainBoard.Board.GetLength(0);
            int cols = mainBoard.Board.GetLength(1);

            int col = (int)(e.X * cols / (float)board.ClientSize.Width);
            int row = (int)(e.Y * rows / (float)board.ClientSize.Height);

            if (row < 0 || row >= rows || col < 0 || col >= cols) return;

            if (e.Button == MouseButtons.Right)
            {
                var any = FindAnyPlaceableBlock();
                if (any != null)
                {
                    nextBlockIndex = any.Item1;
                    nextBlock = any.Item2;
                    nextPreview.Invalidate();
                }
                return;
            }
            if (currentBlock == null) return;

            bool fits = (row + currentBlock.height <= rows) && (col + currentBlock.width <= cols);
            if (!fits)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            if (currentBlock.Can_Place(mainBoard, row, col))
            {
                if (mainBoard.Place(currentBlock, row, col))
                {
                    score++;
                    UpdateScoreLabel();

                    if (!HasAnyPlacementForAnyBlock())
                    {
                        var res = MessageBox.Show(this, "Thua cmmr", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (res == DialogResult.Yes)
                        {
                            ResetBoard();
                            return;
                        }
                        else
                        {
                            this.Close();
                            return;
                        }
                    }

                    SpawnNextBlockAsCurrent();
                    board.Invalidate();
                    nextPreview.Invalidate();
                }
                else
                    System.Media.SystemSounds.Beep.Play();
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(this, "EXIT", "sang an cut", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                this.Close();
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}
