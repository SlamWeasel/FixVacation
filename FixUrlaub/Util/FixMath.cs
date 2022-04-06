using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    /// <summary>
    /// Internal Library with useful Math-Functions
    /// </summary>
    internal abstract class FixMath
    {
        public const double VacationFormularAspect = 147.0 / 106.0;

        /// <summary>
        /// Maps a given Value <paramref name="val"/> from a given source field onto a new field and scales the value with the field<br></br>
        /// The so-to-say "Percentage" of the value in the field stays the same through this process<br></br><br></br>
        /// For Example:<br></br>
        /// 1(0-2) ->(0;10)-> 5<br></br>
        /// 1(0-3) ->(0;10)-> 3,3333~<br></br>
        /// 4(0-3) ->(0;10)-> 13,3333~<br></br>
        /// 4(0-3) ->(-0,2;0,3)-> 0,46666~
        /// </summary>
        /// <param name="val">returns <see cref="double"/> if <paramref name="val"/> is a <see cref="double"/></param>
        /// <param name="sourceFloor"></param>
        /// <param name="sourceCeil"></param>
        /// <param name="destFloor"></param>
        /// <param name="destCeil"></param>
        /// <returns>Value of <paramref name="val"/> in the new field as <see cref="double"/></returns>
        public static double Map(double val, double sourceFloor, double sourceCeil, double destFloor, double destCeil)
            => ((destCeil - destFloor) * ((val - sourceFloor) / (sourceCeil - sourceFloor))) + destFloor;
        /// <summary>
        /// Maps a given Value <paramref name="val"/> from a given source field onto a new field and scales the value with the field and rounds it to a <see cref="int"/><br></br>
        /// The exact so-to-say "Percentage" of the value in the field may change through this process, but its roughly the same in big fields<br></br><br></br>
        /// For Example:<br></br>
        /// 1(0-2) ->(0;10)-> 5<br></br>
        /// 1(0-3) ->(0;10)-> 3<br></br>
        /// 4(0-3) ->(0;10)-> 13<br></br>
        /// 4(0-3) ->(-0,2;1,3)-> 2
        /// </summary>
        /// <param name="val">returns <see cref="int"/> if <paramref name="val"/> is a <see cref="int"/></param>
        /// <param name="sourceFloor"></param>
        /// <param name="sourceCeil"></param>
        /// <param name="destFloor"></param>
        /// <param name="destCeil"></param>
        /// <returns>Value of <paramref name="val"/> in the new field as <see cref="int"/> rounded</returns>
        public static int Map(int val, double sourceFloor, double sourceCeil, double destFloor, double destCeil)
            => (int)Math.Round(((destCeil - destFloor) * ((val - sourceFloor) / (sourceCeil - sourceFloor))) + destFloor, 0);

        /// <summary>
        /// Restricts a given value between the given <paramref name="floor"/>(lowest possible) and <paramref name="ceil"/>(highest possible). <br></br>
        /// If <paramref name="val"/> exceeds the border values, the corresponding border is returned
        /// </summary>
        /// <param name="val">returns <see cref="double"/> if <paramref name="val"/> is a <see cref="double"/></param>
        /// <param name="floor"></param>
        /// <param name="ceil"></param>
        /// <returns></returns>
        public static double Clamp(double val, double floor, double ceil)
        {
            if (val < floor)
                return floor;
            if (val > ceil)
                return ceil;
            return val;
        }
        /// <summary>
        /// Restricts a given value between the given <paramref name="floor"/>(lowest possible) and <paramref name="ceil"/>(highest possible). <br></br>
        /// If <paramref name="val"/> exceeds the border values, the corresponding border is returned
        /// </summary>
        /// <param name="val">returns <see cref="int"/> if <paramref name="val"/> is a <see cref="int"/></param>
        /// <param name="floor"></param>
        /// <param name="ceil"></param>
        /// <returns></returns>
        public static int Clamp(int val, int floor, int ceil)
        {
            if (val < floor)
                return floor;
            if (val > ceil)
                return ceil;
            return val;
        }
    }
}
