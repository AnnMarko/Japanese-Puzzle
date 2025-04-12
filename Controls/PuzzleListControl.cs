using JapanezePuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JapanezePuzzle.Controls
{
    public partial class PuzzleListControl : TemplateControl
    {
        private PictureBox _arrowBackIcon;
        private PictureBox _arrowLeftIcon;
        private PictureBox _arrowRightIcon;

        // The puzzle data
        private List<Puzzle> _puzzles;
        private int _currentIndex;
        private int _difficulty;

        // Two panels for sliding
        private Controls.Panels.PuzzlePanel _currentPanel;
        private Controls.Panels.PuzzlePanel _incomingPanel;

        // Name label
        private Controls.Labels.HeaderLabel _nameLabel;

        // Animation
        private Timer _slideTimer;
        private bool _isSliding = false;
        private int _slideStep = 0;
        private const int MAX_STEPS = 30;  // total frames for the slide
        private int _slideDirection = 0;   // +1: next puzzle (slide left), -1: previous puzzle (slide right)
        private const int PANEL_WIDTH = 400; // puzzle panel width for referencing
        private const int PANEL_OFFSET_DISTANCE = 600; // puzzle panel width for referencing
        private int _panelCenterX; // will store the center X for puzzle panels

        public PuzzleListControl(int difficulty, int currentIndex = 0)
        {
            InitializeComponent();

            _difficulty = difficulty;
            _currentIndex = currentIndex;

            // Background
            this.BackgroundImage = Properties.Resources.gradientBackground;

            _puzzles = new List<Puzzle>();

            // Attempt to load from JSON
            _puzzles = PuzzleStorage.LoadPuzzles();

            // If empty, create them in code and save
            if (_puzzles.Count == 0)
            {
                _puzzles = CreateHardcodedPuzzles();
                PuzzleStorage.SavePuzzles(_puzzles);
            }

            // Get puzzles by difficulty
            _puzzles = _puzzles
                .FindAll(x => x.Difficulty == _difficulty)
                .OrderBy(x => x.IsSolved) // false (0) comes before true (1)
                .ToList();

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
                    _puzzles.Clear();
                    _slideTimer?.Stop();
                    var levelSelection = new LevelSelectionControl();
                    mainForm.SwitchControl(levelSelection);
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
            _arrowLeftIcon.Visible = false;
            this.Controls.Add(_arrowLeftIcon);

            // Right arrow
            _arrowRightIcon = new PictureBox();
            _arrowRightIcon.Image = Properties.Resources.rightArrowImage;
            _arrowRightIcon.SizeMode = PictureBoxSizeMode.Zoom;
            _arrowRightIcon.Size = new Size(50, 50);
            _arrowRightIcon.BackColor = Color.Transparent;
            _arrowRightIcon.Click += ArrowRightIcon_Click;
            _arrowRightIcon.Visible = false;
            this.Controls.Add(_arrowRightIcon);

            // Current puzzle panel
            _currentPanel = new Controls.Panels.PuzzlePanel();
            _currentPanel.SetPuzzle(_puzzles[_currentIndex]);
            _currentPanel.DrawPuzzle();
            _currentPanel.Visible = false;
            for (int i = 0; i < _currentPanel.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < _currentPanel.Cells.GetLength(1); j++)
                {
                    _currentPanel.Cells[i, j].Click += PuzzlePannel_Click;
                }
            }
            this.Controls.Add(_currentPanel);

            // Name of puzzle Label
            _nameLabel = new Controls.Labels.HeaderLabel()
            {
                Text = $"{_puzzles[_currentIndex].Name}",
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_nameLabel);
            _nameLabel.BringToFront();

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

            // hide name of puzzle
            _nameLabel.Visible = false;

            // Create the incoming panel
            _incomingPanel = new Controls.Panels.PuzzlePanel();
            _incomingPanel.SetPuzzle(_puzzles[_currentIndex]);
            _incomingPanel.DrawPuzzle();
            _incomingPanel.Top = _incomingPanel.GetPanelCenterY(this.ClientSize.Height);

            // If going to next puzzle, the new puzzle starts from the right
            if (direction > 0)
            {
                // place it off-screen to the right
                _incomingPanel.Left = _incomingPanel.GetPanelCenterX(this.ClientSize.Width) + PANEL_WIDTH + PANEL_OFFSET_DISTANCE;
            }
            else
            {
                // place it off-screen to the left
                _incomingPanel.Left = _incomingPanel.GetPanelCenterX(this.ClientSize.Width) - PANEL_WIDTH - PANEL_OFFSET_DISTANCE;
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
            int totalDistance = PANEL_WIDTH + PANEL_OFFSET_DISTANCE;
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

                // show name of puzzle
                _nameLabel.Text = $"{_puzzles[_currentIndex].Name}";
                _nameLabel.Visible = true;

                // final position
                _currentPanel.Left = _currentPanel.GetPanelCenterX(this.ClientSize.Width);
                
                for (int i = 0; i < _currentPanel.Cells.GetLength(0); i++)
                {
                    for (int j = 0; j < _currentPanel.Cells.GetLength(1); j++)
                    {
                        _currentPanel.Cells[i, j].Click += PuzzlePannel_Click;
                    }
                }
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

            // Position name label
            _nameLabel.Left = (this.ClientSize.Width - _nameLabel.Width) / 2;
            _nameLabel.Top = panelCenterY - 70;

            if (!_currentPanel.Visible)
            {
                _currentPanel.Visible = true;
            }
        }

        private void PuzzlePannel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _currentPanel.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < _currentPanel.Cells.GetLength(1); j++)
                {
                    _currentPanel.Cells[i, j].Click -= PuzzlePannel_Click;
                }
            }
            var puzzleStartConfirmation = new PuzzleStartConfirmationControl(_difficulty, _currentIndex, _currentPanel);
            ((MainForm)this.ParentForm).SwitchControl(puzzleStartConfirmation);
        }

        private List<Puzzle> CreateHardcodedPuzzles()
        {
            var puzzles = new List<Puzzle>();

            puzzles.Add(
            new Puzzle(
                -1, 13, 12,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 8 },
                        new int[] { 2, 2 },
                        new int[] { 2, 2 },
                        new int[] { 1, 4, 1 },
                        new int[] { 1, 2, 2, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 2, 2, 2 },
                        new int[] { 2, 3, 3 },
                        new int[] { 2 },
                        new int[] { 2, 2 },
                        new int[] { 7 }
                    },
                    new int[][]
                    {
                        new int[] { 8 },
                        new int[] { 2, 2 },
                        new int[] { 2, 2 },
                        new int[] { 1, 5, 2 },
                        new int[] { 1, 2, 2, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 1, 1, 1 },
                        new int[] { 1, 2, 1, 1 },
                        new int[] { 1, 6, 1 },
                        new int[] { 2, 1, 2 },
                        new int[] { 2, 2, 1 },
                        new int[] { 7 }
                    }
                },
                "At",
                1
                )
            );

            puzzles.Add(
            new Puzzle(
                0, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1, 1 },
                        new int[] { 5 },
                        new int[] { 5 },
                        new int[] { 3 },
                        new int[] { 1 },
                    },
                    new int[][]
                    {
                        new int[] { 2 },
                        new int[] { 4 },
                        new int[] { 4 },
                        new int[] { 4 },
                        new int[] { 2 },
                    }
                },
                "Heart"
                )
            );

            puzzles.Add(
            new Puzzle(
                1, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1 },
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1 },
                        new int[] { 1, 1, 1 },
                    },
                    new int[][]
                    {
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1 },
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1 },
                        new int[] { 1, 1, 1 },
                    }
                },
                "Chess"
                )
            );

            puzzles.Add(
            new Puzzle(
                2, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 2 },
                        new int[] { 1 },
                        new int[] { 5 },
                        new int[] { 3 },
                    },
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 2 },
                        new int[] { 5 },
                        new int[] { 1, 2 },
                        new int[] { 1 },
                    }
                },
                "Boat"
                )
            );

            puzzles.Add(
            new Puzzle(
                3, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 2, 1 },
                        new int[] { 4 },
                        new int[] { 2, 1 },
                        new int[] { 1 },
                    },
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 3 },
                        new int[] { 5 },
                        new int[] { 1 },
                        new int[] { 1, 1 },
                    }
                },
                "Fish"
                )
            );

            puzzles.Add(
            new Puzzle(
                4, 5, 5,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 2, 1 },
                        new int[] { 2 },
                        new int[] { 2, 2 },
                        new int[] { 2 },
                        new int[] { 3 },
                    },
                    new int[][]
                    {
                        new int[] { 1, 2 },
                        new int[] { 1, 2 },
                        new int[] { 1, 1 },
                        new int[] { 2, 1 },
                        new int[] { 1, 1, 1 },
                    }
                },
                "Tetris"
                )
            );

            puzzles.Add(
            new Puzzle(
                5, 10, 9,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 3 },
                        new int[] { 2, 2 },
                        new int[] { 2, 2 },
                        new int[] { 9 },
                        new int[] { 7 },
                        new int[] { 1, 1, 1 },
                        new int[] { 1, 1, 1 },
                        new int[] { 4, 1 },
                        new int[] { 4, 1 },
                    },
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 7 },
                        new int[] { 4, 2 },
                        new int[] { 2, 2, 2 },
                        new int[] { 2, 6 },
                        new int[] { 2, 2 },
                        new int[] { 4 },
                        new int[] { 7 },
                        new int[] { 1 },
                    }
                },
                "House",
                1
                )
            );

            puzzles.Add(
            new Puzzle(
                6, 9, 9,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 1 },
                        new int[] { 5 },
                        new int[] { 7 },
                        new int[] { 9 },
                        new int[] { 1, 1, 1, 1, 1 },
                        new int[] { 1 },
                        new int[] { 1 },
                        new int[] { 1, 1 },
                        new int[] { 3 },
                    },
                    new int[][]
                    {
                        new int[] { 2 },
                        new int[] { 2 },
                        new int[] { 4, 2 },
                        new int[] { 3, 1 },
                        new int[] { 9 },
                        new int[] { 3 },
                        new int[] { 4 },
                        new int[] { 2 },
                        new int[] { 2 },
                    }
                },
                "Umbrella",
                1
                )
            );

            puzzles.Add(
            new Puzzle(
                7, 15, 15,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 3 },
                        new int[] { 3, 1 },
                        new int[] { 3, 1 },
                        new int[] { 5, 2 },
                        new int[] { 6, 3 },
                        new int[] { 2, 6, 2 },
                        new int[] { 4, 8 },
                        new int[] { 2, 2, 8 },
                        new int[] { 5, 5, 2 },
                        new int[] { 3, 5, 2 },
                        new int[] { 7, 2 },
                        new int[] { 5, 1 },
                        new int[] { 4, 1 },
                        new int[] { 4 },
                        new int[] { 3 },
                    },
                    new int[][]
                    {
                        new int[] { 2 },
                        new int[] { 4 },
                        new int[] { 2, 2 },
                        new int[] { 7 },
                        new int[] { 1, 3, 2 },
                        new int[] { 3, 3 },
                        new int[] { 10 },
                        new int[] { 12 },
                        new int[] { 13 },
                        new int[] { 4, 6, 3 },
                        new int[] { 2, 4, 2 },
                        new int[] { 1, 2, 1 },
                        new int[] { 4 },
                        new int[] { 8 },
                        new int[] { 4, 5 },
                    }
                },
                "Big fish",
                2
                )
            );

            puzzles.Add(
            new Puzzle(
                8, 15, 18,
                new int[][][]
                {
                    new int[][]
                    {
                        new int[] { 3, 4 },
                        new int[] { 1, 6 },
                        new int[] { 1, 1, 1, 1, 1, 6 },
                        new int[] { 3, 1, 6 },
                        new int[] { 5, 2, 1, 2 },
                        new int[] { 2, 6, 1, 2, 1 },
                        new int[] { 1, 2, 3, 2, 1 },
                        new int[] { 3, 1 },
                        new int[] { 2, 2 },
                        new int[] { 3, 3 },
                        new int[] { 3, 3 },
                        new int[] { 3, 5 },
                        new int[] { 3, 5 },
                        new int[] { 1, 1, 1 },
                        new int[] { 8 },
                    },
                    new int[][]
                    {
                        new int[] { 1, 6 },
                        new int[] { 3, 4 },
                        new int[] { 3, 2, 2 },
                        new int[] { 4, 3 },
                        new int[] { 1, 2, 2, 1 },
                        new int[] { 1, 3 },
                        new int[] { 1, 1 },
                        new int[] { 1, 3 },
                        new int[] { 3, 2, 2, 1 },
                        new int[] { 1, 1, 1, 4 },
                        new int[] { 1, 1, 3, 2, 1 },
                        new int[] { 1, 3, 1 },
                        new int[] { 5, 3 },
                        new int[] { 4, 2, 3 },
                        new int[] { 4, 4 },
                        new int[] { 4 },
                        new int[] { 5 },
                        new int[] { 6 },
                    }
                },
                "Pelican",
                2
                )
            );

            return puzzles;
        }
    }
}
