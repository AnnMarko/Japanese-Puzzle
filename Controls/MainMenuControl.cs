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
    public partial class MainMenuControl : TemplateControl
    {
        protected Bitmap _backgroundImage = Properties.Resources.mainMenuBackground;   // background

        public MainMenuControl()
        {
            InitializeComponent();

            // Title
            var title = new PictureBox();
            title.Image = Properties.Resources.mainMenuTitle;
            title.SizeMode = PictureBoxSizeMode.Zoom;
            title.Size = new Size(580, 180);
            title.BackColor = Color.Transparent;
            title.Left = (this.ClientSize.Width - title.Width) / 2;
            title.Top = (int)(100 + (this.ClientSize.Height - 450) * 0.1);
            title.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            // Add title
            this.Controls.Add(title);

            // Button Play
            var playButton = new Controls.Buttons.OptionButton();
            playButton.Text = "Play";
            playButton.Width = 250;
            playButton.Height = 50;
            playButton.Top = this.Height - playButton.Height * 3 - 100;
            playButton.Left = (this.Width - playButton.Width) / 2;
            playButton.Anchor = AnchorStyles.Bottom;
            playButton.Click += PlayButton_Click;

            // Button Edit
            var editorButton = new Controls.Buttons.OptionButton();
            editorButton.Text = "Sandbox";
            editorButton.Width = 250;
            editorButton.Height = 50;
            editorButton.Top = this.Height - editorButton.Height * 2 - 80;
            editorButton.Left = playButton.Left;
            editorButton.Anchor = playButton.Anchor;
            editorButton.Click += EditorButton_Click;

            // Button Exit
            var exitButton = new Controls.Buttons.OptionButton();
            exitButton.Text = "Exit";
            exitButton.Width = 250;
            exitButton.Height = 50;
            exitButton.Top = this.Height - exitButton.Height - 60;
            exitButton.Left = playButton.Left;
            exitButton.Anchor = playButton.Anchor;
            exitButton.Click += (s, e) => Application.Exit();

            // Add buttons
            this.Controls.Add(playButton);
            this.Controls.Add(editorButton);
            this.Controls.Add(exitButton);
        }

        // Button Play Click Event
        private void PlayButton_Click(object sender, EventArgs e)
        {
            var levelSelection = new LevelSelectionControl();
            ((MainForm)this.ParentForm).SwitchControl(levelSelection);
        }

        // Button Edit Click Event
        private void EditorButton_Click(object sender, EventArgs e)
        {
            var editor = new MainMenuControl();
            ((MainForm)this.ParentForm).SwitchControl(editor);
        }
    }
}
