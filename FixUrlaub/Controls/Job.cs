using FixUrlaub.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using FixUrlaub.Masks;

namespace FixUrlaub.Controls
{
    internal class Job : Label
    {
        public DateRange Time;
        public string Sender, Recipient, Reason = null;
        /// <summary>
        /// Flag to show the stage;
        /// <list type="number">
        /// <item>Has to be allowed by TeamLeader</item>
        /// <item>Has to be confirmed by HR</item>
        /// <item>Is fully allowed</item>
        /// </list>
        /// </summary>
        public byte Stage;

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                Invalidate();
            }
        }
        public new VacLeaderForm Parent;

        private ToolTip ReasonTip;

        /// <summary>
        /// A control that houses a Vacation acceptance Job and displays it
        /// </summary>
        /// <param name="p">Parent-Form of the Job</param>
        /// <param name="ID"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        /// <param name="reason"></param>
        /// <param name="stage">Flag to show the stage;
        /// <list type="number">
        /// <item>Has to be allowed by TeamLeader</item>
        /// <item>Has to be confirmed by HR</item>
        /// <item>Is fully allowed</item>
        /// </list></param>
        public Job(VacLeaderForm p, long ID, DateTime Start, DateTime End, string sender, string recipient, string reason, byte stage)
        {
            if (reason != null && reason != "")
            {
                Reason = reason;

                ReasonTip = new ToolTip()
                {
                    AutoPopDelay = 7500,
                    InitialDelay = 500,
                    ReshowDelay = 200,
                    UseFading = false,
                    UseAnimation = true,
                    ToolTipIcon = ToolTipIcon.Info,
                    OwnerDraw = false
                };
                ReasonTip.SetToolTip(this, reason);
            }

            Parent = p;
            Name = "Job#\n" + ID.ToString();
            Time = new DateRange(Start, End);
            Sender = sender;
            Recipient = recipient;
            Stage = stage;
            Cursor = Cursors.Hand;

            Size = new Size(350, 70);
            BorderStyle = BorderStyle.FixedSingle;

            LoadControls();
        }

        public void LoadControls()
        {
            Label IDLabel = new Label()
            {
                Name = "IDLabel",
                Text = Name,
                Bounds = new Rectangle(5, 5, 60, 60),
                Font = new Font(Parent.FrutigerBoldFam, 12),
            };
            Controls.Add(IDLabel);
            IDLabel.MouseDown += OnControlMouseDown;
            if(Reason != null)
                ReasonTip.SetToolTip(IDLabel, Reason);
            Label SenderLabel = new Label()
            {
                Name = "SenderLabel",
                Text = Sender + " > " + Recipient,
                Bounds = new Rectangle(70, 5, 200, 40),
                Font = new Font(Parent.FrutigerFam, 12),
            };
            Controls.Add(SenderLabel);
            SenderLabel.MouseDown += OnControlMouseDown;
            if (Reason != null)
                ReasonTip.SetToolTip(SenderLabel, Reason);
            Label RangeLabel = new Label()
            {
                Name = "RangeLabel",
                Text = Time.ToString(),
                Bounds = new Rectangle(70, 45, 200, 20),
                Font = new Font(Parent.FrutigerFam, 12),
            };
            Controls.Add(RangeLabel);
            RangeLabel.MouseDown += OnControlMouseDown;
            if (Reason != null)
                ReasonTip.SetToolTip(RangeLabel, Reason);
        }

        private void OnControlMouseDown(object sender, MouseEventArgs e)
        {
            Parent.OnJobMouseDown(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            switch (Stage)
            {
                case 3:
                    e.Graphics.FillRectangle(new SolidBrush(Parent.AppliedTheme.Tertiary),
                                        new Rectangle(320, 10, 20, 20));
                    e.Graphics.FillRectangle(new SolidBrush(Parent.AppliedTheme.Tertiary),
                                            new Rectangle(320, 40, 20, 20));
                    break;
                case 2:
                    e.Graphics.FillRectangle(new SolidBrush(Parent.AppliedTheme.Tertiary),
                                        new Rectangle(320, 10, 20, 20));
                    e.Graphics.DrawRectangle(new Pen(Parent.AppliedTheme.Tertiary, 1),
                                            new Rectangle(320, 40, 20, 20));
                    break;
                default:
                    e.Graphics.DrawRectangle(new Pen(Parent.AppliedTheme.Tertiary, 1),
                                        new Rectangle(320, 10, 20, 20));
                    e.Graphics.DrawRectangle(new Pen(Parent.AppliedTheme.Tertiary, 1),
                                            new Rectangle(320, 40, 20, 20));
                    break;
            }

            if (Selected)
                e.Graphics.DrawRectangle(new Pen(Parent.AppliedTheme.Secondary, 3),
                                        new Rectangle(0, 0, 347, 67));
        }
        protected override void Select(bool directed, bool forward)
        {
            base.Select(directed, forward);
            Selected = true;
        }
    }
}