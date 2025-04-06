using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Classes
{
    public class PuzzlePanel : Panel
    {
        private Classes.Puzzle _puzzle;
        private PictureBox[,] _cells;

        private int _size;

        public PuzzlePanel(int size = 400)
        {
            _size = size;
            this.DoubleBuffered = true;
            this.Size = new Size(_size, _size);
        }

        public void SetPuzzle(Puzzle puzzle)
        {
            _puzzle = puzzle;
            _cells = new PictureBox[_puzzle.Rows, _puzzle.Cols];
            DrawPuzzle();
        }

        public void DrawPuzzle()
        {
            if (_puzzle == null) return;

            this.Size = new Size(_size, _size);

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
                        BackColor = Color.White,
                        Width = cellSize,
                        Height = cellSize,
                        Left = j * cellSize,
                        Top = i * cellSize,
                        BorderStyle = BorderStyle.FixedSingle,
                    };
                    this.Controls.Add(_cells[i, j]);
                }
            }
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
