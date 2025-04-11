using JapanezePuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls.Panels
{
    public class PuzzleSolvingPanel : PuzzlePanel
    {
        private Classes.Puzzle _puzzle;
        private PictureBox[,] _cells;

        private int MAX_SIZE = 700;
        private int MAX_CELL_SIZE = 60;
        private int _rows;
        private int _cols;
        private int _numberedRows;
        private int _numberedCols;
        private int _sizeOfCell;

        public int Rows
        {
            get => _rows;
            set => _rows = value;
        }

        public int Cols
        {
            get => _cols;
            set => _cols = value;
        }
        public int NumberedRows
        {
            get => _numberedRows;
            set => _numberedRows = value;
        }
        public int NumberedCols
        {
            get => _numberedCols;
            set => _numberedCols = value;
        }
        public int CellSize
        {
            get => _sizeOfCell;
            set => _sizeOfCell = value;
        }

        public PuzzleSolvingPanel(Classes.Puzzle puzzle)
        {
            _puzzle = puzzle;
            Rows = CalculateRows(puzzle);
            Cols = CalculateCols(puzzle);
            NumberedRows = Rows - puzzle.Rows;
            NumberedCols = Cols - puzzle.Cols;

            // If user solves the puzzle again
            if (_puzzle.IsSolved)
            {
                _puzzle.IsSolved = false;
                for (int i = 0; i < _puzzle.Rows; i++)
                {
                    for (int j = 0; j < _puzzle.Cols; j++)
                    {
                        _puzzle.PuzzleCellMatrix[i, j] = 0;
                    }
                }
            }

            int maxCells = Math.Max(Rows, Cols);

            CellSize = Math.Min(MAX_SIZE / maxCells, MAX_CELL_SIZE);
            Size = new Size(CellSize * Cols, CellSize * Rows);
            SideSize = CellSize * Math.Max(Cols, Rows);

            Cells = new PictureBox[Rows, Cols];

            // Panel settings
            BackColor = Color.Black;
        }

        private int CalculateRows(Classes.Puzzle puzzle)
        {
            int result = puzzle.Rows;
            int rows = 1;

            int[][] puzzleNumbers = puzzle.PuzzleNumbers[1];

            for (int i = 0; i < puzzleNumbers.Length; i++)
            {
                if (puzzleNumbers[i].Length > rows)
                {
                    rows = puzzleNumbers[i].Length;
                }
            }
            result += rows;
            return result;
        }

        private int CalculateCols(Classes.Puzzle puzzle)
        {
            int result = puzzle.Cols;
            int cols = 1;

            int[][] puzzleNumbers = puzzle.PuzzleNumbers[0];

            for (int i = 0; i < puzzleNumbers.Length; i++)
            {
                if (puzzleNumbers[i].Length > cols)
                {
                    cols = puzzleNumbers[i].Length;
                }
            }
            result += cols;
            return result;
        }

        override public void DrawPuzzle()
        {
            if (_puzzle == null) return;

            int sizeX = 0;
            int sizeY = 0;
            // Draw cells firstly
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Cells[i, j] = new PictureBox()
                    {
                        BackColor = Color.White,
                        ForeColor = Color.Black,
                        Width = CellSize,
                        Height = CellSize,
                        Left = j * CellSize + ((int)(Math.Max(j - NumberedCols, 0) / 5)) * 2 + (j - NumberedCols >= 0 ? 5 : 0),
                        Top = i * CellSize + ((int)(Math.Max(i - NumberedRows, 0) / 5)) * 2 + (i - NumberedRows >= 0 ? 5 : 0),
                        BorderStyle = (i < NumberedRows && j < NumberedCols) ? BorderStyle.None : BorderStyle.FixedSingle,
                    };
                    this.Controls.Add(Cells[i, j]);
                }
            }

            // Size with offsets
            sizeX = sizeX + (Cols - 1) * CellSize + ((int)(Math.Max((Cols - 1) - NumberedCols, 0) / 5)) * 2 + ((Cols - 1) - NumberedCols >= 0 ? 5 : 0) + CellSize;
            sizeY = sizeY + (Rows - 1) * CellSize + ((int)(Math.Max((Rows - 1) - NumberedRows, 0) / 5)) * 2 + ((Rows - 1) - NumberedRows >= 0 ? 5 : 0) + CellSize;
            this.Size = new Size(sizeX, sizeY);

            // Then enter numbers
            // Rows
            int maxNumbersRowsCount = 1;
            for (int j = 0; j < _puzzle.PuzzleNumbers[0].Length; j++)
            {
                if (_puzzle.PuzzleNumbers[0][j].Length > maxNumbersRowsCount)
                {
                    maxNumbersRowsCount = _puzzle.PuzzleNumbers[0][j].Length;
                }
            }
            for (int j = 0; j < _puzzle.PuzzleNumbers[0].Length; j++)
            {
                for (int k = 0; k < _puzzle.PuzzleNumbers[0][j].Length; k++)
                {
                    int offset = maxNumbersRowsCount - _puzzle.PuzzleNumbers[0][j].Length;

                    var label = new Label()
                    {
                        Text = _puzzle.PuzzleNumbers[0][j][k].ToString(),
                        AutoSize = false,
                        Width = CellSize,
                        Height = CellSize,
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.Black,
                        BackColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle,
                        Location = Cells[j + NumberedRows, k + offset].Location
                    };
                    this.Controls.Add(label);
                    label.BringToFront();
                }
            }
            // Cols
            int maxNumbersColsCount = 1;
            for (int j = 0; j < _puzzle.PuzzleNumbers[1].Length; j++)
            {
                if (_puzzle.PuzzleNumbers[1][j].Length > maxNumbersColsCount)
                {
                    maxNumbersColsCount = _puzzle.PuzzleNumbers[1][j].Length;
                }
            }
            for (int j = 0; j < _puzzle.PuzzleNumbers[1].Length; j++)
            {
                for (int k = 0; k < _puzzle.PuzzleNumbers[1][j].Length; k++)
                {
                    int offset = maxNumbersColsCount - _puzzle.PuzzleNumbers[1][j].Length;

                    var label = new Label()
                    {
                        Text = _puzzle.PuzzleNumbers[1][j][k].ToString(),
                        AutoSize = false,
                        Width = CellSize,
                        Height = CellSize,
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.Black,
                        BackColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle,
                        Location = Cells[k + offset, j + NumberedCols].Location
                    };
                    this.Controls.Add(label);
                    label.BringToFront();
                }
            }

            // Add functions to fillable cells and fill them if needed
            for (int i = NumberedRows; i < Rows; i++)
            {
                for (int j = NumberedCols; j < Cols; j++)
                {
                    int localI = i;
                    int localJ = j;
                    int localCellRow = localI - NumberedRows;
                    int localCellCol = localJ - NumberedCols;
                    

                    Cells[localI, localJ].Click += (s, e) =>
                    {
                        bool localIsPainted = _puzzle.PuzzleCellMatrix[localCellRow, localCellCol] == 1;
                        _puzzle.PuzzleCellMatrix[localCellRow, localCellCol] = localIsPainted ? 0 : 1;
                        Cells[localI, localJ].BackColor = localIsPainted ? Color.White : Color.Black;

                        if (CheckIfPuzzleIsSolved())
                        {
                            _puzzle.IsSolved = true;
                            PuzzleStorage.SavePuzzle(_puzzle);
                            SendWin();
                        }
                    };

                    int cellRow = i - NumberedRows;
                    int cellCol = j - NumberedCols;
                    bool isPainted = _puzzle.PuzzleCellMatrix[cellRow, cellCol] == 1;
                    if (isPainted)
                    {
                        Cells[i, j].BackColor = Color.Black;
                    }
                }
            }
        }

        private bool CheckIfPuzzleIsSolved()
        {
            int length = _puzzle.Cols;
            int width = _puzzle.Rows;

            for (int i = 0; i < _puzzle.PuzzleNumbers.Length; i++)
            {
                for (int j = 0; j < (i > 0 ? length : width); j++)
                {
                    int value = 0;
                    int puzzleNumberIndex = 0;

                    for (int k = 0; k < (i > 0 ? width : length); k++)
                    {
                        switch (i > 0 ? _puzzle.PuzzleCellMatrix[k, j] : _puzzle.PuzzleCellMatrix[j, k])
                        {
                            case 0:
                                if (value != 0)
                                {
                                    if (_puzzle.PuzzleNumbers[i][j][puzzleNumberIndex++] != value)
                                    {
                                        return false;
                                    }
                                    value = 0;
                                }
                                break;
                            case 1:
                                value++;
                                break;
                        }
                    }
                    if (puzzleNumberIndex < _puzzle.PuzzleNumbers[i][j].Length)
                    {
                        if (_puzzle.PuzzleNumbers[i][j][puzzleNumberIndex++] != value)
                        {
                            return false;
                        }
                    }
                    if (puzzleNumberIndex < _puzzle.PuzzleNumbers[i][j].Length)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void SendWin()
        {
            if (this.Parent is PuzzleSolvingControl puzzleSolving)
            {
                puzzleSolving.UserWinEvent();
            }
        }
    }
}
