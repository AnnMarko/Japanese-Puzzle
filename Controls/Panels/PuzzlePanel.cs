using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace JapanezePuzzle.Controls.Panels
{
    public class PuzzlePanel : Panel
    {
        private Classes.Puzzle _puzzle;
        private PictureBox[,] _cells;

        private int _size;

        public int SideSize
        {
            get { return _size; }
            set { _size = value; }
        }
        public PictureBox[,] Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }

        public PuzzlePanel(Classes.Puzzle puzzle = null, int size = 400)
        {
            // Double Buffer
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                          | ControlStyles.UserPaint
                          | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            // Pazzle
            if (puzzle != null)
            {
                _puzzle = puzzle;
                Cells = new PictureBox[_puzzle.Rows, _puzzle.Cols];
            }
            SideSize = size;
            this.Size = new Size(SideSize, SideSize);
        }

        public void SetPuzzle(Classes.Puzzle puzzle)
        {
            _puzzle = puzzle;
            Cells = new PictureBox[_puzzle.Rows, _puzzle.Cols];
        }

        public Classes.Puzzle GetPuzzle()
        {
            return _puzzle;
        }

        virtual public void DrawPuzzle()
        {
            Visible = false;
            if (_puzzle == null) return;

            this.Size = new Size(SideSize, SideSize);

            int rows = _puzzle.Rows;
            int cols = _puzzle.Cols;

            int panelSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
            int cellWidth = panelSize / cols;
            int cellHeight = panelSize / rows;
            int cellSize = Math.Min(cellWidth, cellHeight);

            this.Size = new Size(cellSize * cols, cellSize * rows);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _cells[i, j] = new PictureBox()
                    {
                        BackColor = _puzzle.PuzzleCellMatrix[i, j] == 1 ? Color.Black : Color.White,
                        Width = cellSize,
                        Height = cellSize,
                        Left = j * cellSize,
                        Top = i * cellSize,
                        BorderStyle = BorderStyle.FixedSingle,
                    };
                    this.Controls.Add(_cells[i, j]);
                }
            }
            Visible = true;
        }

        public int GetPanelCenterX(int formWidth)
        {
            return (int)(formWidth - this.Width) / 2;
        }

        public int GetPanelCenterY(int formHeight)
        {
            return (int)(formHeight - this.Height) / 2;
        }
    }
}
