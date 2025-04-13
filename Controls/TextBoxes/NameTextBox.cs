using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls.TextBoxes
{
    /// <summary>
    /// Custom TextBox for entering puzzle name.
    /// </summary>
    internal class NameTextBox : TextBox
    {
        public NameTextBox()
        {
            // TextBox settings
            this.BorderStyle = BorderStyle.None;
            this.Font = new Font(this.Font.FontFamily, 20f, FontStyle.Bold);
            this.ForeColor = Color.LightSlateGray;
            this.TextAlign = HorizontalAlignment.Center;
            this.MaxLength = 25;
            this.Multiline = false;
            this.Size = new System.Drawing.Size(300, 40);
        }
    }
}
