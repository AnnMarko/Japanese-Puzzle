using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls.Buttons
{
    /// <summary>
    /// Represents a button used for level selection in the game.
    /// </summary>
    public class LevelSelectionButton : Button
    {
        public LevelSelectionButton()
        {
            // Button settings
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }
    }
}
