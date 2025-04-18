﻿using JapanezePuzzle.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls
{
    /// <summary>
    /// Control for solving the puzzle.
    /// </summary>
    public partial class PuzzleSolvingControl : TemplateControl
    {
        // Puzzle
        private Classes.Puzzle _puzzle;

        // Arrow back
        private PictureBox _arrowBackIcon;

        // Puzzle panel
        private Controls.Panels.PuzzleSolvingPanel _puzzlePanel;

        // Variables storage
        private int _difficulty;
        private int _currentIndex;

        /// <summary>
        /// Constructor for the PuzzleSolvingControl class.
        /// </summary>
        /// <param name="puzzle"></param>
        /// <param name="difficulty"></param>
        /// <param name="currentIndex"></param>
        public PuzzleSolvingControl(Classes.Puzzle puzzle, int difficulty, int currentIndex)
        {
            InitializeComponent();

            // Variables storage
            _difficulty = difficulty;
            _currentIndex = currentIndex;

            // Background
            this.BackgroundImage = Properties.Resources.lakeBackground;

            // Puzzle panel
            _puzzle = puzzle;
            _puzzlePanel = new Controls.Panels.PuzzleSolvingPanel(puzzle);
            this.Controls.Add(_puzzlePanel);

            // Back arrow
            _arrowBackIcon = new PictureBox();
            _arrowBackIcon.Image = Properties.Resources.backArrowImage;
            _arrowBackIcon.SizeMode = PictureBoxSizeMode.Zoom;
            _arrowBackIcon.Size = new Size(40, 40);
            _arrowBackIcon.BackColor = Color.Transparent;
            _arrowBackIcon.Left = 20;
            _arrowBackIcon.Top = 20;
            _arrowBackIcon.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            _arrowBackIcon.Click += (s, e) =>
            {
                if (this.ParentForm is MainForm mainForm)
                {
                    PuzzleStorage.SavePuzzle(_puzzle);
                    var puzzleList = new PuzzleListControl(_difficulty, _currentIndex);
                    mainForm.SwitchControl(puzzleList);
                }
            };
            this.Controls.Add(_arrowBackIcon);

            // Position everything
            this.Resize += (s, e) => ArrangeLayout();

            // Draw the puzzle
            _puzzlePanel.DrawPuzzle();
        }

        /// <summary>
        /// Arranges the layout of the control.
        /// </summary>
        private void ArrangeLayout()
        {
            // The center the puzzle panel
            int puzzlePanelX = _puzzlePanel.GetPanelCenterX(this.ClientSize.Width);
            int panelCenterY = _puzzlePanel.GetPanelCenterY(this.ClientSize.Height);

            _puzzlePanel.Left = puzzlePanelX;
            _puzzlePanel.Top = panelCenterY;
        }

        /// <summary>
        /// Handles the event when the user wins the game.
        /// </summary>
        public void UserWinEvent()
        {
            if (this.ParentForm is MainForm mainForm)
            {
                var showWin = new ShowWinControl(_difficulty, 0, _puzzle);
                mainForm.SwitchControl(showWin);
            }
        }
    }
}
