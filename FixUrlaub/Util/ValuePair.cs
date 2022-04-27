using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    /// <summary>
    /// Object that houses two generic values to work with unsafe code without actually triggering the unsafe Code Warnings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    internal class ValuePair<T, U>
    {
        public T Value1 { get; set; }
        public U Value2 { get; set; }

        /// <summary>
        /// Creates instance of Object with two values that can be edited. Allows to be pointed at and still edited
        /// </summary>
        /// <param name="t"></param>
        /// <param name="u"></param>
        public ValuePair(T t, U u)
        {
            Value1 = t;
            Value2 = u;
        }
    }
}
