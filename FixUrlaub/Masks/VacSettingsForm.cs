using FixUrlaub.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixUrlaub.Masks
{
    internal class VacSettingsForm : VacPaperForm 
    {
        new public VacMainForm Parent;
        public Settings cfg;

        OpenFileDialog dlg;

        #region Controls
        public Label
            ExitIcon,
            LangLabel,
            ColorLabel,
            PriColLabel,
            SecColLabel,
            TriColLabel;
        public Button
            LangEnButton,
            LangDeButton,
            LangCustomButton,
            DefColButton,
            WhiColButton,
            BlaColButton;
        #endregion


        public VacSettingsForm(VacMainForm sender, Settings set)
        {
            Parent = sender;
            cfg = set;
            Size = new Size(350, 600);
            BackColor = cfg.Theme.Primary;
            ForeColor = cfg.Theme.Secondary;

            LoadControls();
        }

        private void LoadControls()
        {
            Language lang = cfg.CurrentLanguage;

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
            ExitTip.SetToolTip(ExitIcon, cfg.CurrentLanguage.Close);

            Controls.Add(ExitIcon);
            #endregion

            #region Language
            LangLabel = new Label()
            {
                Text = lang.Lang,
                Name = "langLabel",
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(FrutigerBoldFam, 20f),
                ForeColor = AppliedTheme.Secondary,
                AutoSize = true
            };

            ToolTip EnTip = new ToolTip()
            {
                AutoPopDelay = 7500,
                InitialDelay = 500,
                ReshowDelay = 200
            };
            LangEnButton = new Button()
            {
                Text = "",
                Name = "LangButtonEn",
                Bounds = new Rectangle(20, 60, 40, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Blue,
                ForeColor = AppliedTheme.Secondary
            };
            LangEnButton.Paint += (sender, e) =>
                {
                    base.OnPaint(e);

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.White), 5),
                        -3, -2,
                        42, 30);
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.White), 5),
                        42, -2,
                        -3, 31);
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.Red), 1),
                        0, 1,
                        20, 16);
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.Red), 1),
                        40, 28,
                        20, 13);
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.Red), 1),
                        0, 30,
                        20, 15);
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.Red), 1),
                        38, 0,
                        20, 13);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.White), 8),
                        20, 0,
                        20, 30);
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.White), 8),
                        0, 15,
                        40, 15);
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.Red), 4),
                        20, 0,
                        20, 30);
                    e.Graphics.DrawLine(
                        new Pen(new SolidBrush(Color.Red), 4),
                        0, 15,
                        40, 15);
                };
            LangEnButton.MouseEnter += OnColorInvert;
            LangEnButton.MouseLeave += OnColorInvert;
            EnTip.SetToolTip(LangEnButton, Language.English.LangName);

            ToolTip DeTip = new ToolTip
            {
                AutoPopDelay = 7500,
                InitialDelay = 500,
                ReshowDelay = 200
            };
            LangDeButton = new Button()
            {
                Text = "",
                Name = "LangButtonEn",
                Bounds = new Rectangle(80, 60, 40, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = AppliedTheme.Secondary
            };
            LangDeButton.Paint += (sender, e) =>
                {
                    base.OnPaint(e);

                    e.Graphics.FillRectangle(
                        brush: new SolidBrush(Color.Black),
                        rect: new Rectangle(2, 2, Width - 4, ((Control)sender).Height / 3 + 3));
                    e.Graphics.FillRectangle(
                        brush: new SolidBrush(Color.Red),
                        rect: new Rectangle(2, ((Control)sender).Height / 3 + 2, Width - 4, (int)Math.Round(((Control)sender).Height / 1.5f) + 3));
                    e.Graphics.FillRectangle(
                        brush: new SolidBrush(Color.Gold),
                        rect: new Rectangle(2, (int)Math.Round(((Control)sender).Height / 1.5f) + 2, Width - 4, Height - 4));
                };
            LangDeButton.MouseEnter += OnColorInvert;
            LangDeButton.MouseLeave += OnColorInvert;
            DeTip.SetToolTip(LangDeButton, Language.German.LangName);

            LangCustomButton = new Button()
            {
                Text = lang.Custom,
                Name = "LangButtonCustom",
                Bounds = new Rectangle(140, 60, 100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = AppliedTheme.Primary,
                ForeColor = AppliedTheme.Secondary,
                Font = new Font(FrutigerFam, 10)
            };
            LangCustomButton.Click += OnLangButtonCustomClick;
            #endregion

            #region Color
            ColorLabel = new Label()
            {
                Text = lang.Color,
                Name = "ColorLabel",
                Location = new Point(20, 110),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(FrutigerBoldFam, 20f),
                ForeColor = AppliedTheme.Secondary,
                AutoSize = true
            };
            ToolTip PriTip = new ToolTip()
            {
                AutoPopDelay = 7500,
                InitialDelay = 500,
                ReshowDelay = 200
            };
            PriColLabel = new Label()
            {
                Name = "PriColLabel",
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.Black,
                Bounds = new Rectangle(20, 150, 30, 30),
                BackColor = AppliedTheme.Primary
            };
            PriColLabel.Click += (sender, e) =>
                {
                    ColorDialog colorDialog = new ColorDialog();
                    colorDialog.AllowFullOpen = true;
                    if(colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        cfg.Theme.Primary = colorDialog.Color;

                        Controls.Clear();
                        LoadControls();
                        Invalidate();
                    }
                };
            PriTip.SetToolTip(PriColLabel, lang.Pri);
            ToolTip SecTip = new ToolTip()
            {
                AutoPopDelay = 7500,
                InitialDelay = 500,
                ReshowDelay = 200
            };
            SecColLabel = new Label()
            {
                Name = "SecColLabel",
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.Black,
                Bounds = new Rectangle(60, 150, 30, 30),
                BackColor = AppliedTheme.Secondary
            };
            SecColLabel.Click += (sender, e) =>
                {
                    ColorDialog colorDialog = new ColorDialog();
                    colorDialog.AllowFullOpen = true;
                    if(colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        cfg.Theme.Secondary = colorDialog.Color;

                        Controls.Clear();
                        LoadControls();
                        Invalidate();
                    }
                };
            SecTip.SetToolTip(SecColLabel, lang.Sec);
            ToolTip TriTip = new ToolTip()
            {
                AutoPopDelay = 7500,
                InitialDelay = 500,
                ReshowDelay = 200
            };
            TriColLabel = new Label()
            {
                Name = "TriColLabel",
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.Black,
                Bounds = new Rectangle(100, 150, 30, 30),
                BackColor = AppliedTheme.Tertiary
            };
            TriColLabel.Click += (sender, e) =>
                {
                    ColorDialog colorDialog = new ColorDialog();
                    colorDialog.AllowFullOpen = true;
                    if(colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        cfg.Theme.Tertiary = colorDialog.Color;

                        Controls.Clear();
                        LoadControls();
                        Invalidate();
                    }
                };
            TriTip.SetToolTip(TriColLabel, lang.Tri);

            DefColButton = new Button()
            {
                Text = "Default",
                Name = "DefColButton",
                Bounds = new Rectangle(20, 190, 100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTheme.Default.Primary,
                ForeColor = ColorTheme.Default.Secondary,
                Font = new Font(FrutigerFam, 12)
            };
            DefColButton.Click += (sender, e) =>
                {
                    cfg.Theme = ColorTheme.Default;

                    Controls.Clear();
                    LoadControls();
                    Invalidate();
                };
            WhiColButton = new Button()
            {
                Text = "\"Paper\"",
                Name = "WhiColButton",
                Bounds = new Rectangle(120, 190, 100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTheme.White.Primary,
                ForeColor = ColorTheme.White.Secondary,
                Font = new Font(FrutigerFam, 12)
            };
            WhiColButton.Click += (sender, e) =>
                {
                    cfg.Theme = ColorTheme.White;

                    Controls.Clear();
                    LoadControls();
                    Invalidate();
                };
            BlaColButton = new Button()
            {
                Text = "\"Charcoal\"",
                Name = "BlaColButton",
                Bounds = new Rectangle(220, 190, 100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTheme.Dark.Primary,
                ForeColor = ColorTheme.Dark.Secondary,
                Font = new Font(FrutigerFam, 12)
            };
            BlaColButton.Click += (sender, e) =>
                {
                    cfg.Theme = ColorTheme.Dark;
                    cfg.Theme.Primary = ColorTheme.Dark.Primary;

                    Controls.Clear();
                    LoadControls();
                    Invalidate();
                };
            #endregion




            Controls.Add(LangLabel);
            Controls.Add(LangEnButton);
            Controls.Add(LangDeButton);
            Controls.Add(LangCustomButton);
            Controls.Add(ColorLabel);
            Controls.Add(PriColLabel);
            Controls.Add(SecColLabel);
            Controls.Add(TriColLabel);
            Controls.Add(DefColButton);
            Controls.Add(WhiColButton);
            Controls.Add(BlaColButton);
        }

        private void OnColorInvert(object sender, EventArgs e)
        {
            ((Control)sender).ForeColor = ColorTheme.InvertColor(((Control)sender).ForeColor);
        }

        private void OnLangButtonCustomClick(object sender, EventArgs e)
        {
            LangCustomButton.BeginInvoke(new Action(() =>
            {
                dlg = new OpenFileDialog()
                {
                    Filter = "Textfile (*.txt)|*.txt"
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    cfg.LanguageaugeOverride = dlg.FileName;
                    cfg.CurrentLanguage = new Language(dlg.FileName);

                    Controls.Clear();
                    LoadControls();
                    Invalidate();
                }
            }
            ));
        }
    }
}
