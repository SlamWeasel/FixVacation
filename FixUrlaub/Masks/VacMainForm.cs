using FixUrlaub.Util;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using FixUrlaub.Controls;
using System.Drawing.Imaging;

namespace FixUrlaub.Masks
{
    internal class VacMainForm : VacPaperForm
    {
        public VacLeaderForm vlf;
        public VacCalendarForm vcf;
        public VacADLogin vadl;
        public VacSettingsForm vsf;

        public ADUser User;

        private int xHeight;
        private float HeightRatio = 0.462963f;
        private int xWidth;
        private float WidthRatio = 1;

        #region Controls
        public CheckBox
            YearCheck,
            SpecCheck,
            UnpaidCheck;
        public Label
            SettingsIcon,
            ExitIcon,
            NameLine,
            BirthLine,
            IDLine,
            DepLine,
            YearAnnouncement,
            TakenVacLabel,
            AnnounceLabel,
            YearVacLabel,
            SpecVacLabel,
            UnpaidVacLabel,
            ReasonLabel,
            FromLabel, ToLabel;
        public SeeThroughTextBox 
            NameLineField, 
            IDLineField, 
            DepLineField, 
            CurrentYearField, 
            TakenVacField, 
            LeftVacField,
            Res1, Res2, Res3,
            ReasonField;
        public DateTimePicker BirthLineField, From1, From2, From3, To1, To2, To3;
        public Button Submit, Calendar, Approve;
        #endregion

        public VacMainForm(ADUser u) : base("VacMainForm")
        {
            #region Child-Forms
            vlf = new VacLeaderForm(this);
            vcf = new VacCalendarForm(this);
            vadl = new VacADLogin(this);
            vsf = new VacSettingsForm(this, new Settings() { CurrentLanguage = Language.German, Theme = AppliedTheme});
            vsf.FormClosed += (sender, e) =>
                {
                    Controls.Clear();
                    LoadControls();
                    Invalidate();
                };
            #endregion

            Bounds = new Rectangle(200, 200, (int)Math.Round(500 * FixMath.VacationFormularAspect, 0), 500);
            xHeight = Height;
            xWidth = Width;
            User = u;

            LoadControls();
        }

