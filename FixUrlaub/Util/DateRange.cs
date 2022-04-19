using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    internal class DateRange
    {
        public DateTime Start;
        public DateTime End;
        /// <summary>
        /// The Total Amount of actual days between the Start and the End of the Range
        /// </summary>
        public int Days
        {
            get => (int)Math.Round((End - Start).TotalDays) + 1;
        }
        /// <summary>
        /// The Totoal Amount of workdays between the Start and the End of the Range. Saturdays and Sundays are excluded here
        /// </summary>
        public int WorkDays
        {
            get
            {
                int days = 0;
                if (End == Start)
                    return 1;
                for (int i = 0; (End - Start.AddDays(i)).TotalDays >= 0; i++)
                {
                    if (Start.AddDays(i).DayOfWeek == DayOfWeek.Sunday || Start.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                        continue;
                    days++;
                }
                return days;
            }
        }

        /// <summary>
        /// Creates an instance of a DateRange using <see cref="DateTime.Today"/>
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="_start"/>must be before or on the same day as <paramref name="_end"/></exception>
        public DateRange() : this(DateTime.Today, DateTime.Today){}
        /// <summary>
        /// Creates an instance of a DateRange between the <paramref name="_start"/> and the <paramref name="_end"/>
        /// </summary>
        /// <param name="_start"></param>
        /// <param name="_end"></param>
        /// <exception cref="ArgumentException"><paramref name="_start"/>must be before or on the same day as <paramref name="_end"/></exception>
        public DateRange(DateTime _start, DateTime _end)
        {
            if (_start.Date > _end.Date)
                throw new ArgumentException("The Start-DateTime has to be before the End. No negative Ranges Allowed!", new ArgumentOutOfRangeException());

            Start = _start.Date;
            End = _end.Date;
        }

        /// <summary>
        /// Checks, if the given Date is in the DateRange using a datestring <c>(day|month|year)</c>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsInRange(string input)
            => IsInRange(
                day:    int.Parse(input.Split('|')[0]), 
                month:  int.Parse(input.Split('|')[1]), 
                year:   int.Parse(input.Split('|')[2]));
        /// <summary>
        /// Checks, if the given Date is in the DateRange
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool IsInRange(int day, int month, int year)
            => IsInRange(new DateTime(year:year, month:month, day:day));
        /// <summary>
        /// Checks, if the given Date is in the DateRange using a <see cref="DateTime"/> as variable
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsInRange(DateTime date)
            => Start <= date && date <= End;

        /// <summary>
        /// Outputs the DateRange into a semantically readably string
        /// </summary>
        /// <returns>f.e. "03.05.2022 - 08.05.2022"</returns>
        public override string ToString()
            => Start.ToString("d") + " - " + End.ToString("d");
        /// <summary>
        /// Creates a DateRange from a string, the way it also gets turned to a string <para>"03.05.2022 - 08.05.2022" -> <see cref="DateRange"/></para>
        /// </summary>
        /// <param name="datestring"></param>
        /// <returns></returns>
        public static DateRange FromString(string datestring)
            => new DateRange(
                DateTime.Parse(datestring.Split(" - ".ToCharArray())[0]),
                DateTime.Parse(datestring.Split(" - ".ToCharArray())[1]));
    }
}
