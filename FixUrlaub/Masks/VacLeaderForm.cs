﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Masks
{
    internal class VacLeaderForm : VacPaperForm
    {
        private VacMainForm vacMainForm;

        public VacLeaderForm(VacMainForm vacMainForm)
        {
            this.vacMainForm = vacMainForm;
        }
    }
}
