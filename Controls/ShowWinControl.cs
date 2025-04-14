using JapanezePuzzle.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls
{
    /// <summary>
    /// This class represents a control that displays a congratulatory message and the solved puzzle.
    /// </summary>
    public partial class ShowWinControl : TemplateControl
    {
        // Puzzle
        private Controls.Panels.PuzzlePanel _puzzlePanel;

        // Arrow back
        private PictureBox _arrowBackIcon;

        // Congrats label
        private System.Windows.Forms.Label _congratsLabel;

        // Variables storage
        private int _difficulty;
        private int _currentIndex;

        /// <summary>
        /// Constructor for the ShowWinControl class.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="currentIndex"></param>
        /// <param name="puzzle"></param>
        public ShowWinControl(int difficulty, int currentIndex, Classes.Puzzle puzzle)
        {
            InitializeComponent();

            // Background
            this.BackgroundImage = Properties.Resources.lakeBackground;

            // Variables storage
            _difficulty = difficulty;
            _currentIndex = currentIndex;

            // Pannel
            _puzzlePanel = new Panels.PuzzlePanel();
            _puzzlePanel.SetPuzzle(puzzle);
            _puzzlePanel.DrawPuzzle();
            this.Controls.Add(_puzzlePanel);

            // Back arrow
            _arrowBackIcon = new PictureBox();
            _arrowBackIcon.Image = Properties.Resources.backArrowImage; // some arrow icon
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
                    var puzzleList = new PuzzleListControl(_difficulty, _currentIndex);
                    mainForm.SwitchControl(puzzleList);
                }
            };
            this.Controls.Add(_arrowBackIcon);

            // Congrats label
            _congratsLabel = new Controls.Labels.HeaderLabel();
            _congratsLabel.Text = "Great! You did it!";
            this.Controls.Add(_congratsLabel);
            _congratsLabel.BringToFront();

            // Position everything
            this.Resize += (s, e) => ArrangeLayout();
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

            // Position button
            _congratsLabel.Left = (this.ClientSize.Width - _congratsLabel.Width) / 2;
            _congratsLabel.Top = (int)((this.ClientSize.Height - _puzzlePanel.SideSize) / 2 + _puzzlePanel.SideSize + 10 + 0.1 * (this.ClientSize.Height - 600));
        }
    }
}
