using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JapanezePuzzle.Controls.Buttons
{
    public class OptionButton : Button
    {
        public OptionButton() : base()
        {
            // Button settings
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseOverBackColor = Color.AliceBlue;
            this.FlatAppearance.MouseDownBackColor = Color.LightSteelBlue;
        }
    }
}
