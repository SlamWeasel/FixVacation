using FixUrlaub.Controls;
using FixUrlaub.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace FixUrlaub.Masks
{
    internal class VacCalendarForm : VacPaperForm
    {
        private readonly new VacMainForm Parent;

        public bool SelectionMode = false;
        public DateRange Selection;
        public DateBox Hovering;

        public DateTime ObservationDate;
        public Label
            ExitIcon,
            LeftLabel, MonthName, RightLabel,
            RangeLabel, 
            RangeDescLabel,
            TeamVacInfoLabel;

        public VacCalendarForm(VacMainForm vacMainForm, Settings set)
        {
            Parent = vacMainForm;
            AppliedTheme = Parent.AppliedTheme;
            Size = new Size(600, 500);
            ObservationDate = new DateTime(2022, 04, 30);
            Selection = new DateRange();

            LoadControls(set);
        }

        private void LoadControls(Settings set)
        {
            #region Icon
            ToolTip ExitTip = new ToolTip()
            {
                AutoPopDelay = 7500,
                InitialDelay = 500,
                ReshowDelay = 200
            };
            ExitIcon = new Label()
            {
                Name = "ExitIcon",
                Text = "X",
                Bounds = new Rectangle(Width - 31, 1, 30, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(FrutigerBoldFam, 20f),
                ForeColor = AppliedTheme.Secondary
            };
            ExitIcon.Click += (object sender, EventArgs e) => this.Close();
            Utils.AddHoverPointer(ExitIcon);
            ExitTip.SetToolTip(ExitIcon, Parent.vsf.cfg.CurrentLanguage.Close);

            Controls.Add(ExitIcon);
            #endregion

            LoadMonth(ObservationDate, set);

            #region Monthcontrols to control the Month
            LeftLabel = new Label()
            {
                Text = "<",
                Name = "LeftLabel",
                Bounds = new Rectangle(20, 430, 50, 50),
                BackColor = AppliedTheme.Primary,
                ForeColor = AppliedTheme.Secondary,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(FrutigerBoldFam, 22)
            };
            LeftLabel.Click += (sender, e) =>
            {
                ObservationDate = ObservationDate.AddMonths(-1);

                Controls.Clear();
                LoadControls(set);
                Invalidate();
            };
            Utils.AddHoverPointer(LeftLabel);
            Controls.Add(LeftLabel);
            MonthName = new Label()
            {
                Text = ObservationDate.ToString("Y"),
                Name = "MonthName",
                Location = new Point(LeftLabel.Location.X + 55, 433),
                BackColor = AppliedTheme.Primary,
                ForeColor = AppliedTheme.Secondary,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(FrutigerBoldFam, 22),
                AutoSize = true
            };
            Controls.Add(MonthName);
            RightLabel = new Label()
            {
                Text = ">",
                Name = "RightLabel",
                Bounds = new Rectangle(MonthName.Location.X + MonthName.Width + 5, 430, 50, 50),
                BackColor = AppliedTheme.Primary,
                ForeColor = AppliedTheme.Secondary,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(FrutigerBoldFam, 22)
            };
            RightLabel.Click += (sender, e) =>
            {
                ObservationDate = ObservationDate.AddMonths(1);

                Controls.Clear();
                LoadControls(set);
                Invalidate();
            };
            Utils.AddHoverPointer(RightLabel);
            Controls.Add(RightLabel);
            #endregion

            #region Range
            RangeLabel = new Label()
            {
                Name = "RangeLabel",
                Bounds = new Rectangle(400, 30, 160, 60),
                ForeColor = AppliedTheme.Secondary,
                Font = new Font(FrutigerBoldFam, 20)
            };
            RangeLabel.MouseDown += OnControlMouseDown;
            RangeDescLabel = new Label()
            {
                Name = "RangeDescLabel",
                Bounds = new Rectangle(400, 100, 160, 25),
                ForeColor = AppliedTheme.Secondary,
                Font = new Font(FrutigerFam, 15)
            };
            RangeDescLabel.MouseDown += OnControlMouseDown;
            TeamVacInfoLabel = new Label()
            {
                Name = "TeamVacInfoLabel",
                Bounds = new Rectangle(400, 135, 160, 240),
                ForeColor = AppliedTheme.Secondary,
                Font = new Font(FrutigerFam, 15)
            };
            TeamVacInfoLabel.MouseDown += OnControlMouseDown;
            #endregion


            Controls.Add(RangeLabel);
            Controls.Add(RangeDescLabel);
            Controls.Add(TeamVacInfoLabel);
        }

        private void LoadMonth(DateTime origin, Settings set)
        {
            DateTime First = origin.AddDays(1 - origin.Day).AddDays(
                origin.AddDays(1 - origin.Day).DayOfWeek == 0 ? 
                    -6 : 
                    1 - (int)origin.AddDays(1 - origin.Day).DayOfWeek);

            for(int monthweek = 0;; monthweek++)
            {
                for (int weekday = 0; weekday < 7; weekday++)
                {
                    DateBox db = new DateBox(this, First.AddDays(weekday + (monthweek * 7)))
                    {
                        Bounds = new Rectangle(20 + (weekday * 53), 20 + (monthweek * 53), 50, 50),
                        TextAlign = ContentAlignment.BottomRight,
                        Font = new Font(FrutigerFam, 20)
                    };
                    db.Name = db.Date.Day + "|" + db.Date.Month + "|" + db.Date.Year;
                    db.Click += (sender, e) =>
                    {
                        if (!SelectionMode)
                        {
                            foreach (Control c in ((Control)sender).Parent.Controls)
                                if (c.Name.Contains("|"))
                                    c.Text = "";

                                    SelectionMode = true;
                            Selection.Start = ((DateBox)sender).Date;
                            ((Control)sender).Text = "X";
                        }
                        else
                        {
                            SelectionMode = false;
                            if (Selection.Start > Hovering.Date)
                            {
                                Selection.End = Selection.Start;
                                Selection.Start = Hovering.Date;
                            }
                            else
                                Selection.End = Hovering.Date;

                            foreach (Control c in ((Control)sender).Parent.Controls)
                                if (c.Name.Contains("|"))
                                    if (Selection.IsInRange(c.Name))
                                        c.Text = "X";

                            UpdateRangeInfo(set);
                        }
                    };
                    db.MouseEnter += (sender, e) =>
                    {
                        Hovering = (DateBox)sender;

                        if (SelectionMode)
                            ((Control)sender).Text = "X";
                    };
                    db.MouseLeave += (sender, e) =>
                    {
                        if (SelectionMode)
                            ((Control)sender).Text = "";
                    };
                    Utils.AddHoverPointer(db);

                    if (db.Date.Month != origin.Month)
                    {
                        db.BackColor = AppliedTheme.Secondary;
                        db.ForeColor = AppliedTheme.Primary;
                    }

                    Controls.Add(db);
                }

                if (First.AddDays(6 + (monthweek * 7)).Month > origin.Month || 
                    (First.AddDays(6 + (monthweek * 7)).Month < origin.Month && First.AddDays(6 + (monthweek * 7)) > origin))
                    break;
            }
        }

        private void OnControlMouseDown(object sender, MouseEventArgs e)
        {
            DoMouseDown(sender, e);
        }
        private void UpdateRangeInfo(Settings set)
        {
            RangeLabel.Text = Selection.ToString();
            RangeDescLabel.Text = (Selection.WorkDays == 1 ? set.CurrentLanguage.Day : set.CurrentLanguage.Days) + ": " + Selection.WorkDays;
        }
    }
}
