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
    public partial class LevelSelectionControl : TemplateControl
    {
        private PictureBox _arrowBackIcon;

        Controls.Buttons.LevelSelectionButton _easyLevelButton;
        Controls.Buttons.LevelSelectionButton _mediumLevelButton;
        Controls.Buttons.LevelSelectionButton _difficultLevelButton;

        public LevelSelectionControl()
        {
            InitializeComponent();

            // Background
            this.BackgroundImage = Properties.Resources.gradientBackground;

            // Level buttons
            //Easy
            _easyLevelButton = new Controls.Buttons.LevelSelectionButton();
            _easyLevelButton.Text = "Easy";
            _easyLevelButton.BackColor = Color.White;
            _easyLevelButton.FlatAppearance.MouseOverBackColor = Color.MintCream;
            _easyLevelButton.FlatAppearance.MouseDownBackColor = Color.Honeydew;
            _easyLevelButton.TextAlign = ContentAlignment.TopCenter;
            _easyLevelButton.Click += EasyButton_Click;

            this.Controls.Add(_easyLevelButton);
            //Medium
            _mediumLevelButton = new Controls.Buttons.LevelSelectionButton();
            _mediumLevelButton.Text = "Medium";
            _mediumLevelButton.BackColor = Color.White;
            _mediumLevelButton.FlatAppearance.MouseOverBackColor = Color.Ivory;
            _mediumLevelButton.FlatAppearance.MouseDownBackColor = Color.LightYellow;
            _mediumLevelButton.TextAlign = ContentAlignment.TopCenter;
            _mediumLevelButton.Click += MediumButton_Click;

            this.Controls.Add(_mediumLevelButton);
            //Difficult
            _difficultLevelButton = new Controls.Buttons.LevelSelectionButton();
            _difficultLevelButton.Text = "Difficult";
            _difficultLevelButton.BackColor = Color.White;
            _difficultLevelButton.FlatAppearance.MouseOverBackColor = Color.SeaShell;
            _difficultLevelButton.FlatAppearance.MouseDownBackColor = Color.MistyRose;
            _difficultLevelButton.TextAlign = ContentAlignment.TopCenter;
            _difficultLevelButton.Click += DifficultButton_Click;

            this.Controls.Add(_difficultLevelButton);


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

            // Position everything
            this.Resize += (s, e) => ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            int buttonWidth = this.ClientSize.Width / 4;
            int buttonHeight = this.ClientSize.Height / 11 * 5;

            // Easy level button
            _easyLevelButton.Width = buttonWidth;
            _easyLevelButton.Height = buttonHeight;
            _easyLevelButton.Left = buttonWidth / 4;
            _easyLevelButton.Top = (this.ClientSize.Height - buttonHeight) / 2;

            // Medium level button
            _mediumLevelButton.Width = buttonWidth;
            _mediumLevelButton.Height = buttonHeight;
            _mediumLevelButton.Left = (this.ClientSize.Width - buttonWidth) / 2;
            _mediumLevelButton.Top = (this.ClientSize.Height - buttonHeight) / 2;

            // Difficult level button
            _difficultLevelButton.Width = buttonWidth;
            _difficultLevelButton.Height = buttonHeight;
            _difficultLevelButton.Left = this.ClientSize.Width - _easyLevelButton.Left - buttonWidth;
            _difficultLevelButton.Top = (this.ClientSize.Height - buttonHeight) / 2;
        }

        // Buttons events
        private void EasyButton_Click(object sender, EventArgs e)
        {
            var levelSelection = new PuzzleListControl(0);
            ((MainForm)this.ParentForm).SwitchControl(levelSelection);
        }

        private void MediumButton_Click(object sender, EventArgs e)
        {
            var levelSelection = new PuzzleListControl(1);
            ((MainForm)this.ParentForm).SwitchControl(levelSelection);
        }

        private void DifficultButton_Click(object sender, EventArgs e)
        {
            var levelSelection = new PuzzleListControl(2);
            ((MainForm)this.ParentForm).SwitchControl(levelSelection);
        }
    }
}
