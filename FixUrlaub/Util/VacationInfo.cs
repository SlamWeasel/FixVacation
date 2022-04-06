using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    internal class VacationInfo
    {
        /// <summary>
        /// Total Amount of Vacation
        /// </summary>
        public int TotalDays;

        /// <summary>
        /// Dictionary of taken Days. Can also include half days
        /// </summary>
        public Dictionary<DateTime, float> TakenDays;

        /// <summary>
        /// Represents Data or Information about the Vacation of a User
        /// </summary>
        public VacationInfo()
        {
            TotalDays = 0;
            TakenDays = new Dictionary<DateTime, float>();
        }

        public bool IsDayTaken(DateTime d)
            => TakenDays.ContainsKey(d);
        public bool EnoughDaysLeft(float DaysAmount)
        {
            float total = 0f;
            foreach (KeyValuePair<DateTime, float> day in TakenDays)
                total += day.Value;
            return TotalDays - total >= DaysAmount;
        }
    }
}
