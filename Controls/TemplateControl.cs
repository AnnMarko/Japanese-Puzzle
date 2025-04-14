using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace JapanezePuzzle.Controls
{
    /// <summary>
    /// TemplateControl class represents a user control with a background image and falling petals. It's used by every user control in the application.
    /// </summary>
    public partial class TemplateControl : UserControl
    {
        // Images
        private PictureBox _volumeSettingIcon;   // volume settings
        private Bitmap _petalImage;   // sakura petal

        // Timer
        private Timer _petalTimer;

        // List of petals
        private List<Classes.Petal> _petals = new List<Classes.Petal>();

        // Random
        private Random rand = new Random();

        /// <summary>
        /// Constructor for the TemplateControl class.
        /// </summary>
        public TemplateControl()
        {

            // Enable double buffering for smoothness
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                      | ControlStyles.UserPaint
                      | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            InitializeComponent();

            // Control settings
            this.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.ForeColor = Color.LightSlateGray;
            this.BackColor = Color.White;
            this.Text = "Japanese Puzzle";
            this.MinimumSize = new Size(Width, Height);
            this.BackgroundImage = Properties.Resources.mainMenuBackground;
            this.BackgroundImageLayout = ImageLayout.Zoom;
            this.Width = 800;
            this.Height = 550;
            this.Load += TemplateControl_Load;

            // Sound icon
            _volumeSettingIcon = new PictureBox();
            _volumeSettingIcon.SizeMode = PictureBoxSizeMode.Zoom;
            _volumeSettingIcon.Size = new Size(40, 40);
            _volumeSettingIcon.BackColor = Color.Transparent;

            _volumeSettingIcon.Left = this.ClientSize.Width - _volumeSettingIcon.Width - 30;
            _volumeSettingIcon.Top = this.ClientSize.Height - _volumeSettingIcon.Height - 20;
            _volumeSettingIcon.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _volumeSettingIcon.Click += (s, e) =>
            {
                if (this.ParentForm is MainForm mainForm)
                {
                    mainForm.SwitchMusic(s, e);
                    if (mainForm.VolumeIsOn)
                    {
                        _volumeSettingIcon.Image = Properties.Resources.volumeOffWhite;
                        mainForm.VolumeIsOn = false;
                    }
                    else
                    {
                        _volumeSettingIcon.Image = Properties.Resources.volumeOnWhite;
                        mainForm.VolumeIsOn = true;
                    }
                }
            };

            // Add sound icon
            this.Controls.Add(_volumeSettingIcon);

            // Starting the petal timer
            _petalTimer = new Timer();
            _petalTimer.Interval = 30;
            _petalTimer.Tick += (s, e) => UpdatePetals();
            _petalTimer.Start();

            // Loading a petal from resources
            _petalImage = Properties.Resources.sakuraPetal;
        }

        /// <summary>
        /// Event handler for the Load event of the TemplateControl. It sets the volume icon based on the current volume state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplateControl_Load(object sender, EventArgs e)
        {
            if (this.ParentForm is MainForm mainForm)
            {
                _volumeSettingIcon.Image = mainForm.VolumeIsOn
                    ? Properties.Resources.volumeOnWhite
                    : Properties.Resources.volumeOffWhite;
            }

            // Empty the petals list for new objects
            _petals.Clear();
        }

        /// <summary>
        /// Override the OnPaintBackground method to draw the background image and petals.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Scaling on the smaller side
            if (this.BackgroundImage != null)
            {
                e.Graphics.Clear(this.BackColor);

                Image img = this.BackgroundImage;

                // Form size
                int formWidth = this.ClientSize.Width;
                int formHeight = this.ClientSize.Height;

                // Image size
                float imgAspect = (float)img.Width / img.Height;
                float formAspect = (float)formWidth / formHeight;

                int drawWidth, drawHeight;
                int drawX, drawY;

                // Scaling on the smaller side
                if (formAspect > imgAspect)
                {
                    drawWidth = formWidth;
                    drawHeight = (int)(formWidth / imgAspect);
                }
                else
                {
                    drawHeight = formHeight;
                    drawWidth = (int)(formHeight * imgAspect);
                }

                drawX = (formWidth - drawWidth) / 2;
                drawY = (formHeight - drawHeight) / 2;

                e.Graphics.DrawImage(img, drawX, drawY, drawWidth, drawHeight);
            }
            else
            {
                base.OnPaintBackground(e);
            }

            // Draw petals
            foreach (var p in _petals)
            {
                DrawRotatedImage(e.Graphics, _petalImage, p.X, p.Y, p.Rotation);
            }
        }

        /// <summary>
        /// Update the petals' positions and remove them if they are out of bounds.
        /// </summary>
        private void UpdatePetals()
        {
            // Generate new petal
            if (_petals.Count < 40 && rand.NextDouble() < 0.01)
            {
                _petals.Add(new Classes.Petal
                {
                    // Initial coordinates
                    X = rand.Next(0, this.Width),
                    Y = -20,

                    // Slow falling: 0.5–1.5
                    SpeedY = (float)(0.5 + rand.NextDouble() * 1.0),

                    // Wiggling: -0.95..0.55
                    SpeedX = (float)(rand.NextDouble() * 1.5 - 0.95),

                    // Initial  angle 0..360
                    Rotation = (float)(rand.NextDouble() * 360),

                    // Rotation speed: -0.5..0.5
                    RotationSpeed = (float)(rand.NextDouble() * 1.0 - 0.5)
                });
            }

            // Move petals
            for (int i = _petals.Count - 1; i >= 0; i--)
            {
                var p = _petals[i];

                // Smooth wiggling
                p.X += p.SpeedX + (float)Math.Sin(p.Y / 30f) * 0.5f;

                // Slowly falls down
                p.Y += p.SpeedY;

                // Rotates
                p.Rotation += p.RotationSpeed;

                // Delete if the petal is out of bounds
                if (
                    p.X < -40 ||
                    p.X > this.Width + 40 ||
                    p.Y > this.Height + 40 ||
                    p.Y < -50
                )
                {
                    _petals.RemoveAt(i);
                }
            }

            // Ask for a redraw
            this.Invalidate();
        }

        /// <summary>
        /// Draws a rotated image at the specified coordinates.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="angle"></param>
        private void DrawRotatedImage(Graphics g, Image image, float x, float y, float angle)
        {
            // Save the matrix
            var matrix = g.Transform;

            // Move the origin to (x, y)
            g.TranslateTransform(x, y);

            // Rotate
            g.RotateTransform(angle);

            // Draw (center of picture in (0,0))
            // 20x20 — the actual size of the petal on the screen
            g.DrawImage(image, -10, -10, 20, 20);

            // Restore the matrix
            g.Transform = matrix;
        }
    }
}
