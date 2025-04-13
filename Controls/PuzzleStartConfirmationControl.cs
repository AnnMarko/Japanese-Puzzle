using JapanezePuzzle.Controls.Panels;
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
    public partial class PuzzleStartConfirmationControl : TemplateControl
    {
        // Puzzle
        private Controls.Panels.PuzzlePanel _puzzlePanel;

        // Arrow back
        private PictureBox _arrowBackIcon;

        // Button start solving
        private Controls.Buttons.OptionButton _startButton;

        // Name label
        private Controls.Labels.HeaderLabel _nameLabel;

        // Variables storage
        private int _difficulty;
        private int _currentIndex;

        public PuzzleStartConfirmationControl(int difficulty, int currentIndex, Controls.Panels.PuzzlePanel puzzlePanel)
        {
            InitializeComponent();

            // Background
            this.BackgroundImage = Properties.Resources.gradientBackground;

            // Variables storage
            _difficulty = difficulty;
            _currentIndex = currentIndex;

            // Pannel
            _puzzlePanel = puzzlePanel;
            this.Controls.Add(_puzzlePanel);
            _puzzlePanel.DrawPuzzle();

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
                    var puzzleList = new PuzzleListControl(_difficulty, _currentIndex);
                    mainForm.SwitchControl(puzzleList);
                }
            };
            this.Controls.Add(_arrowBackIcon);

            // Start button
            _startButton = new Controls.Buttons.OptionButton();
            if (_puzzlePanel.GetPuzzle().IsSolved)
            {
                _startButton.Text = "Solve again";
            }
            else
            {
                _startButton.Text = "Start";
            }
            _startButton.Width = 200;
            _startButton.Height = 40;
            _startButton.TextAlign = ContentAlignment.MiddleCenter;
            _startButton.Click += StartButton_Click;
            this.Controls.Add(_startButton);

            // Name of puzzle Label
            _nameLabel = new Controls.Labels.HeaderLabel()
            {
                Text = $"{_puzzlePanel.GetPuzzle().Name}",
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_nameLabel);
            _nameLabel.BringToFront();

            // Position everything
            this.Resize += (s, e) => ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            // The center the puzzle panel
            int puzzlePanelX = _puzzlePanel.GetPanelCenterX(this.ClientSize.Width);
            int panelCenterY = _puzzlePanel.GetPanelCenterY(this.ClientSize.Height);

            _puzzlePanel.Left = puzzlePanelX;
            _puzzlePanel.Top = panelCenterY;

            // Position button
            _startButton.Left = (this.ClientSize.Width - _startButton.Width) / 2;
            _startButton.Top = (int)((this.ClientSize.Height - _puzzlePanel.SideSize) / 2 + _puzzlePanel.SideSize + 10 + 0.1 * (this.ClientSize.Height - 600));

            // Position name label
            _nameLabel.Left = (this.ClientSize.Width - _nameLabel.Width) / 2;
            _nameLabel.Top = panelCenterY - 70;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var solving = new PuzzleSolvingControl(_puzzlePanel.GetPuzzle(), _difficulty, _currentIndex);
            ((MainForm)this.ParentForm).SwitchControl(solving);
        }
    }
}
