using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls.Labels
{
    /// <summary>
    /// Represents a label for a cell in the puzzle grid.
    /// </summary>
    internal class CellLabel : Label
    {
        public CellLabel(int size)
        {
            // Label settings
            this.AutoSize = false;
            this.Width = size;
            this.Height = size;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.ForeColor = Color.Black;
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
