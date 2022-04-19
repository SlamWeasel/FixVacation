using FixUrlaub.Masks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixUrlaub.Controls
{
    internal class DateBox : Label
    {
        /// <summary>
        /// The Date Assigned to the DateBox
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// The <see cref="VacCalendarForm"/> this Control is sitting in
        /// </summary>
        public readonly new VacCalendarForm Parent;

        /// <summary>
        /// Creates a Label, showing the DayNumber of the assigned Date in the top left
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="date"></param>
        public DateBox(VacCalendarForm sender, DateTime date)
        {
            Date = date.Date;
            Parent = sender;
            BorderStyle = BorderStyle.None;
            BackColor = sender.AppliedTheme.Primary;
            ForeColor = sender.AppliedTheme.Secondary;

            ToolTip FullDate = new ToolTip()
            {
                AutoPopDelay = 7500,
                InitialDelay = 500,
                ReshowDelay = 200
            };
            FullDate.SetToolTip(this, Date.ToLongDateString());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if(Date.DayOfWeek == DayOfWeek.Sunday || Date.DayOfWeek == DayOfWeek.Saturday)
                e.Graphics.DrawString(Date.Day.ToString(),
                    new Font(Parent.FrutigerBoldFam, 12),
                    new SolidBrush(ForeColor),
                    new PointF(5, 5));
            else
                e.Graphics.DrawString(Date.Day.ToString(),
                    new Font(Parent.FrutigerFam, 12),
                    new SolidBrush(ForeColor),
                    new PointF(5, 5));

            Pen LinePen = new Pen(new SolidBrush(Parent.AppliedTheme.Secondary), 1);

            e.Graphics.DrawLine(LinePen, 0, 0, Width - 1, 0);
            e.Graphics.DrawLine(LinePen, Width - 1, 0, Width - 1, Height - 1);
            e.Graphics.DrawLine(LinePen, Width - 1, Height - 1, 0, Height - 1);
            e.Graphics.DrawLine(LinePen, 0, Height - 1, 0, 0);
        }
    }
}
