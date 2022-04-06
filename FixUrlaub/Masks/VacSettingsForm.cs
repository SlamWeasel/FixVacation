using FixUrlaub.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Masks
{
    internal class VacSettingsForm : VacPaperForm 
    {
        new public VacMainForm Parent;

        public Settings cfg;

        public VacSettingsForm()
        {
            Parent = (VacMainForm)base.Parent;
            cfg = new Settings();
            cfg.CurrentLanguage = Language.German;
        }
    }
}
