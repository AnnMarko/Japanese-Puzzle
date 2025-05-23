﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls.Labels
{
    /// <summary>
    /// Represents a label for the name of the puzzle.
    /// </summary>
    public class HeaderLabel : Label
    {
        public HeaderLabel()
        {
            // Label settings
            this.AutoSize = false;
            this.Width = 450;
            this.Height = 40;
            this.Font = new Font(this.Font.FontFamily, 20f, FontStyle.Bold);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.ForeColor = Color.White;
            this.BackColor = Color.LightSlateGray;
        }
    }
}
