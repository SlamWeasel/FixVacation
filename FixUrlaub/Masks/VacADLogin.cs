using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using FixUrlaub.Util;
using System.Data.SqlClient;
using FixUrlaub.Controls;

namespace FixUrlaub.Masks
{
    internal class VacADLogin : VacPaperForm
    {
        private TextBox
            TeamField;
        private DateTimePicker
            BirthDate;

        public ValuePair<DateTime?, string> keyVal;

        public VacADLogin(ADUser us, ref ValuePair<DateTime?, string> kv)
        {
            keyVal = kv;

            #region Buttons
            Button OK = new Button()
            {
                Text = "OK",
                Name = "OK",
                ForeColor = AppliedTheme.Secondary,
                BackColor = ColorTheme.AddColor(AppliedTheme.Primary, Color.FromArgb(20, 20, 20)),
                Font = new Font(FrutigerBoldFam, 12),
                FlatStyle = FlatStyle.Popup,
                Bounds = new Rectangle(20, 250, 70, 30),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            OK.Click += (_, _e) =>
            {
                if (TeamField.Modified)
                    keyVal.Value2 = TeamField.Text;
                keyVal.Value1 = BirthDate.Value;

                DialogResult = DialogResult.OK;
            };
            Utils.AddHoverPointer(OK);
            Button Abort = new Button()
            {
                Text = "Exit",
                Name = "Exit",
                ForeColor = AppliedTheme.Secondary,
                BackColor = ColorTheme.AddColor(AppliedTheme.Primary, Color.FromArgb(20, 20, 20)),
                Font = new Font(FrutigerBoldFam, 12),
                FlatStyle = FlatStyle.Popup,
                Bounds = new Rectangle(110, 250, 70, 30),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Abort.Click += (_, _e) => DialogResult = DialogResult.Cancel;
            Utils.AddHoverPointer(Abort);

            Controls.Add(OK);
            Controls.Add(Abort);
            #endregion

            #region Data Fields
            Label Name = new Label()
            {
                Text = "Name:",
                Name = "Name",
                Font = new Font(FrutigerBoldFam, 15),
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true
            };
            TextBox NameField = new TextBox()
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(
                                FixMath.Clamp(BackColor.R - 10, 0, 255),
                                FixMath.Clamp(BackColor.G - 10, 0, 255),
                                FixMath.Clamp(BackColor.B - 10, 0, 255)),
                Text = us.FullName,
                Name = "NameField",
                Bounds = new Rectangle(20, 35, 160, 30)
            };
            Label Team = new Label()
            {
                Text = "Team:",
                Name = "Team",
                Font = new Font(FrutigerBoldFam, 15),
                Location = new Point(10, 80),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true
            };
            TeamField = new TextBox()
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(
                                FixMath.Clamp(BackColor.R - 10, 0, 255),
                                FixMath.Clamp(BackColor.G - 10, 0, 255),
                                FixMath.Clamp(BackColor.B - 10, 0, 255)),
                Text = keyVal.Value2,
                Name = "TeamField",
                Bounds = new Rectangle(20, 105, 160, 30)
            };
            Label Birthday = new Label()
            {
                Text = "Geburtstag/\nBirthday:",
                Name = "Birthday",
                Font = new Font(FrutigerBoldFam, 15),
                Location = new Point(10, 150),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true
            };
            BirthDate = new DateTimePicker()
            {
                Name = "DatePicker",
                Bounds = new Rectangle(20, 200, 80, 15),
                CalendarFont = new Font(FrutigerFam, 8),
                Font = new Font(FrutigerFam, 8),
                CalendarForeColor = AppliedTheme.Secondary,
                CalendarTrailingForeColor = AppliedTheme.Secondary,
                CalendarMonthBackground = AppliedTheme.Primary,
                CalendarTitleBackColor = ColorTheme.InvertColor(AppliedTheme.Secondary),
                CalendarTitleForeColor = AppliedTheme.Secondary,
                Value = keyVal.Value1 ?? DateTime.Today,
                Format = DateTimePickerFormat.Short
            };

            Controls.Add(BirthDate);
            Controls.Add(TeamField);
            Controls.Add(NameField);
            Controls.Add(Birthday);
            Controls.Add(Team);
            Controls.Add(Name);
            #endregion
        }
    }
}
