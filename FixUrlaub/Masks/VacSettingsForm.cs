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

        private int xHeight;
        private float HeightRatio = 0.462963f;
        private int xWidth;
        private float WidthRatio = 1;

        #region Controls
        public Label
            ExitIcon,
            LangLabel;
        public Button
            LangButtonEn,
            LangButtonDe,
            LangButtonCustom;
        #endregion

        public VacSettingsForm(VacMainForm sender, Settings set)
        {
            Parent = sender;
            cfg = set;
            Size = new Size(350, 600);

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

            Controls.Add(LangLabel);
        }
    }
}
