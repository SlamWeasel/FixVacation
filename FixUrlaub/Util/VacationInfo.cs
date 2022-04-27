using System;
using System.Collections.Generic;

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
        public Dictionary<DateRange, float> TakenDays;

        /// <summary>
        /// Represents Data or Information about the Vacation of a User
        /// </summary>
        public VacationInfo()
        {
            TotalDays = 0;
            TakenDays = new Dictionary<DateRange, float>();
        }
        public VacationInfo(DateRange range, float rangeAmount) : this()
        {
            TakenDays.Add(range, rangeAmount);
        }
    }
}
