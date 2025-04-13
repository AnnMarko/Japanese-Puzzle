﻿using JapanezePuzzle.Classes;
using JapanezePuzzle.Controls.Labels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls.Panels
{
    internal class PuzzleSandboxPanel : PuzzlePanel
    {
        private Classes.Puzzle _puzzle;

        private int MAX_SIZE = 700;
        private int MAX_CELL_SIZE = 60;
        private int _rows;
        private int _cols;
        private int _numberedRows;
        private int _numberedCols;
        private int _sizeOfCell;

        // Labels
        private CellLabel[,] _labelsRows;
        private CellLabel[,] _labelsCols;

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

        public PuzzleSandboxPanel()
        {
            // Create puzzle object
            _puzzle = new Classes.Puzzle(PuzzleStorage.GetMaxId(), 5, 5);

            NumberedRows = 3;
            NumberedCols = 3;
            Rows = _puzzle.Rows + NumberedRows;
            Cols = _puzzle.Cols + NumberedCols;

            InitializePuzzleNumbers();

            int maxCells = Rows;

            CellSize = Math.Min(MAX_SIZE / maxCells, MAX_CELL_SIZE);
            Size = new Size(CellSize * Cols, CellSize * Rows);
            SideSize = CellSize * Math.Max(Cols, Rows);

            Cells = new PictureBox[Rows, Cols];

            // Labels
            _labelsRows = new CellLabel[NumberedRows, _puzzle.Rows];
            _labelsCols = new CellLabel[NumberedCols, _puzzle.Cols];

            // Panel settings
            BackColor = Color.Black;
        }

        private void InitializePuzzleNumbers()
        {
            _puzzle.PuzzleNumbers = new int[2][][];
            _puzzle.PuzzleNumbers[0] = new int[_puzzle.Cols][];
            _puzzle.PuzzleNumbers[1] = new int[_puzzle.Rows][];
            for (int i = 0; i < _puzzle.Cols; i++)
            {
                _puzzle.PuzzleNumbers[0][i] = new int[] { 0 };
            }
            for (int i = 0; i < _puzzle.Rows; i++)
            {
                _puzzle.PuzzleNumbers[1][i] = new int[] { 0 };
            }
        }

        override public void DrawPuzzle()
        {
            Visible = false;
            if (_puzzle == null) return;

            int sizeX = 0;
            int sizeY = 0;
            // Draw cells firstly
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (i < NumberedRows ^ j < NumberedCols) // xor
                    {
                        var label = new Labels.CellLabel(CellSize)
                        {
                            Text = "",
                            Left = j * CellSize + ((int)(Math.Max(j - NumberedCols, 0) / 5)) * 2 + (j - NumberedCols >= 0 ? 5 : 0),
                            Top = i * CellSize + ((int)(Math.Max(i - NumberedRows, 0) / 5)) * 2 + (i - NumberedRows >= 0 ? 5 : 0),
                        };
                        this.Controls.Add(label);
                        label.BringToFront();
                        if (i < NumberedRows)
                        {
                            _labelsRows[i, j - 3] = label;
                        }
                        else if (j < NumberedCols)
                        {
                            _labelsCols[j, i - 3] = label;
                        }
                    }
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
                        CalculatePuzzleNumbers();
                        UpdatePuzzleNumbers();
                    };
                }
            }
            Visible = true;
        }

        private void UpdatePuzzleNumbers()
        {
            // Firstly clear all labels
            // Rows
            int maxNumbersRowsCount = (_puzzle.Rows + 1) / 2;
            for (int j = 0; j < _labelsRows.GetLength(0); j++)
            {
                for (int k = 0; k < _labelsRows.GetLength(1); k++)
                {
                    _labelsRows[j, k].Text = "";
                }
            }
            // Cols
            int maxNumbersColsCount = (_puzzle.Cols + 1) / 2;
            for (int j = 0; j < _labelsCols.GetLength(0); j++)
            {
                for (int k = 0; k < _labelsCols.GetLength(1); k++)
                {
                    _labelsCols[j, k].Text = "";
                }
            }
            // Then enter numbers
            // Rows
            for (int j = 0; j < _puzzle.PuzzleNumbers[1].Length; j++)
            {
                for (int k = 0; k < _puzzle.PuzzleNumbers[1][j].Length; k++)
                {
                    int offset = maxNumbersRowsCount - _puzzle.PuzzleNumbers[1][j].Length;

                    if (_puzzle.PuzzleNumbers[1][j][k] == 0)
                    {
                        _labelsRows[k + offset, j].Text = "";
                        continue;
                    }

                    _labelsRows[k + offset, j].Text = _puzzle.PuzzleNumbers[1][j][k].ToString();
                }
            }
            // Cols
            for (int j = 0; j < _puzzle.PuzzleNumbers[0].Length; j++)
            {
                for (int k = 0; k < _puzzle.PuzzleNumbers[0][j].Length; k++)
                {
                    int offset = maxNumbersColsCount - _puzzle.PuzzleNumbers[0][j].Length;

                    if (_puzzle.PuzzleNumbers[0][j][k] == 0)
                    {
                        _labelsCols[k + offset, j].Text = "";
                        continue;
                    }

                    _labelsCols[k + offset, j].Text = _puzzle.PuzzleNumbers[0][j][k].ToString();
                }
            }
        }

        private int[][][] CalculatePuzzleNumbers()
        {
            int length = _puzzle.Cols;
            int width = _puzzle.Rows;

            int[][][] puzzleNumbers = _puzzle.PuzzleNumbers;

            for (int i = 0; i < puzzleNumbers.Length; i++)
            {
                for (int j = 0; j < (i > 0 ? length : width); j++)
                {
                    int value = 0;
                    int maxPossibleNumbersCount = (length + 1) / 2;
                    int[] numbers = new int[maxPossibleNumbersCount];
                    int numberIndex = 0;

                    for (int k = 0; k < (i > 0 ? width : length); k++)
                    {
                        switch (i > 0 ? _puzzle.PuzzleCellMatrix[k, j] : _puzzle.PuzzleCellMatrix[j, k])
                        {
                            case 0:
                                if (value != 0)
                                {
                                    numbers[numberIndex++] = value;
                                    value = 0;
                                }
                                break;
                            case 1:
                                value++;
                                break;
                        }
                    }
                    if (value != 0)
                    {
                        numbers[numberIndex++] = value;
                        value = 0;
                    }
                    int numbersCount = 0;
                    for (int k = 0; k < numbers.Length; k++)
                    {
                        if (numbers[k] == 0)
                        {
                            break;
                        }
                        numbersCount++;
                    }
                    int[] numbersToSave;
                    if (numbersCount == 0)
                    {
                        numbersToSave = new int[] { 0 };
                    }
                    else
                    {
                        numbersToSave = new int[numbersCount];
                        for (int k = 0; k < numbersCount; k++)
                        {
                            numbersToSave[k] = numbers[k];
                        }
                    }

                    puzzleNumbers[i][j] = numbersToSave;
                }
            }
            return puzzleNumbers;
        }
    }
}
