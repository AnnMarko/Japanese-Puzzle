using JapanezePuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls
{
    public partial class PuzzleListControl : TemplateControl
    {
        private PictureBox _arrowBackIcon;
        private PictureBox _arrowLeftIcon;
        private PictureBox _arrowRightIcon;

        // The puzzle data
        private List<Puzzle> _puzzles = new List<Puzzle>();
        private int _currentIndex = 0;

        // Two panels for sliding
        private PuzzlePanel _currentPanel;
        private PuzzlePanel _incomingPanel;

        // Animation
        private Timer _slideTimer;
        private bool _isSliding = false;
        private int _slideStep = 0;
        private const int MAX_STEPS = 30;  // total frames for the slide
        private int _slideDirection = 0;   // +1: next puzzle (slide left), -1: previous puzzle (slide right)
        private const int PANEL_WIDTH = 400; // puzzle panel width for referencing
        private int _panelCenterX; // will store the center X for puzzle panels

        public PuzzleListControl(int difficulty)
        {
            InitializeComponent();

            // Background
            this.BackgroundImage = Properties.Resources.gradientBackground;

            // Example puzzle data
            _puzzles.Add(new Puzzle { Name = "Puzzle #1", Rows = 5, Cols = 5 });
            _puzzles.Add(new Puzzle { Name = "Puzzle #2", Rows = 10, Cols = 15 });
            _puzzles.Add(new Puzzle { Name = "Puzzle #3", Rows = 15, Cols = 15 });

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

            // Left arrow
            _arrowLeftIcon = new PictureBox();
            _arrowLeftIcon.Image = Properties.Resources.leftArrowImage;
            _arrowLeftIcon.SizeMode = PictureBoxSizeMode.Zoom;
            _arrowLeftIcon.Size = new Size(50, 50);
            _arrowLeftIcon.BackColor = Color.Transparent;
            _arrowLeftIcon.Click += ArrowLeftIcon_Click;
            this.Controls.Add(_arrowLeftIcon);

            // Right arrow
            _arrowRightIcon = new PictureBox();
            _arrowRightIcon.Image = Properties.Resources.rightArrowImage;
            _arrowRightIcon.SizeMode = PictureBoxSizeMode.Zoom;
            _arrowRightIcon.Size = new Size(50, 50);
            _arrowRightIcon.BackColor = Color.Transparent;
            _arrowRightIcon.Click += ArrowRightIcon_Click;
            this.Controls.Add(_arrowRightIcon);

            // Current puzzle panel
            _currentPanel = new PuzzlePanel();
            _currentPanel.SetPuzzle(_puzzles[_currentIndex]);
            this.Controls.Add(_currentPanel);

            // Timer for sliding
            _slideTimer = new Timer();
            _slideTimer.Interval = 10;
            _slideTimer.Tick += SlideTimer_Tick;

            // Position everything
            this.Resize += (s, e) => ArrangeLayout();

            // Make sure arrows are visible or not
            UpdateArrows();
        }

        private void ArrowLeftIcon_Click(object sender, EventArgs e)
        {
            if (_isSliding || _currentIndex <= 0) return;

            // Move to previous puzzle
            _currentIndex--;
            StartSlide(-1);
        }

        private void ArrowRightIcon_Click(object sender, EventArgs e)
        {
            if (_isSliding || _currentIndex >= _puzzles.Count - 1) return;

            // Move to next puzzle
            _currentIndex++;
            StartSlide(+1);
        }

        private void StartSlide(int direction)
        {
            _isSliding = true;
            _slideStep = 0;
            _slideDirection = direction;

            // Create the incoming panel
            _incomingPanel = new PuzzlePanel();
            _incomingPanel.SetPuzzle(_puzzles[_currentIndex]);
            _incomingPanel.Top = _incomingPanel.GetPanelCenterY(this.ClientSize.Height);

            // If going to next puzzle, the new puzzle starts from the right
            if (direction > 0)
            {
                // place it off-screen to the right
                _incomingPanel.Left = _incomingPanel.GetPanelCenterX(this.ClientSize.Width) + PANEL_WIDTH + 200;
            }
            else
            {
                // place it off-screen to the left
                _incomingPanel.Left = _incomingPanel.GetPanelCenterX(this.ClientSize.Width) - PANEL_WIDTH - 200;
            }

            this.Controls.Add(_incomingPanel);
            _incomingPanel.BringToFront();
            _currentPanel.BringToFront();

            // Start the timer
            _slideTimer.Start();

            // Update arrow visibility
            UpdateArrows();
        }

        private void SlideTimer_Tick(object sender, EventArgs e)
        {
            _slideStep++;
            // total horizontal distance = panel width
            // we do an equal fraction each step
            int totalDistance = PANEL_WIDTH + 200;
            int stepDistance = totalDistance / MAX_STEPS;

            if (_slideDirection > 0)
            {
                // next puzzle => move _currentPanel left, _incomingPanel left
                _currentPanel.Left -= stepDistance;
                _incomingPanel.Left -= stepDistance;
            }
            else
            {
                // previous puzzle => move _currentPanel right, _incomingPanel right
                _currentPanel.Left += stepDistance;
                _incomingPanel.Left += stepDistance;
            }

            if (_slideStep >= MAX_STEPS)
            {
                // done sliding
                _slideTimer.Stop();
                _isSliding = false;

                // remove old panel
                this.Controls.Remove(_currentPanel);
                _currentPanel.Dispose();

                // new panel becomes current
                _currentPanel = _incomingPanel;
                _incomingPanel = null;

                // final position
                _currentPanel.Left = _currentPanel.GetPanelCenterX(this.ClientSize.Width);
            }
        }

        private void UpdateArrows()
        {
            _arrowLeftIcon.Visible = (_currentIndex > 0);
            _arrowRightIcon.Visible = (_currentIndex < _puzzles.Count - 1);
        }

        // This arranges the arrows and puzzle panel in the center 
        // whenever the control is resized.
        private void ArrangeLayout()
        {
            // The center X for the puzzle panel
            _panelCenterX = _currentPanel.GetPanelCenterX(this.ClientSize.Width);

            int panelCenterY = _currentPanel.GetPanelCenterY(this.ClientSize.Height);

            if (_currentPanel != null && !_isSliding)
            {
                _currentPanel.Left = _panelCenterX;
                _currentPanel.Top = panelCenterY;
            }

            if (_incomingPanel != null && _isSliding)
            {
                // keep the same Y
                _incomingPanel.Top = panelCenterY;
            }

            // Position left/right arrows
            _arrowLeftIcon.Left = 30;
            _arrowLeftIcon.Top = (this.ClientSize.Height - _arrowLeftIcon.Height) / 2;

            _arrowRightIcon.Left = this.ClientSize.Width - _arrowRightIcon.Width - 30;
            _arrowRightIcon.Top = (this.ClientSize.Height - _arrowRightIcon.Height) / 2;
        }
    }
}
