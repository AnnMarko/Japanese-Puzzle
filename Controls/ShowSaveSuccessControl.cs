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
    /// <summary>
    /// This class represents a control that shows a success message after saving a puzzle.
    /// </summary>
    public partial class ShowSaveSuccessControl : TemplateControl
    {
        // Puzzle
        private Controls.Panels.PuzzlePanel _puzzlePanel;

        // Arrow back
        private PictureBox _arrowBackIcon;

        // Success label
        private System.Windows.Forms.Label _successMessageLabel;

        /// <summary>
        /// Constructor for the ShowSaveSuccessControl class.
        /// </summary>
        /// <param name="puzzle"></param>

        public ShowSaveSuccessControl(Classes.Puzzle puzzle)
        {
            InitializeComponent();

            // Background
            this.BackgroundImage = Properties.Resources.lakeBackground;

            // Pannel
            _puzzlePanel = new Panels.PuzzlePanel(puzzle);

            // Show the puzzle
            _puzzlePanel.DrawPuzzle();
            this.Controls.Add(_puzzlePanel);

            // Empty the puzzle and save it to the storage
            puzzle.FillAllCellsWithZero();
            PuzzleStorage.SavePuzzle(puzzle);

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
                    var mainMenu = new MainMenuControl();
                    mainForm.SwitchControl(mainMenu);
                }
            };
            this.Controls.Add(_arrowBackIcon);

            // Congrats label
            _successMessageLabel = new Controls.Labels.HeaderLabel();
            _successMessageLabel.Text = "Your puzzle was saved to easy level puzzles!";
            _successMessageLabel.Font = new Font(this.Font.FontFamily, 17, this.Font.Style);
            this.Controls.Add(_successMessageLabel);
            _successMessageLabel.BringToFront();

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
            _successMessageLabel.Left = (this.ClientSize.Width - _successMessageLabel.Width) / 2;
            _successMessageLabel.Top = (int)((this.ClientSize.Height - _puzzlePanel.SideSize) / 2 + _puzzlePanel.SideSize + 10 + 0.1 * (this.ClientSize.Height - 600));
        }
    }
}
