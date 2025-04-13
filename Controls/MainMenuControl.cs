using JapanezePuzzle.Controls.Buttons;
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
    /// Represents the main menu control in the game.
    /// </summary>
    public partial class MainMenuControl : TemplateControl
    {
        // background
        private Bitmap _backgroundImage = Properties.Resources.mainMenuBackground;

        // Title
        private PictureBox _titleImage;

        // Button Play
        private Controls.Buttons.OptionButton _playButton;

        // Button Sandbox
        private Controls.Buttons.OptionButton _sandboxButton;

        // Button Exit
        private Controls.Buttons.OptionButton _exitButton;

        /// <summary>
        /// Constructor for the MainMenuControl class.
        /// </summary>
        public MainMenuControl()
        {
            InitializeComponent();

            // Title
            _titleImage = new PictureBox();
            _titleImage.Image = Properties.Resources.mainMenuTitle;
            _titleImage.SizeMode = PictureBoxSizeMode.Zoom;
            _titleImage.Size = new Size(580, 180);
            _titleImage.BackColor = Color.Transparent;
            _titleImage.Left = (this.ClientSize.Width - _titleImage.Width) / 2;
            _titleImage.Top = (int)(100 + (this.ClientSize.Height - 450) * 0.1);
            _titleImage.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            // Add title
            this.Controls.Add(_titleImage);

            // Button Play
            _playButton = new Controls.Buttons.OptionButton();
            _playButton.Text = "Play";
            _playButton.Click += PlayButton_Click;
            this.Controls.Add(_playButton);

            // Button Edit
            _sandboxButton = new Controls.Buttons.OptionButton();
            _sandboxButton.Text = "Sandbox";
            _sandboxButton.Click += EditorButton_Click;
            this.Controls.Add(_sandboxButton);

            // Button Exit
            _exitButton = new Controls.Buttons.OptionButton();
            _exitButton.Text = "Exit";
            _exitButton.Click += (s, e) => Application.Exit();
            this.Controls.Add(_exitButton);

            // Position everything
            this.Resize += (s, e) => ArrangeLayout();
        }

        /// <summary>
        /// Arranges the layout of the controls.
        /// </summary>
        private void ArrangeLayout()
        {
            int buttonWidth = this.ClientSize.Width / 4;
            int buttonHeight = 50;
            int buttonCenterX = (this.ClientSize.Width - buttonWidth) / 2;

            // Play button
            _playButton.Width = buttonWidth;
            _playButton.Height = buttonHeight;
            _playButton.Left = buttonCenterX;
            _playButton.Top = this.ClientSize.Height - 3 * buttonHeight - 70;

            // Sandbox button
            _sandboxButton.Width = buttonWidth;
            _sandboxButton.Height = buttonHeight;
            _sandboxButton.Left = buttonCenterX;
            _sandboxButton.Top = this.ClientSize.Height - 2 * buttonHeight - 50;

            // Exit button
            _exitButton.Width = buttonWidth;
            _exitButton.Height = buttonHeight;
            _exitButton.Left = buttonCenterX;
            _exitButton.Top = this.ClientSize.Height - buttonHeight - 30;
        }

        /// <summary>
        /// Handles the click event for the Play button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, EventArgs e)
        {
            var levelSelection = new LevelSelectionControl();
            ((MainForm)this.ParentForm).SwitchControl(levelSelection);
        }

        /// <summary>
        /// Handles the click event for the Sandbox button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditorButton_Click(object sender, EventArgs e)
        {
            var sandbox = new SandboxControl();
            ((MainForm)this.ParentForm).SwitchControl(sandbox);
        }
    }
}