        public void LoadControls()
        {
            Language lang = vsf.cfg.CurrentLanguage;

            BackColor = AppliedTheme.Primary;
            ForeColor = AppliedTheme.Secondary;

            #region Icons
            ToolTip SettingsTip = new ToolTip()
            {
                AutoPopDelay = 7500,
                InitialDelay = 500,
                ReshowDelay = 200,
                ToolTipTitle = lang.Settings
            };
            SettingsIcon = new Label()
            {
                Name = "SettingsIcon",
                Bounds = new Rectangle(1, 1, 30, 30)
            };
            SettingsIcon.Paint += (object sender, PaintEventArgs e) => 
                {
                    // Swaps white with the Secondary Color and uses those changes attributed to draw the new bitmap

                    using (Bitmap bmp = ((Icon)resources.GetObject("Vac_Gear")).ToBitmap())
                    {
                        ColorMap[] colorMap = new ColorMap[1];
                        colorMap[0] = new ColorMap();
                        colorMap[0].OldColor = Color.White;
                        colorMap[0].NewColor = AppliedTheme.Secondary;
                        ImageAttributes attr = new ImageAttributes();
                        attr.SetRemapTable(colorMap);
                        
                        Rectangle rect = new Rectangle(0, 0, ((Control)sender).Width + 10, ((Control)sender).Height + 10);
                        e.Graphics.DrawImage(bmp, rect, 0, 0, 350, 350, GraphicsUnit.Point, attr);  // Using Point as Unit, so it renders it out smoothly
                    }
                };
            SettingsIcon.Click += (object sender, EventArgs e) =>
                {
                    SettingsIcon.BeginInvoke(new Action(() =>
                    {
                        vsf.ShowDialog();
                        vsf.BringToFront();

                        Console.WriteLine(vsf.Parent.Name);
                    }
                    ));
                };
            SettingsTip.SetToolTip(SettingsIcon, lang.SettingsDesc);
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
            ExitTip.SetToolTip(ExitIcon, lang.Close);


            Controls.Add(SettingsIcon);
            Controls.Add(ExitIcon);
            #endregion


            #region Labels and Texts
            NameLine = new Label()
            {
                Text = lang.NameLine,
                Name = lang.NameLine,
                Location = new Point(Width / 2 - 50, 30),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true,
                ForeColor = AppliedTheme.Secondary
            };
            BirthLine = new Label()
            {
                Text = lang.BornLine,
                Name = lang.BornLine,
                Location = new Point(Width / 2 - 50, 70),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true,
                ForeColor = vsf.AppliedTheme.Secondary
            };
            IDLine = new Label()
            {
                Text = " " + lang.UserIDLine,
                Name = lang.UserIDLine,
                Location = new Point((int)Math.Round(Width / 1.25f - 50), 70),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true,
                ForeColor = vsf.AppliedTheme.Secondary
            };
            DepLine = new Label()
            {
                Text = lang.DepartmentLine,
                Name = lang.DepartmentLine,
                Location = new Point(Width / 2 - 50, 110),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true,
                ForeColor = vsf.AppliedTheme.Secondary
            };

            YearAnnouncement = new Label()
            {
                Text = lang.YearTag,
                Name = lang.YearTag,
                Location = new Point(35, 145),
                Font = new Font(FrutigerBoldFam, 12),
                AutoSize = true,
                ForeColor = vsf.AppliedTheme.Secondary
            };

            Controls.Add(NameLine);
            Controls.Add(BirthLine);
            Controls.Add(IDLine);
            Controls.Add(DepLine);

            Controls.Add(YearAnnouncement);
            #endregion


            #region Textfields
            NameLineField = new SeeThroughTextBox(this)
            {
                Text = User.FullName,
                Name = "NameLineField",
                ForeColor = AppliedTheme.Tertiary,
                Bounds = new Rectangle(NameLine.Location.X + NameLine.Width, 23, Width - (NameLine.Location.X + NameLine.Width) - 30, 20)
            };
            BirthLineField = new DateTimePicker()
            {
                Name = "BirthLineField",
                Bounds = new Rectangle((Width / 2 - 50) + BirthLine.Width, 60, IDLine.Location.X - ((Width / 2 - 50) + BirthLine.Width), 20),
                CalendarFont = new Font(FrutigerFam, 12),
                Font = new Font(FrutigerFam, 12),
                CalendarForeColor = AppliedTheme.Secondary,
                CalendarTrailingForeColor = AppliedTheme.Secondary,
                CalendarMonthBackground = AppliedTheme.Primary,
                CalendarTitleBackColor = ColorTheme.InvertColor(AppliedTheme.Secondary),
                CalendarTitleForeColor = AppliedTheme.Secondary,
                Value = DateTime.Today.AddDays(30),                         // TODO: Put in Birthday Date Automatically
                Format = DateTimePickerFormat.Short
            };
            IDLineField = new SeeThroughTextBox(this)
            {
                Text = User.ID,
                Name = "IDLineField",
                ForeColor = AppliedTheme.Tertiary,
                Bounds = new Rectangle(IDLine.Location.X + IDLine.Width, 63, Width - (IDLine.Location.X + IDLine.Width) - 30, 20)
            };
            DepLineField = new SeeThroughTextBox(this)
            {
                Text = User.Department,
                Name = "DepLineField",
                ForeColor = AppliedTheme.Tertiary,
                Bounds = new Rectangle(DepLine.Location.X + DepLine.Width, 103, Width - (DepLine.Location.X + DepLine.Width) - 30, 20)
            };
            CurrentYearField = new SeeThroughTextBox(this)
            {
                Text = DateTime.Today.Year.ToString(),
                Name = "CurrentYearField",
                ForeColor = AppliedTheme.Tertiary,
                Bounds = new Rectangle(YearAnnouncement.Location.X + YearAnnouncement.Width, 145, 50, 20)
            };
            TakenVacField = new SeeThroughTextBox(this)
            {
                Text = "",                                                  // TODO: Put Vacation Data in here
                Name = "TakenVacField",
                ForeColor = AppliedTheme.Tertiary,
                Bounds = new Rectangle(35, 190, 40, 20)
            };
            Controls.Add(TakenVacField);
            TakenVacLabel = new Label()
            {
                Text = lang.RemainingVac,
                Name = "TakenVacLabel",
                ForeColor = AppliedTheme.Secondary,
                Location = new Point(TakenVacField.Location.X + TakenVacField.Width, TakenVacField.Location.Y - 4),
                Font = new Font(FrutigerBoldFam, 12),
                AutoSize = true,
                TextAlign = ContentAlignment.BottomLeft
            };
            Controls.Add(TakenVacLabel);
            AnnounceLabel = new Label()
            {
                Text = lang.Announcement,
                Name = "AnnounceLabel",
                ForeColor = AppliedTheme.Secondary,
                Location = new Point(35, TakenVacLabel.Location.Y + 30),
                Font = new Font(FrutigerBoldFam, 12),
                AutoSize = true
            };
            Controls.Add(AnnounceLabel);
            YearVacLabel = new Label()
            {
                Text = lang.YearVac,
                Name = "YearVaclabel",
                ForeColor = AppliedTheme.Secondary,
                Location = new Point(35, AnnounceLabel.Location.Y + 30),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true
            };
            Controls.Add(YearVacLabel);
            YearCheck = new CheckBox()
            {
                Name = "YearCheck",
                Checked = true,
                FlatStyle = FlatStyle.Flat,
                BackColor = AppliedTheme.Primary,
                ForeColor = AppliedTheme.Secondary,
                Bounds = new Rectangle(24, YearVacLabel.Location.Y + 3, 12, 12)
            };
            YearCheck.CheckedChanged += OnYearCheckChanged;
            Controls.Add(YearCheck);
            SpecVacLabel = new Label()
            {
                Text = lang.SpecVac,
                Name = "SpecVacLabel",
                ForeColor = AppliedTheme.Secondary,
                Location = new Point(35, YearVacLabel.Location.Y + 25),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true
            };
            Controls.Add(SpecVacLabel);
            SpecCheck = new CheckBox()
            {
                Name = "SpecCheck",
                Checked = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = AppliedTheme.Primary,
                ForeColor = AppliedTheme.Secondary,
                Bounds = new Rectangle(24, SpecVacLabel.Location.Y + 3, 12, 12)
            };
            SpecCheck.CheckedChanged += OnSpecCheckChanged;
            Controls.Add(SpecCheck);
            UnpaidVacLabel = new Label()
            {
                Text = lang.UnpaidVac,
                Name = "UnpaidVacLabel",
                ForeColor = AppliedTheme.Secondary,
                Location = new Point(35, SpecVacLabel.Location.Y + 25),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true
            };
            Controls.Add(UnpaidVacLabel);
            UnpaidCheck = new CheckBox()
            {
                Name = "UnpaidCheck",
                Checked = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = AppliedTheme.Primary,
                ForeColor = AppliedTheme.Secondary,
                Bounds = new Rectangle(24, UnpaidVacLabel.Location.Y + 3, 12, 12)
            };
            UnpaidCheck.CheckedChanged += OnUnpaidCheckChanged;
            Controls.Add(UnpaidCheck);
            ReasonLabel = new Label()
            {
                Text = lang.Reason,
                Name = "ReasonLabel",
                ForeColor = AppliedTheme.Secondary,
                Location = new Point(35, UnpaidVacLabel.Location.Y + 55),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true
            };
            Controls.Add(ReasonLabel);

            LeftVacField = new SeeThroughTextBox(this)
            {
                Text = "",                                                  // TODO: Put Vacation Data in here
                Name = "LeftVacField-Left",
                ForeColor = AppliedTheme.Tertiary,
                Bounds = new Rectangle(500, 190, 100, 20),
                TextAlign = HorizontalAlignment.Center
            };
            Controls.Add(LeftVacField);
            Res1 = LeftVacField.Clone();
            Res1.Location = new Point(500, YearVacLabel.Location.Y);
            Res1.Name = "LeftVacField-Year";
            Res1.TextChanged += OnResTextChange;
            Controls.Add(Res1);
            Res2 = LeftVacField.Clone();
            Res2.Location = new Point(500, SpecVacLabel.Location.Y);
            Res2.Name = "LeftVacField-Spec";
            Res2.TextChanged += OnResTextChange;
            Controls.Add(Res2);
            Res3 = LeftVacField.Clone();
            Res3.Location = new Point(500, UnpaidVacLabel.Location.Y);
            Res3.Name = "LeftVacField-Unpaid";
            Res3.TextChanged += OnResTextChange;
            Controls.Add(Res3);

            FromLabel = new Label()
            {
                Text = lang.From,
                Name = lang.From,
                ForeColor = AppliedTheme.Secondary,
                Location = new Point(225, YearVacLabel.Location.Y),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true
            };
            Controls.Add(FromLabel);
            Label FromLabel_ = new Label()
            {
                Text = FromLabel.Text,
                Name = FromLabel.Name,
                ForeColor = FromLabel.ForeColor,
                Location = new Point(FromLabel.Location.X, SpecVacLabel.Location.Y),
                Font = FromLabel.Font,
                AutoSize = true
            };
            Controls.Add(FromLabel_);
            Label FromLabel__ = new Label()
            {
                Text = FromLabel.Text,
                Name = FromLabel.Name,
                ForeColor = FromLabel.ForeColor,
                Location = new Point(FromLabel.Location.X, UnpaidVacLabel.Location.Y),
                Font = FromLabel.Font,
                AutoSize = true
            };
            Controls.Add(FromLabel__);


            From1 = new DateTimePicker()
            {
                Name = "From1",
                Bounds = new Rectangle(FromLabel.Location.X + FromLabel.Width, FromLabel.Location.Y, 80, 15),
                CalendarFont = new Font(FrutigerFam, 8),
                Font = new Font(FrutigerFam, 8),
                CalendarForeColor = AppliedTheme.Secondary,
                CalendarTrailingForeColor = AppliedTheme.Secondary,
                CalendarMonthBackground = AppliedTheme.Primary,
                CalendarTitleBackColor = ColorTheme.InvertColor(AppliedTheme.Secondary),
                CalendarTitleForeColor = AppliedTheme.Secondary,
                Value = DateTime.Now,
                Format = DateTimePickerFormat.Short
            };
            From1.ValueChanged += OnDatePicker_DateChanged1;
            Controls.Add(From1);
            From2 = new DateTimePicker()
            {
                Name = "From2",
                Bounds = new Rectangle(FromLabel_.Location.X + FromLabel_.Width, FromLabel_.Location.Y, 80, 15),
                CalendarFont = new Font(FrutigerFam, 8),
                Font = new Font(FrutigerFam, 8),
                CalendarForeColor = AppliedTheme.Secondary,
                CalendarTrailingForeColor = AppliedTheme.Secondary,
                CalendarMonthBackground = AppliedTheme.Primary,
                CalendarTitleBackColor = ColorTheme.InvertColor(AppliedTheme.Secondary),
                CalendarTitleForeColor = AppliedTheme.Secondary,
                Value = DateTime.Now,
                Format = DateTimePickerFormat.Short,
                Enabled = false
            };
            From2.ValueChanged += OnDatePicker_DateChanged2;
            Controls.Add(From2);
            From3 = new DateTimePicker()
            {
                Name = "From3",
                Bounds = new Rectangle(FromLabel__.Location.X + FromLabel__.Width, FromLabel__.Location.Y, 80, 15),
                CalendarFont = new Font(FrutigerFam, 8),
                Font = new Font(FrutigerFam, 8),
                CalendarForeColor = AppliedTheme.Secondary,
                CalendarTrailingForeColor = AppliedTheme.Secondary,
                CalendarMonthBackground = AppliedTheme.Primary,
                CalendarTitleBackColor = ColorTheme.InvertColor(AppliedTheme.Secondary),
                CalendarTitleForeColor = AppliedTheme.Secondary,
                Value = DateTime.Now,
                Format = DateTimePickerFormat.Short,
                Enabled = false
            };
            From3.ValueChanged += OnDatePicker_DateChanged3;
            Controls.Add(From3);
            ToLabel = new Label()
            {
                Text = lang.To,
                Name = lang.To,
                ForeColor = AppliedTheme.Secondary,
                Location = new Point(From1.Location.X + From1.Width, YearVacLabel.Location.Y),
                Font = new Font(FrutigerFam, 12),
                AutoSize = true
            };
            Controls.Add(ToLabel);
            Label ToLabel_ = new Label()
            {
                Text = ToLabel.Text,
                Name = ToLabel.Name,
                ForeColor = ToLabel.ForeColor,
                Location = new Point(From2.Location.X + From2.Width, SpecVacLabel.Location.Y),
                Font = ToLabel.Font,
                AutoSize = true
            };
            Controls.Add(ToLabel_);
            Label ToLabel__ = new Label()
            {
                Text = ToLabel.Text,
                Name = ToLabel.Name,
                ForeColor = ToLabel.ForeColor,
                Location = new Point(From3.Location.X + From3.Width, UnpaidVacLabel.Location.Y),
                Font = ToLabel.Font,
                AutoSize = true
            };
            Controls.Add(ToLabel__);
            To1 = new DateTimePicker()
            {
                Name = "To1",
                Bounds = new Rectangle(ToLabel.Location.X + ToLabel.Width, ToLabel.Location.Y, 80, 15),
                CalendarFont = new Font(FrutigerFam, 8),
                Font = new Font(FrutigerFam, 8),
                CalendarForeColor = AppliedTheme.Secondary,
                CalendarTrailingForeColor = AppliedTheme.Secondary,
                CalendarMonthBackground = AppliedTheme.Primary,
                CalendarTitleBackColor = ColorTheme.InvertColor(AppliedTheme.Secondary),
                CalendarTitleForeColor = AppliedTheme.Secondary,
                Value = DateTime.Now,
                Format = DateTimePickerFormat.Short
            };
            To1.ValueChanged += OnDatePicker_DateChanged1;
            Controls.Add(To1);
            To2 = new DateTimePicker()
            {
                Name = "To2",
                Bounds = new Rectangle(ToLabel_.Location.X + ToLabel_.Width, ToLabel_.Location.Y, 80, 15),
                CalendarFont = new Font(FrutigerFam, 8),
                Font = new Font(FrutigerFam, 8),
                CalendarForeColor = AppliedTheme.Secondary,
                CalendarTrailingForeColor = AppliedTheme.Secondary,
                CalendarMonthBackground = AppliedTheme.Primary,
                CalendarTitleBackColor = ColorTheme.InvertColor(AppliedTheme.Secondary),
                CalendarTitleForeColor = AppliedTheme.Secondary,
                Value = DateTime.Now,
                Format = DateTimePickerFormat.Short,
                Enabled = false
            };
            To2.ValueChanged += OnDatePicker_DateChanged2;
            Controls.Add(To2);
            To3 = new DateTimePicker()
            {
                Name = "To3",
                Bounds = new Rectangle(ToLabel__.Location.X + ToLabel__.Width, ToLabel__.Location.Y, 80, 15),
                CalendarFont = new Font(FrutigerFam, 8),
                Font = new Font(FrutigerFam, 8),
                CalendarForeColor = AppliedTheme.Secondary,
                CalendarTrailingForeColor = AppliedTheme.Secondary,
                CalendarMonthBackground = AppliedTheme.Primary,
                CalendarTitleBackColor = ColorTheme.InvertColor(AppliedTheme.Secondary),
                CalendarTitleForeColor = AppliedTheme.Secondary,
                Value = DateTime.Now,
                Format = DateTimePickerFormat.Short,
                Enabled = false
            };
            To3.ValueChanged += OnDatePicker_DateChanged3;
            Controls.Add(To3);
            ReasonField = new SeeThroughTextBox(this)
            {
                Text = "",
                Name = "ReasonField",
                ForeColor = AppliedTheme.Tertiary,
                Bounds = new Rectangle(ReasonLabel.Location.X + ReasonLabel.Width, ReasonLabel.Location.Y, Width - (ReasonLabel.Location.X + ReasonLabel.Width) - 50, 20),
                TextAlign = HorizontalAlignment.Left
            };
            Controls.Add(ReasonField);



            Controls.Add(NameLineField);
            Controls.Add(BirthLineField);
            Controls.Add(IDLineField);
            Controls.Add(DepLineField);

            Controls.Add(CurrentYearField);
            #endregion


            #region Buttons
            Submit = new Button()
            {
                Text = lang.Submit,
                Name = "Submit",
                ForeColor = AppliedTheme.Secondary,
                BackColor = ColorTheme.AddColor(AppliedTheme.Primary, Color.FromArgb(20, 20, 20)),
                Font = new Font(FrutigerBoldFam, 12),
                FlatStyle = FlatStyle.Popup,
                Bounds = new Rectangle(50, 450, 150, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Submit.Click += OnSubmitClick;
            Submit.MouseEnter += OnColorInvert;
            Submit.MouseLeave += OnColorInvert;
            Calendar = new Button()
            {
                Text = lang.Calendar,
                Name = "Calendar",
                ForeColor = AppliedTheme.Secondary,
                BackColor = ColorTheme.AddColor(AppliedTheme.Primary, Color.FromArgb(20, 20, 20)),
                Font = new Font(FrutigerBoldFam, 12),
                FlatStyle = FlatStyle.Popup,
                Bounds = new Rectangle(275, 450, 150, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Calendar.Click += OnCalendarClick;
            Calendar.MouseEnter += OnColorInvert;
            Calendar.MouseLeave += OnColorInvert;

            Approve = new Button()
            {
                Text = lang.Approve,
                Name = "Approve",
                ForeColor = AppliedTheme.Secondary,
                BackColor = ColorTheme.AddColor(AppliedTheme.Primary, Color.FromArgb(20, 20, 20)),
                Font = new Font(FrutigerBoldFam, 12),
                FlatStyle = FlatStyle.Popup,
                Bounds = new Rectangle(500, 450, 150, 40),
                TextAlign = ContentAlignment.MiddleCenter,

                Enabled = User.IsLeader ? true : false
            };
            Approve.Click += OnApproveClick;
            Approve.MouseEnter += OnColorInvert;
            Approve.MouseLeave += OnColorInvert;

            Controls.Add(Submit);
            Controls.Add(Calendar);
            Controls.Add(Approve);
            #endregion
        }

        private void OnColorInvert(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = ColorTheme.InvertColor(((Control)sender).BackColor);
        }
        private void OnApproveClick(object sender, EventArgs e)
        {
            vlf.ShowDialog(this);
            vlf.BringToFront();
        }
        private void OnCalendarClick(object sender, EventArgs e)
        {
            
        }
        private void OnSubmitClick(object sender, EventArgs e)
        {
            
        }

        private void OnResTextChange(object sender, EventArgs e)
        {
            Invalidate();
        }
        private void OnYearCheckChanged(object sender, EventArgs e)
        {
            From1.Enabled = ((CheckBox)sender).Checked;
            To1.Enabled = ((CheckBox)sender).Checked;
        }
        private void OnSpecCheckChanged(object sender, EventArgs e)
        {
            From2.Enabled = ((CheckBox)sender).Checked;
            To2.Enabled = ((CheckBox)sender).Checked;
        }
        private void OnUnpaidCheckChanged(object sender, EventArgs e)
        {
            From3.Enabled = ((CheckBox)sender).Checked;
            To3.Enabled = ((CheckBox)sender).Checked;
        }
        private void OnDatePicker_DateChanged1(object sender, EventArgs e)
        {
            try
            {
                int days = 1;
                for (int i = 0; To1.Value.Subtract(From1.Value.AddDays(i)).Days > 0; i++)
                {
                    if (From1.Value.AddDays(i).DayOfWeek == DayOfWeek.Sunday || From1.Value.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                        continue;
                    days++;
                }
                Res1.Text = days.ToString();

                Invalidate();
            }
            catch
            {
                Res1.Text = "Fehler";
            }
        }
        private void OnDatePicker_DateChanged2(object sender, EventArgs e)
        {
            try
            {
                int days = 1;
                for (int i = 0; To2.Value.Subtract(From2.Value.AddDays(i)).Days > 0; i++)
                {
                    if (From2.Value.AddDays(i).DayOfWeek == DayOfWeek.Sunday || From2.Value.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                        continue;
                    days++;
                }
                Res2.Text = days.ToString();

                Invalidate();
            }
            catch
            {
                Res2.Text = "Fehler";
            }
        }
        private void OnDatePicker_DateChanged3(object sender, EventArgs e)
        {
            try
            {
                int days = 1;
                for (int i = 0; To3.Value.Subtract(From3.Value.AddDays(i)).Days > 0; i++)
                {
                    if (From3.Value.AddDays(i).DayOfWeek == DayOfWeek.Sunday || From3.Value.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                        continue;
                    days++;
                }
                Res3.Text = days.ToString();

                Invalidate();
            }
            catch
            {
                Res3.Text = "Fehler";
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            int SizeRatio = (int)Math.Round((Height - 470) / 30.0f);
            Pen LinePen = new Pen(AppliedTheme.Secondary, 2 + (SizeRatio / 5));


            #region Drawn in Labels
            e.Graphics.DrawString("fixemer", 
                new Font(FrutigerBoldFam, 24 + SizeRatio), 
                new SolidBrush(AppliedTheme.Secondary),
                30 + (Width / 1200 * 25),
                20 + (Height / 1000 * 25));
            e.Graphics.DrawString(vsf.cfg.CurrentLanguage.LogoTopLeft,
                new Font(FrutigerBoldFam, 18 + SizeRatio),
                new SolidBrush(AppliedTheme.Secondary),
                30 + (Width / 1200 * 25),
                55 + (Height / 1000 * 25) + SizeRatio);
            e.Graphics.DrawString(vsf.cfg.CurrentLanguage.EmployeeField,
                new Font(FrutigerFam, 7 + (int)Math.Round(SizeRatio * 0.7f)),
                new SolidBrush(AppliedTheme.Secondary),
                new RectangleF(60f + (6 * SizeRatio), 360f + (23 * SizeRatio), (250 + (25 * SizeRatio)) - (60 + (6 * SizeRatio)), 100f));
            e.Graphics.DrawString(vsf.cfg.CurrentLanguage.SuperiorField,
                new Font(FrutigerFam, 7 + (int)Math.Round(SizeRatio * 0.7f)),
                new SolidBrush(AppliedTheme.Secondary),
                new RectangleF(270 + (27 * SizeRatio), 360f + (23 * SizeRatio), (450 + (45 * SizeRatio)) - (270 + (27 * SizeRatio)), 100f));
            e.Graphics.DrawString(vsf.cfg.CurrentLanguage.HRField,
                new Font(FrutigerFam, 7 + (int)Math.Round(SizeRatio * 0.7f)),
                new SolidBrush(AppliedTheme.Secondary),
                new RectangleF(470 + (47 * SizeRatio), 360f + (23 * SizeRatio), Width - (470 + (47 * SizeRatio)), 100f));
            e.Graphics.DrawString(DateTime.Now.Date.ToShortDateString() + " " + User.FullName,
                new Font(FrutigerFam, 9 + (int)Math.Round(SizeRatio * 0.85f)),
                new SolidBrush(AppliedTheme.Tertiary),
                new RectangleF(40f + (4 * SizeRatio), 375f + (25 * SizeRatio), (250 + (25 * SizeRatio)) - (40 + (4 * SizeRatio)), 100f));
            #endregion

            #region Structural Lines
            Point[] LinePath = new Point[4]
            {
                new Point(Width, DepLine.Location.Y + DepLine.Height + 10 - (SizeRatio / 5)),
                new Point(20 + (2 * SizeRatio),DepLine.Location.Y + DepLine.Height + 10 - (SizeRatio / 5)),
                new Point(20 + (2 * SizeRatio), 355 + (23 * SizeRatio)),
                new Point(Width, 355 + (23 * SizeRatio))
            };
            byte[] LinePathTypes = new byte[4]
            {
                (byte)PathPointType.Line,
                (byte)PathPointType.Line,
                (byte)PathPointType.Line,
                (byte)PathPointType.Line
            };
            e.Graphics.DrawPath(LinePen, new GraphicsPath(LinePath, LinePathTypes));
            e.Graphics.DrawLine(LinePen,
                20 + (2 * SizeRatio),
                200 + (13 * SizeRatio),
                Width,
                200 + (13 * SizeRatio));
            e.Graphics.DrawLine(new Pen(AppliedTheme.Secondary, 6 + (SizeRatio / 2)),
                new Point(600 + (60 * SizeRatio), DepLine.Location.Y + DepLine.Height + 10 - (SizeRatio / 5)),
                new Point(600 + (60 * SizeRatio), 355 + (23 * SizeRatio)));
            e.Graphics.DrawLine(LinePen,
                new Point(250 + (25 * SizeRatio), 355 + (23 * SizeRatio)),
                new Point(250 + (25 * SizeRatio), 400 + (27 * SizeRatio)));
            e.Graphics.DrawLine(LinePen,
                new Point(450 + (45 * SizeRatio), 355 + (23 * SizeRatio)),
                new Point(450 + (45 * SizeRatio), 400 + (27 * SizeRatio)));
            #endregion

            #region Text Lines
            e.Graphics.DrawLine(pen: LinePen, 
                x1: NameLine.Location.X + NameLine.Width,
                y1: NameLine.Location.Y + NameLine.Height - 10 - (SizeRatio / 5), 
                x2: Width + 100,
                y2: NameLine.Location.Y + NameLine.Height - 10 - (SizeRatio / 5));
            e.Graphics.DrawLine(pen: LinePen,
                x1: BirthLine.Location.X + BirthLine.Width,
                y1: BirthLine.Location.Y + BirthLine.Height - 10 - (SizeRatio / 5),
                x2: Width,
                y2: BirthLine.Location.Y + BirthLine.Height - 10 - (SizeRatio / 5));
            e.Graphics.DrawLine(pen: LinePen,
                x1: DepLine.Location.X + DepLine.Width,
                y1: DepLine.Location.Y + DepLine.Height - 10 - (SizeRatio / 5),
                x2: Width,
                y2: DepLine.Location.Y + DepLine.Height - 10 - (SizeRatio / 5));
            #endregion

            // Special Design around certain Controls
            foreach (Control control in Controls)
            {
                if (control.Name.StartsWith("LeftVacField"))
                {
                    e.Graphics.DrawString("=",
                        new Font(FrutigerFam, 12 + SizeRatio),
                        new SolidBrush(AppliedTheme.Secondary),
                        control.Location.X - 20 - (50 * (SizeRatio / 20)),
                        control.Location.Y);
                    e.Graphics.DrawLine(LinePen,
                        control.Location.X,
                        control.Location.Y + 1 + control.Height,
                        control.Location.X + control.Width,
                        control.Location.Y + 1 + control.Height);
                    string d = int.TryParse(control.Text, out int res) ?
                        res == 1 ?
                            vsf.cfg.CurrentLanguage.Day : vsf.cfg.CurrentLanguage.Days :
                        vsf.cfg.CurrentLanguage.Days;
                    e.Graphics.DrawString(d,
                        new Font(FrutigerFam, 12 + (int)Math.Round(SizeRatio * 0.8f)),
                        new SolidBrush(AppliedTheme.Secondary),
                        control.Location.X + control.Width,
                        control.Location.Y);
                }
                if(control.Name == "ReasonField")
                    e.Graphics.DrawLine(LinePen,
                        control.Location.X,
                        control.Location.Y + 1 + control.Height,
                        control.Location.X + control.Width,
                        control.Location.Y + 1 + control.Height);
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            HeightRatio = Height / ((float)xHeight);
            xHeight = Height;
            WidthRatio = Width / ((float)xWidth);
            xWidth = Width;
            int SizeRatio = (int)Math.Round((Height - 470) / 30.0f);

            string[] ScaleBlacklist =
            {
                "SettingsIcon"
            };

            foreach(Control c in Controls)
            {
                c.Font = new Font(c.Font.FontFamily, (int) Math.Round(c.Font.Size * HeightRatio), c.Font.Style);

                if (ScaleBlacklist.Contains(c.Name))                        // The Fontsize always scales with the Object, but some objects need to keep their dimensions
                    continue;

                c.Bounds = new Rectangle(
                    (int)Math.Round(c.Location.X * WidthRatio), 
                    (int)Math.Round(c.Location.Y * HeightRatio), 
                    (int)Math.Round(c.Width * WidthRatio), 
                    (int)Math.Round(c.Height * HeightRatio));
            }


            try
            {
                if(YearAnnouncement != null)
                    CurrentYearField.Location = new Point(YearAnnouncement.Location.X + YearAnnouncement.Width, YearAnnouncement.Location.Y);

                if(SettingsIcon != null)
                    SettingsIcon.Size = new Size((int)Math.Round(SettingsIcon.Width * HeightRatio), 
                                                (int)Math.Round(SettingsIcon.Height * HeightRatio));
            }
            catch { }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            int screenOffset = Screen.FromPoint(Cursor.Position).Bounds.Y;
            if (Cursor.Position.Y - screenOffset == 0)                  // Maximizes the Window, if it hits the upper Screenborder
                this.WindowState = FormWindowState.Maximized;
        }
    }
}
