using JapanezePuzzle.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace JapanezePuzzle
{
    /// <summary>
    /// Main form of the application.
    /// </summary>
    public partial class MainForm : Form
    {
        // Path to the music file
        private string _tempFile = Path.Combine(Path.GetTempPath(), "musicBackground.mp3");

        // Volume status
        private bool _volumeIsOn;
        public bool VolumeIsOn
        {
            get { return _volumeIsOn; }
            set { _volumeIsOn = value; }
        }

        /// <summary>
        /// Constructor for the MainForm class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // MainForm settings
            this.Text = "Japanese Puzzle";
            this.Width = 1000;
            this.Height = 750;
            this.MinimumSize = new Size(Width, Height);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += (s, e) =>
            {
                this.Location = new Point(
                    (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
                );
            };

            // Music settings
            _volumeIsOn = true;

            File.WriteAllBytes(_tempFile, Properties.Resources.musicBackground);

            axWindowsMediaPlayer1.settings.autoStart = false;
            axWindowsMediaPlayer1.URL = _tempFile;
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            axWindowsMediaPlayer1.settings.volume = 2;
            if (VolumeIsOn)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }

            // JSON settings
            // Attempt to load from JSON
            List<Classes.Puzzle> puzzles = PuzzleStorage.LoadPuzzles();

            // If empty, create them in code and save
            if (puzzles == null || PuzzleStorage.LoadPuzzles().Count == 0)
            {
                puzzles = Classes.Puzzle.CreateHardcodedPuzzles();
                PuzzleStorage.SavePuzzles(puzzles);
            }

            // Start with main menu controls
            SwitchControl(new Controls.MainMenuControl());
        }

        /// <summary>
        /// Switches the current control to a new one.
        /// </summary>
        /// <param name="newControl"></param>
        public void SwitchControl(UserControl newControl)
        {
            this.Controls.Clear();
            this.Controls.Add(newControl);
            newControl.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Switches the music on or off.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SwitchMusic(object sender, EventArgs e)
        {
            if (VolumeIsOn)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }
    }
}