using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using FixUrlaub.Controls;
using FixUrlaub.Util;

namespace FixUrlaub.Masks
{
    internal class VacLeaderForm : VacPaperForm
    {
        private VacMainForm vacMainForm;

        public Label
            ExitIcon,
            JobPanel;
        public Button
            Allow,
            Deny,
            Calendar;

        public Job _selectedJob;
        /// <summary>
        /// Deselects the old Selection and selects the new Job when being set
        /// </summary>
        public Job SelectedJob
        {
            get => _selectedJob;
            set
            {
                _selectedJob.Selected = false;
                _selectedJob = value;
                value.Select();
            }
        }

        public VacLeaderForm(VacMainForm vacMainForm)
        {
            Language lang = vacMainForm.vsf.cfg.CurrentLanguage;

            this.vacMainForm = vacMainForm;
            AppliedTheme = vacMainForm.AppliedTheme;
            Console.WriteLine(AppliedTheme.ToString());

            Size = new Size(400, 600);

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
            ExitTip.SetToolTip(ExitIcon, vacMainForm.vsf.cfg.CurrentLanguage.Close);

            Controls.Add(ExitIcon);
            #endregion

            JobPanel = new Label()
            {
                Bounds = new Rectangle(20, 40, 360, 500),
                BorderStyle = BorderStyle.FixedSingle
            };
            JobPanel.MouseWheel += (sender, e) =>
            {
                if ((JobPanel.Controls[0].Location.Y < 0 || e.Delta < 0) 
                    && (JobPanel.Controls[JobPanel.Controls.Count-1].Location.Y + JobPanel.Controls[JobPanel.Controls.Count - 1].Height > JobPanel.Height || e.Delta > 0))
                    foreach (Control c in JobPanel.Controls)
                        c.Location = new Point(c.Location.X, c.Location.Y + (int)Math.Round(e.Delta / 5.0f));
            };

            #region Buttons
            Allow = new Button()
            {
                Name = "Allow",
                Text = lang.Allow,
                ForeColor = AppliedTheme.Secondary,
                BackColor = ColorTheme.AddColor(AppliedTheme.Primary, Color.FromArgb(20, 20, 20)),
                Font = new Font(FrutigerBoldFam, 12),
                FlatStyle = FlatStyle.Popup,
                Bounds = new Rectangle(20, 555, 100, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Allow.Click += (sender, e) =>
            {

            };
            Utils.AddHoverPointer(Allow);
            Deny = new Button()
            {
                Name = "Deny",
                Text = lang.Deny,
                ForeColor = AppliedTheme.Secondary,
                BackColor = ColorTheme.AddColor(AppliedTheme.Primary, Color.FromArgb(20, 20, 20)),
                Font = new Font(FrutigerBoldFam, 12),
                FlatStyle = FlatStyle.Popup,
                Bounds = new Rectangle(130, 555, 100, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Deny.Click += (sender, e) =>
            {
                try
                {
                    SelectedJob.Dispose();
                    //LoadJobs;                     // TODO: Deny Job in SQL and remove Control and properly reload the List
                }
                catch { }

            };
            Utils.AddHoverPointer(Deny);
            Calendar = new Button()
            {
                Name = "Calendar",
                Text = lang.Calendar,
                ForeColor = AppliedTheme.Secondary,
                BackColor = ColorTheme.AddColor(AppliedTheme.Primary, Color.FromArgb(20, 20, 20)),
                Font = new Font(FrutigerBoldFam, 12),
                FlatStyle = FlatStyle.Popup,
                Bounds = new Rectangle(280, 555, 100, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Calendar.Click += (sender, e) =>
            {
                VacCalendarForm vcf = new VacCalendarForm(vacMainForm, vacMainForm.vsf.cfg)
                {
                    Location = new Point(Location.X + Width, Location.Y)
                };

                vcf.Show();
                vcf.BringToFront();
            };
            Utils.AddHoverPointer(Calendar);
            #endregion

            Controls.Add(JobPanel);
            Controls.Add(Allow);
            Controls.Add(Deny);
            Controls.Add(Calendar);

            LoadJobs();                             // TODO: Load Jobs from SQL
        }

        public void LoadJobs()
        {
            for (int i = 0; i < 20; i++)
            {
                Job job = new Job(this, i * i, DateTime.Now, DateTime.Now.AddDays(i), "EVLehmann", "JSchuler", (byte)FixMath.Clamp(i, 1, 3))
                {
                    Location = new Point(5, 5 + (i * 75))
                };
                JobPanel.Controls.Add(job);
                job.MouseDown += OnJobMouseDown;
                Utils.AddHoverPointer(job);
            }
        }

        public void OnJobMouseDown(object sender, MouseEventArgs e)
        {
            // If the SelectedJob is not yet set, it sets it by hand
            try
            {
                SelectedJob = (Job)sender;
            }
            catch
            {
                _selectedJob = (Job)sender;
                _selectedJob.Select();
            }
        }
    }
}
