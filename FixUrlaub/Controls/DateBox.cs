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

        private string _membersVac;
        public string MembersVac
        {
            get => _membersVac;
            set
            {
                _membersVac = value;

                if (value != null && value != "")
                {
                    ToolTip ReasonTip = new ToolTip()
                    {
                        AutoPopDelay = 20000,
                        InitialDelay = 500,
                        ReshowDelay = 200,
                        UseFading = false,
                        UseAnimation = true,
                        ToolTipIcon = ToolTipIcon.Warning,
                        OwnerDraw = false,
                        ToolTipTitle = Date.ToLongDateString()
                    };
                    ReasonTip.SetToolTip(this, value);
                }
            }
        }

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

            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Parent.AppliedTheme.Secondary), 1),
                                    0, 0, Width - 1, Height - 1);

            if (MembersVac != null && MembersVac != "")
                e.Graphics.FillRectangle(new SolidBrush(Parent.AppliedTheme.Tertiary),
                                        5, Height - 25, 20, 20);
        }
    }
}
