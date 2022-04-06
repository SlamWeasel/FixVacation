using FixUrlaub.Masks;
using FixUrlaub.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixUrlaub.Controls
{
    internal class SeeThroughTextBox : TextBox
    {
        public SeeThroughTextBox(VacMainForm _Parent) 
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BorderStyle = BorderStyle.None;
            Parent = _Parent;
            BackColor = Color.FromArgb(Parent.BackColor.R - 10, Parent.BackColor.G - 10, Parent.BackColor.B - 10);
            Font = new Font(((VacPaperForm)Parent).FrutigerFam, 12);
        }
    }
}
