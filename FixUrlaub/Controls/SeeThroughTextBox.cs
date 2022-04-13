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
            BackColor = Color.FromArgb(
                FixMath.Clamp(Parent.BackColor.R - 10, 0, 255),
                FixMath.Clamp(Parent.BackColor.G - 10, 0, 255),
                FixMath.Clamp(Parent.BackColor.B - 10, 0, 255));
            Font = new Font(((VacPaperForm)Parent).FrutigerFam, 12);
        }

        public SeeThroughTextBox Clone()
            => new SeeThroughTextBox((VacMainForm)Parent)
            {
                Name = this.Name,
                Bounds = this.Bounds,
                Text = this.Text,
                TextAlign = this.TextAlign,
                ForeColor = this.ForeColor
            };
    }
}
