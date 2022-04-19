using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Masks
{
    internal class VacADLogin : VacPaperForm
    {
        private VacMainForm vacMainForm;

        public VacADLogin(VacMainForm vacMainForm)
        {
            this.vacMainForm = vacMainForm;
        }
    }
}
