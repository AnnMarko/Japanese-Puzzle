using JapanezePuzzle.Classes;
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
    public partial class SandboxControl : TemplateControl
    {
        // Puzzle
        private Classes.Puzzle _puzzle;

        // Arrow back
        private PictureBox _arrowBackIcon;

        // Puzzle panel
        private Controls.Panels.PuzzleSandboxPanel _puzzlePanel;

        // Button save
        private Controls.Buttons.OptionButton _saveButton;

        public SandboxControl(Classes.Puzzle puzzle = null)
        {
            InitializeComponent();

            // Background
            this.BackgroundImage = Properties.Resources.waterfallBackground;

            // Puzzle
            if (puzzle == null)
            {
                _puzzle = new Classes.Puzzle(PuzzleStorage.GetMaxId(), 5, 5);
            }
            else
            {
                _puzzle = puzzle;
            }

            // Puzzle panel
            _puzzlePanel = new Controls.Panels.PuzzleSandboxPanel(_puzzle);
            this.Controls.Add(_puzzlePanel);

            // Draw the puzzle
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
                    var mainMenu = new MainMenuControl();
                    mainForm.SwitchControl(mainMenu);
                }
            };
            this.Controls.Add(_arrowBackIcon);

            // Start button
            _saveButton = new Controls.Buttons.OptionButton();
            _saveButton.Text = "Save";
            _saveButton.Width = 200;
            _saveButton.Height = 40;
            _saveButton.TextAlign = ContentAlignment.MiddleCenter;
            _saveButton.Click += SaveButton_Click;
            this.Controls.Add(_saveButton);

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
            _saveButton.Left = (this.ClientSize.Width - _saveButton.Width) / 2;
            _saveButton.Top = (int)((this.ClientSize.Height - _puzzlePanel.SideSize) / 2 + _puzzlePanel.SideSize + 10 + 0.1 * (this.ClientSize.Height - 600));
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Save current puzzle numbers
            _puzzle.PuzzleNumbers = _puzzle.CalculatePuzzleNumbers();

            // Check if puzzle doesn't have any numbers
            if (!_puzzle.HasAtLeastOneNumber())
            {
                MessageBox.Show("Please draw the picture before saving.", "Puzzle is empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var saving = new PuzzleSavingControl(_puzzle);
            ((MainForm)this.ParentForm).SwitchControl(saving);
        }
    }
}
