using JapanezePuzzle.Classes;
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
    /// <summary>
    /// This class represents a control for saving user's puzzle.
    /// </summary>
    public partial class PuzzleSavingControl : UserControl
    {
        // Puzzle
        private Classes.Puzzle _puzzle;

        // Puzzle panel
        private Controls.Panels.PuzzlePanel _puzzlePanel;

        // Arrow back
        private PictureBox _arrowBackIcon;

        // Button confirm
        private Controls.Buttons.OptionButton _confirmButton;

        // Name textbox
        private Controls.TextBoxes.NameTextBox _nameTextBox;

        // Placeholder for puzzle name
        private string _placeholder;

        /// <summary>
        /// Constructor for the PuzzleSavingControl class.
        /// </summary>
        /// <param name="puzzle"></param>
        public PuzzleSavingControl(Classes.Puzzle puzzle)
        {
            // Double Buffer
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                      | ControlStyles.UserPaint
                      | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            InitializeComponent();

            // Background
            this.BackgroundImage = Properties.Resources.gradientBackground;

            // Puzzle
            _puzzle = puzzle;

            // Pannel
            _puzzlePanel = new Panels.PuzzlePanel(puzzle, 300);
            _puzzlePanel.DrawPuzzle();
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
                    var puzzleList = new SandboxControl(_puzzle);
                    mainForm.SwitchControl(puzzleList);
                }
            };
            this.Controls.Add(_arrowBackIcon);

            // Confirm button
            _confirmButton = new Controls.Buttons.OptionButton();
            _confirmButton.Text = "Confirm";
            _confirmButton.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _confirmButton.ForeColor = Color.LightSlateGray;
            _confirmButton.Width = 200;
            _confirmButton.Height = 40;
            _confirmButton.TextAlign = ContentAlignment.MiddleCenter;
            _confirmButton.Click += ConfirmButton_Click;
            this.Controls.Add(_confirmButton);

            // Name of puzzle textbox
            _placeholder = "Type name of your puzzle...";
            _nameTextBox = new Controls.TextBoxes.NameTextBox();
            _nameTextBox.Text = _placeholder;
            _nameTextBox.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            // When the textbox gains focus, clear the placeholder text if it's still there
            _nameTextBox.Enter += (s, e) =>
            {
                if (_nameTextBox.Text == _placeholder)
                {
                    _nameTextBox.Text = "";
                }
            };

            // When the textbox loses focus, restore the placeholder text if left empty
            _nameTextBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
                {
                    _nameTextBox.Text = _placeholder;
                }
            };
            this.Controls.Add(_nameTextBox);
            _nameTextBox.BringToFront();

            // Position everything
            this.Resize += (s, e) => ArrangeLayout();
        }

        /// <summary>
        /// Arranges the layout of the controls.
        /// </summary>
        private void ArrangeLayout()
        {
            // The center the puzzle panel
            int puzzlePanelX = _puzzlePanel.GetPanelCenterX(this.ClientSize.Width);
            int panelCenterY = _puzzlePanel.GetPanelCenterY(this.ClientSize.Height) - 60;

            _puzzlePanel.Left = puzzlePanelX;
            _puzzlePanel.Top = panelCenterY;

            // Position name textbox
            _nameTextBox.Left = (this.ClientSize.Width - _nameTextBox.Width) / 2;
            _nameTextBox.Top = (int)(panelCenterY + _puzzlePanel.SideSize + 10 + 0.1 * (this.ClientSize.Height - 600));

            // Position button
            _confirmButton.Left = (this.ClientSize.Width - _confirmButton.Width) / 2;
            _confirmButton.Top = (int)(panelCenterY + _puzzlePanel.SideSize + _nameTextBox.Height + 20 + 0.2 * (this.ClientSize.Height - 600));
        }

        /// <summary>
        /// Handles the click event of the confirm button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            // Check if the name is empty
            if (string.IsNullOrWhiteSpace(_nameTextBox.Text) || _nameTextBox.Text == _placeholder)
            {
                MessageBox.Show("Please enter a name for the puzzle.", "Enter the name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Set puzzle fields
            _puzzle.Name = _nameTextBox.Text;
            _puzzle.IsSolved = false;

            var mainMenu = new ShowSaveSuccessControl(_puzzle);
            ((MainForm)this.ParentForm).SwitchControl(mainMenu);
        }
    }
}
