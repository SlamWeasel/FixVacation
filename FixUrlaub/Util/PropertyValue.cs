using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    /// <summary>
    /// Deserialization of a Directory Entry
    /// </summary>
    internal class PropertyValue
    {
        /// <summary>
        /// Canonical Name of the Active Directory Entry
        /// </summary>
        public string CN;
        /// <summary>
        /// Organizazional Units the Entry is in
        /// </summary>
        public string[] OU = new string[0];
        /// <summary>
        /// Domain Controllers of the Directory Entry
        /// </summary>
        public string[] DC = new string[0];

        /// <summary>
        /// Erstellt einen PropertyValue aus dem Value-Sting, z.B. "CN=abc,OU=main,OU=sub,DC=dc01"
        /// </summary>
        /// <param name="FullString"></param>
        public PropertyValue(string FullString)
        {
            char[] Seperators = { '=', ',' };
            string[] args = FullString.Split(Seperators);

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "CN")
                    this.CN = args[++i];
                else if (args[i] == "OU")
                    AddOU(args[++i]);
                else if (args[i] == "DC")
                    AddDC(args[++i]);
            }
        }

        /// <summary>
        /// Adds a lost of Organizational Units to the <see cref="PropertyValue"/> Object
        /// </summary>
        /// <param name="OUs"></param>
        public void AddOU(params string[] OUs)
        {
            foreach (string ou in OUs)
                if(ou != null || ou == "")
                    this.OU.Append(ou);
        }
        /// <summary>
        /// Adds a lost of Domain Controllers to the <see cref="PropertyValue"/> Object
        /// </summary>
        /// <param name="DCs"></param>
        public void AddDC(params string[] DCs)
        {
            foreach(string dc in DCs)
                if (dc != null || dc == "")
                    this.DC.Append(dc);
        }

        /// <summary>
        /// Brings the Property back into its DistinguishedName
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "DC=" + this.DC + ",OU=" + string.Join(",OU=", this.OU) + ",DC=" + string.Join(",DC=", this.DC);
    }
}
