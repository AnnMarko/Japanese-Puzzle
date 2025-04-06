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
    public partial class MainForm : Form
    {
        // Path to music files
        private string _tempFile = Path.Combine(Path.GetTempPath(), "musicBackground.mp3");

        // Volume status
        private bool _volumeIsOn = false;
        public bool VolumeIsOn
        {
            get { return _volumeIsOn; }
            set { _volumeIsOn = value; }
        }

        public MainForm()
        {
            InitializeComponent();

            // MainForm settings
            this.Text = "Japanese Puzzle";
            this.Width = 800;
            this.Height = 600;
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
            File.WriteAllBytes(_tempFile, Properties.Resources.musicBackground);

            axWindowsMediaPlayer1.settings.autoStart = false;
            axWindowsMediaPlayer1.URL = _tempFile;
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            axWindowsMediaPlayer1.settings.volume = 2;
            //axWindowsMediaPlayer1.Ctlcontrols.play();

            // Start with main menu controls
            SwitchControl(new Controls.MainMenuControl());
        }

        // Change current Control
        public void SwitchControl(UserControl newControl)
        {
            this.Controls.Clear();
            this.Controls.Add(newControl);
            newControl.Dock = DockStyle.Fill;
        }

        // Switch the music
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